Shader "GPUTools/Hair"
{
	Properties
	{
		_ColorTex("Color Tex (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" "DisableBatching" = "True" }
		LOD 100

		Pass
		{
			Name "ForwardBase"

			Tags{ "LightMode" = "ForwardBase" }
		
			ZWrite On
			Offset -1, -1
			//ZTest Less

			CGPROGRAM
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "UnityCG.cginc"
			#include "Include/HairTypes.cginc"

			#pragma target 5.0
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#pragma vertex VS
			#pragma hull HS
			#pragma domain DS
			#pragma geometry GS
			#pragma fragment FS

			half4 _Size;
			half4 _TessFactor;
			float3 _LightCenter;

			uniform StructuredBuffer<Body> _BodiesBuffer;
			uniform StructuredBuffer<BodyData> _BodiesDataBuffer;
			uniform StructuredBuffer<fixed3> _BarycentricBuffer;

			half _StandWidth;

			fixed4 _Length;

			fixed _SpecularShift;
			half _PrimarySpecular;
			half _SecondarySpecular;
			fixed4 _SpecularColor;

			half3 _WavinessAxis;

			sampler2D _ColorTex;

			// ***************************************************************
			// Programs
			// ***************************************************************

			VS_OUTPUT VS(uint id:SV_VertexID)
			{
				VS_OUTPUT o;
				o.id = id;
				return o;
			}

			HS_CONSTANT_OUTPUT HSConst()
			{
				HS_CONSTANT_OUTPUT output;

				output.edges[0] = _TessFactor.x; // Detail factor
				output.edges[1] = _TessFactor.y; // Density factor

				return output;
			}

			[domain("isoline")]
			[partitioning("integer")]
			[outputtopology("line")]
			[outputcontrolpoints(3)]
			[patchconstantfunc("HSConst")]
			HS_OUTPUT HS(InputPatch<VS_OUTPUT, 3> ip, uint id : SV_OutputControlPointID)
			{
				HS_OUTPUT output;
				output.id = ip[id].id;
				return output;
			}

			StepData GetPosition(OutputPatch<HS_OUTPUT, 3> op, fixed2 uv)
			{
				fixed3 barycentric = _BarycentricBuffer[uv.y * 64];

				fixed signV = sign(uv.x - _TessFactor.z);
				fixed downUvX = uv.x - _TessFactor.z*signV;

				fixed length = GetBarycentricFixed(_Length.x, _Length.y, _Length.z, barycentric);

				fixed length1 = length*uv.x;
				fixed length2 = length*downUvX;

				float3 p1 = GetSplinePoint(op[0].id, length1, _BodiesBuffer, _Size.y);
				float3 p2 = GetSplinePoint(op[1].id, length1, _BodiesBuffer, _Size.y);
				float3 p3 = GetSplinePoint(op[2].id, length1, _BodiesBuffer, _Size.y);

				float3 p1Next = GetSplinePoint(op[0].id, length2, _BodiesBuffer, _Size.y);
				float3 p2Next = GetSplinePoint(op[1].id, length2, _BodiesBuffer, _Size.y);
				float3 p3Next = GetSplinePoint(op[2].id, length2, _BodiesBuffer, _Size.y);

				BodyData b1 = GetSplineBodyData(op[0].id, length1, _BodiesDataBuffer, _Size.y);
				BodyData b1Next = GetSplineBodyData(op[0].id, length2, _BodiesDataBuffer, _Size.y);

				half3 wavinessAxis = mul(unity_ObjectToWorld, half4(_WavinessAxis, 0));
				float3 curve = CurveDirrection(wavinessAxis, half3(uv.x, 0, op[0].id), b1.wavinessScale, b1.wavinessFrequency);
				float3 curveNext = CurveDirrection(wavinessAxis, half3(downUvX, 0, op[0].id), b1Next.wavinessScale, b1Next.wavinessFrequency);

				float3 position = GetBarycentric(p1, p2, p3, barycentric);
				float3 positionNext = GetBarycentric(p1Next, p2Next, p3Next, barycentric);

				position = lerp(position, p1, b1.interpolation);
				positionNext = lerp(positionNext, p1Next, b1Next.interpolation);

				position += curve;
				positionNext += curveNext;

				StepData data;
				data.position = position;
				data.tangent = normalize(position - positionNext)*signV;
				data.color = b1.color;

				return data;
			}

			float4 LightData()
			{
				return float4(_WorldSpaceLightPos0.xyz, 1);
			}

			[domain("isoline")]
			DS_OUTPUT DS(HS_CONSTANT_OUTPUT input, OutputPatch<HS_OUTPUT, 3> op, float2 uv : SV_DomainLocation)
			{
				DS_OUTPUT output;

				StepData step = GetPosition(op, uv);

				float4 lightData = LightData();
				float3 lightDir = lightData.xyz;
				half attenuation = lightData.w;

				attenuation *= Diffuse(normalize(step.position - _LightCenter), lightDir, 0.025f);//head shadow

				half shift = saturate(tex2Dlod(_ColorTex, half4(uv.yx, 0, 0)).r - 0.5);
				fixed thickness = 1 - pow(2, -10 * (1 - uv.x));//curve

				output.position = float4(step.position, 1);
				output.tangent = step.tangent;
				output.normal = cross(step.tangent, cross(lightDir, step.tangent));
				output.viewDir = normalize(_WorldSpaceCameraPos.xyz - step.position);
				output.lightDir = lightDir;
				output.factor = half4(attenuation, shift, 0, 0);
				output.right = normalize(cross(step.tangent, output.viewDir))*thickness*_StandWidth;
				output.color = step.color;

				return output;
			}

			GS_OUTPUT CopyToFragment(DS_OUTPUT i, float4 position)
			{
				GS_OUTPUT output;

				output.pos = position;
				output.tangent = i.tangent;
				output.normal = i.normal;
				output.viewDir = i.viewDir;
				output.lightDir = i.lightDir;
				output.factor = i.factor;
				output.color = i.color;
				TRANSFER_VERTEX_TO_FRAGMENT(output);
				UNITY_TRANSFER_FOG(output, output.pos);
				return output;
			}

			// Geometry Shader -----------------------------------------------------
			[maxvertexcount(4)]
			void GS(line DS_OUTPUT p[2], inout TriangleStream<GS_OUTPUT> triStream)
			{
				float4 v[4];
				v[0] = float4(p[0].position + p[0].right, 1);
				v[1] = float4(p[1].position + p[1].right, 1);
				v[2] = float4(p[0].position - p[0].right, 1);
				v[3] = float4(p[1].position - p[1].right, 1);

				float4x4 vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);

				triStream.Append(CopyToFragment(p[0], mul(vp, v[0])));
				triStream.Append(CopyToFragment(p[1], mul(vp, v[1])));
				triStream.Append(CopyToFragment(p[0], mul(vp, v[2])));
				triStream.Append(CopyToFragment(p[1], mul(vp, v[3])));
			}


			void FS(GS_OUTPUT i, out float4 final:SV_Target, out float depth : DEPTH)
			{
				fixed3 lightColor = _LightColor0*LIGHT_ATTENUATION(i)*i.factor.x;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb*i.color;
				fixed3 diffuse = Diffuse(i.normal, i.lightDir, 0.25)*i.color;
				fixed3 specular = SpecularColor(i, _SpecularShift, _PrimarySpecular, _SecondarySpecular, _SpecularColor);
				
				final = fixed4((diffuse + specular)*lightColor + ambient, 1);
				depth = i.pos.z;
				UNITY_APPLY_FOG(i.fogCoord, final);
			}
			ENDCG
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Forward add pass

		Pass
		{
			Name "ForwardAdd"
			Blend One One

			Tags{ "LightMode" = "ForwardAdd" }

			CGPROGRAM
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "UnityCG.cginc"
			#include "Include/HairTypes.cginc"


			#pragma target 5.0
			#pragma multi_compile_fog
			#pragma multi_compile_lightpass

			#pragma vertex VS
			#pragma hull HS
			#pragma domain DS
			#pragma geometry GS
			#pragma fragment FS

			half4 _Size;
			half4 _TessFactor;
			float3 _LightCenter;

			uniform StructuredBuffer<Body> _BodiesBuffer;
			uniform StructuredBuffer<BodyData> _BodiesDataBuffer;
			uniform StructuredBuffer<fixed3> _BarycentricBuffer;

			half _StandWidth;

			fixed4 _Length;

			fixed _SpecularShift;
			half _PrimarySpecular;
			half _SecondarySpecular;
			fixed4 _SpecularColor;

			half3 _WavinessAxis;

			sampler2D _ColorTex;
			

			// ***************************************************************
			// Programs
			// ***************************************************************

			VS_OUTPUT VS(uint id:SV_VertexID)
			{
				VS_OUTPUT o;
				o.id = id;
				return o;
			}

			HS_CONSTANT_OUTPUT HSConst()
			{
				HS_CONSTANT_OUTPUT output;

				output.edges[0] = _TessFactor.x; // Detail factor
				output.edges[1] = _TessFactor.y; // Density factor

				return output;
			}


			[domain("isoline")]
			[partitioning("integer")]
			[outputtopology("line")]
			[outputcontrolpoints(3)]
			[patchconstantfunc("HSConst")]
			HS_OUTPUT HS(InputPatch<VS_OUTPUT, 3> ip, uint id : SV_OutputControlPointID)
			{
				HS_OUTPUT output;
				output.id = ip[id].id;
				return output;
			}

			StepData GetPosition(OutputPatch<HS_OUTPUT, 3> op, fixed2 uv)
			{
				fixed3 barycentric = _BarycentricBuffer[uv.y * 64];

				fixed signV = sign(uv.x - _TessFactor.z);
				fixed downUvX = uv.x - _TessFactor.z*signV;

				fixed length = GetBarycentricFixed(_Length.x, _Length.y, _Length.z, barycentric);

				fixed length1 = length*uv.x;
				fixed length2 = length*downUvX;

				float3 p1 = GetSplinePoint(op[0].id, length1, _BodiesBuffer, _Size.y);
				float3 p2 = GetSplinePoint(op[1].id, length1, _BodiesBuffer, _Size.y);
				float3 p3 = GetSplinePoint(op[2].id, length1, _BodiesBuffer, _Size.y);

				float3 p1Next = GetSplinePoint(op[0].id, length2, _BodiesBuffer, _Size.y);
				float3 p2Next = GetSplinePoint(op[1].id, length2, _BodiesBuffer, _Size.y);
				float3 p3Next = GetSplinePoint(op[2].id, length2, _BodiesBuffer, _Size.y);

				BodyData b1 = GetSplineBodyData(op[0].id, length1, _BodiesDataBuffer, _Size.y);
				BodyData b1Next = GetSplineBodyData(op[0].id, length2, _BodiesDataBuffer, _Size.y);

				half3 wavinessAxis = mul(unity_ObjectToWorld, half4(_WavinessAxis, 0));
				float3 curve = CurveDirrection(wavinessAxis, half3(uv.x, 0, op[0].id), b1.wavinessScale, b1.wavinessFrequency);
				float3 curveNext = CurveDirrection(wavinessAxis, half3(downUvX, 0, op[0].id), b1Next.wavinessScale, b1Next.wavinessFrequency);

				float3 position = GetBarycentric(p1, p2, p3, barycentric);
				float3 positionNext = GetBarycentric(p1Next, p2Next, p3Next, barycentric);

				position = lerp(position, p1, b1.interpolation);
				positionNext = lerp(positionNext, p1Next, b1Next.interpolation);

				position += curve;
				positionNext += curveNext;

				StepData data;
				data.position = position;
				data.tangent = normalize(position - positionNext)*signV;
				data.color = b1.color;

				return data;
			}

			float4 LightData(float3 position)
			{
				float attenuation = 1;
				float3 lightDirection;
#if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);
#elif defined (POINT_NOATT)
				lightDirection = normalize(_WorldSpaceLightPos0 - position.xyz);
#elif defined(POINT)||defined(POINT_COOKIE)||defined(SPOT)
				float3 vertexToLightSource =_WorldSpaceLightPos0.xyz - position.xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / (1 + distance);
				lightDirection = normalize(vertexToLightSource);
#endif
				return float4(lightDirection, attenuation);
			}

			[domain("isoline")]
			DS_OUTPUT DS(HS_CONSTANT_OUTPUT input, OutputPatch<HS_OUTPUT, 3> op, float2 uv : SV_DomainLocation)
			{
				DS_OUTPUT output;

				StepData step = GetPosition(op, uv);

				float4 lightData = LightData(step.position);
				float3 lightDir = lightData.xyz;
				half attenuation = lightData.w;

				attenuation *= Diffuse(normalize(step.position - _LightCenter), lightDir, 0.025f);//head shadow

				half shift = saturate(tex2Dlod(_ColorTex, half4(uv.yx, 0, 0)).r - 0.5);
				float thickness = 1 - pow(2, -10 * (1 - uv.x));

				output.position = float4(step.position, 1);
				output.tangent = step.tangent;
				output.normal = cross(step.tangent, cross(lightDir, step.tangent));
				output.viewDir = normalize(_WorldSpaceCameraPos.xyz - step.position);
				output.lightDir = lightDir;
				output.factor = half4(attenuation, shift, 0, 0);
				output.right = normalize(cross(step.tangent, output.viewDir))*thickness*_StandWidth;
				output.color = step.color;
				
				return output;
			}

			GS_OUTPUT_LIGHT CopyToFragment(DS_OUTPUT i, float4 position, float4 wPosition)///////////////////////////////////////////////////////////////////////copy paste
			{
				GS_OUTPUT_LIGHT output;

				output.pos = position;
				output.tangent = i.tangent;
				output.normal = i.normal;
				output.viewDir = i.viewDir;
				output.lightDir = i.lightDir;
				output.factor = i.factor;
				output.color = i.color;

#if defined (POINT_COOKIE) || defined (DIRECTIONAL_COOKIE) || defined (SPOT)
				output.lightPos = mul(unity_WorldToLight, wPosition);
#else
				output.lightPos = float4(0,0,0,0);
#endif
				return output;
			}

			// Geometry Shader -----------------------------------------------------
			[maxvertexcount(4)]
			void GS(line DS_OUTPUT p[2], inout TriangleStream<GS_OUTPUT_LIGHT> triStream)
			{
				float4 v[4];
				v[0] = float4(p[0].position + p[0].right, 1);
				v[1] = float4(p[1].position + p[1].right, 1);
				v[2] = float4(p[0].position - p[0].right, 1);
				v[3] = float4(p[1].position - p[1].right, 1);

				float4x4 vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);

				triStream.Append(CopyToFragment(p[0], mul(vp, v[0]), v[0]));
				triStream.Append(CopyToFragment(p[1], mul(vp, v[1]), v[1]));
				triStream.Append(CopyToFragment(p[0], mul(vp, v[2]), v[2]));
				triStream.Append(CopyToFragment(p[1], mul(vp, v[3]), v[3]));
			}

			fixed4 FS(GS_OUTPUT_LIGHT i) : SV_Target
			{
				fixed cookieAttenuation = 1.0;

#if defined (DIRECTIONAL_COOKIE)
				cookieAttenuation = tex2D(_LightTexture0, i.lightPos.xy).a;
#elif defined (POINT_COOKIE)
				cookieAttenuation = texCUBE(_LightTexture0, i.lightPos.xyz).a;
#elif defined (SPOT)
				cookieAttenuation = tex2D(_LightTexture0, i.lightPos.xy / i.lightPos.w + float2(0.5, 0.5)).a;
#endif

				fixed3 lightColor = _LightColor0*i.factor.x*cookieAttenuation;
				fixed3 diffuse = Diffuse(i.normal, i.lightDir, 0.25)*i.color;
				fixed3 specular = SpecularColorLight(i, _SpecularShift, _PrimarySpecular, _SecondarySpecular, _SpecularColor);

				return fixed4((diffuse + specular)*lightColor, 1);
			}
			ENDCG
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Shadow pass

		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }

			CGPROGRAM
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "UnityCG.cginc"
			#include "Include/HairTypes.cginc"


			#pragma target 5.0
			#pragma multi_compile_fog
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_fwdbase
			#pragma shader_feature _ _ALPHATEST_OFF _ALPHABLEND_OFF _ALPHAPREMULTIPLY_OFF
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma vertex VS
			#pragma hull HS
			#pragma domain DS
			#pragma geometry GS
			#pragma fragment FS

			half4 _Size;
			half4 _TessFactor;
			float3 _LightCenter;

			uniform StructuredBuffer<Body> _BodiesBuffer;
			uniform StructuredBuffer<BodyData> _BodiesDataBuffer;
			uniform StructuredBuffer<fixed3> _BarycentricBuffer;

			half _StandWidth;

			fixed4 _Length;

			fixed _SpecularShift;
			half _PrimarySpecular;
			half _SecondarySpecular;
			fixed4 _SpecularColor;

			half3 _WavinessAxis;

			sampler2D _ColorTex;

			// ***************************************************************
			// Programs
			// ***************************************************************

			VS_OUTPUT VS(uint id:SV_VertexID)
			{
				VS_OUTPUT o;
				o.id = id;
				return o;
			}

			HS_CONSTANT_OUTPUT HSConst()
			{
				HS_CONSTANT_OUTPUT output;

				output.edges[0] = _TessFactor.x; // Detail factor
				output.edges[1] = _TessFactor.y; // Density factor

				return output;
			}

			[domain("isoline")]
			[partitioning("integer")]
			[outputtopology("line")]
			[outputcontrolpoints(3)]
			[patchconstantfunc("HSConst")]
			HS_OUTPUT HS(InputPatch<VS_OUTPUT, 3> ip, uint id : SV_OutputControlPointID)
			{
				HS_OUTPUT output;
				output.id = ip[id].id;
				return output;
			}

			StepData GetPosition(OutputPatch<HS_OUTPUT, 3> op, fixed2 uv)
			{
				fixed3 barycentric = _BarycentricBuffer[uv.y * 64];

				fixed signV = sign(uv.x - _TessFactor.z);
				fixed downUvX = uv.x - _TessFactor.z*signV;

				fixed length = GetBarycentricFixed(_Length.x, _Length.y, _Length.z, barycentric);

				fixed length1 = length*uv.x;
				fixed length2 = length*downUvX;

				float3 p1 = GetSplinePoint(op[0].id, length1, _BodiesBuffer, _Size.y);
				float3 p2 = GetSplinePoint(op[1].id, length1, _BodiesBuffer, _Size.y);
				float3 p3 = GetSplinePoint(op[2].id, length1, _BodiesBuffer, _Size.y);

				float3 p1Next = GetSplinePoint(op[0].id, length2, _BodiesBuffer, _Size.y);
				float3 p2Next = GetSplinePoint(op[1].id, length2, _BodiesBuffer, _Size.y);
				float3 p3Next = GetSplinePoint(op[2].id, length2, _BodiesBuffer, _Size.y);

				BodyData b1 = GetSplineBodyData(op[0].id, length1, _BodiesDataBuffer, _Size.y);
				BodyData b1Next = GetSplineBodyData(op[0].id, length2, _BodiesDataBuffer, _Size.y);

				half3 wavinessAxis = mul(unity_ObjectToWorld, half4(_WavinessAxis, 0));
				float3 curve = CurveDirrection(wavinessAxis, half3(uv.x, 0, op[0].id), b1.wavinessScale, b1.wavinessFrequency);
				float3 curveNext = CurveDirrection(wavinessAxis, half3(downUvX, 0, op[0].id), b1Next.wavinessScale, b1Next.wavinessFrequency);

				float3 position = GetBarycentric(p1, p2, p3, barycentric);
				float3 positionNext = GetBarycentric(p1Next, p2Next, p3Next, barycentric);

				position = lerp(position, p1, b1.interpolation);
				positionNext = lerp(positionNext, p1Next, b1Next.interpolation);

				position += curve;
				positionNext += curveNext;

				StepData data;
				data.position = position;
				data.tangent = normalize(position - positionNext)*signV;
				data.color = b1.color;

				return data;
			}

			[domain("isoline")]
			DS_OUTPUT DS(HS_CONSTANT_OUTPUT input, OutputPatch<HS_OUTPUT, 3> op, float2 uv : SV_DomainLocation)
			{
				DS_OUTPUT output;

				StepData step = GetPosition(op, uv);

				float thickness = 1 - pow(2, -10 * (1 - uv.x));

				output.position = float4(step.position, 1);
				output.tangent = step.tangent;
				output.factor = half4(0, 0, uv.x, 0);
				output.normal = half3(0, 0, 0);
				output.viewDir = normalize(_WorldSpaceCameraPos.xyz - step.position);
				output.lightDir = float3(0, 0, 0);
				output.right = normalize(cross(step.tangent, output.viewDir))*thickness*_StandWidth;
				output.color = fixed3(0,0,0);

				return output;
			}

			GS_OUTPUT_SHADOW CopyToFragment(DS_OUTPUT i, float4 position)
			{
				GS_OUTPUT_SHADOW output;

				output.pos = position;
				TRANSFER_VERTEX_TO_FRAGMENT(output);
				return output;
			}

			// Geometry Shader -----------------------------------------------------
			[maxvertexcount(4)]
			void GS(line DS_OUTPUT p[2], inout TriangleStream<GS_OUTPUT_SHADOW> triStream)
			{
				float4 v[4];
				v[0] = float4(p[0].position + p[0].right, 1);
				v[1] = float4(p[1].position + p[1].right, 1);
				v[2] = float4(p[0].position - p[0].right, 1);
				v[3] = float4(p[1].position - p[1].right, 1);

				float4x4 vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);

				triStream.Append(CopyToFragment(p[0], mul(vp, v[0])));
				triStream.Append(CopyToFragment(p[1], mul(vp, v[1])));
				triStream.Append(CopyToFragment(p[0], mul(vp, v[2])));
				triStream.Append(CopyToFragment(p[1], mul(vp, v[3])));
			}

			fixed4 FS(GS_OUTPUT_SHADOW i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
				return fixed4(1, 1, 1, 1);
			}
			ENDCG
		}
	}

	Fallback "VertexLit"
}
