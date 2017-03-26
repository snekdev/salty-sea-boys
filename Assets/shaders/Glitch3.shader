// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Glitch3"
{
	Properties
	{
		_MainTex ("_MainTex", 2D) = "white" {}
		_Color1("Color1", Color) = (1,1,1,1)
			_Color2("Color2", Color) = (1,1,1,1)

	//	_SecondaryTex("_SecondaryTex", 2D) = "white" {}
		//_Range("Range", Range(0,20)) = 0.0
			_WorldSpaceScale("_WorldSpaceScale", Float) = 0.0
			_Scale("Scale", Float) = 0.0
			_Rotation("_Rotation", Float) = 0.0
			_Dilation("_Dilation", Float) = 0.0
			_ModConst("_ModConst", Float) = 0.0
			_SinScale("_SinScale", Float) = 0.0


	//	_Percent("Percent", Range(0,1)) = 0.0
	//	_Point("_Point", Vector) = (0,0,0,0)

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 normal : NORMAL;

				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 wpos : TEXCOORD1;
				float3 vpos : TEXCOORD2;
			};

			sampler2D _MainTex;
		//	sampler2D _SecondaryTex;
			float4 _MainTex_ST;
		//	float	_Range;
			float	_Scale;
			float	_WorldSpaceScale;
			float	_ModConst;
			float	_SinScale;
			float	_Dilation;
			float	_Rotation;


		//	float	_Percent;
			float4	_Color1;
			float4	_Color2;

			
			v2f vert (appdata v)
			{   
				//v2f o;
			//	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			//	o.wpos = worldPos;

			//	float3 aaa = worldPos * _WorldSpaceScale;

			//	float aaa1 = aaa.x + aaa.y + aaa.z + sin(_Dilation *_Time.y);
			//	v.vertex.xyz +=  v.normal *(  _SinScale *sin(fmod(aaa1, _ModConst)) - .07);
				float s = sin(_Rotation);
				float c = cos(_Rotation);
				float2x2 rotationMatrix = float2x2(c, -s, s, c);

				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				float offsetX = .5; //_MainTex_ST.z +_MainTex_ST.x / 2;
				float offsetY = .5; //_MainTex_ST.w +_MainTex_ST.y / 2;

				float x = v.texcoord.x - offsetX; //* _MainTex_ST.x + _MainTex_ST.z - offsetX;
				float y = v.texcoord.y - offsetY; //* _MainTex_ST.y + _MainTex_ST.w - offsetY;

				o.uv = mul(float2(x, y), rotationMatrix) + float2(offsetX, offsetY);

				return o;

				//float sinX = sin(_RotationSpeed * _Time.y);
				//float cosX = cos(_RotationSpeed * _Time.y);
				//float sinY = sin(_RotationSpeed * _Time.y);
				//float2x2 rotationMatrix = float2x2(cosX, -sinX, sinY, cosX);
				//v.uv.xy = mul(v.uv.xy, rotationMatrix);
				 
			//	o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			//	UNITY_TRANSFER_FOG(o,o.vertex);
				//return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

			//	float4 aaa = mul(unity_ObjectToWorld, i.vertex); 
		//		i.wpos *= _WorldSpaceScale;
				//col = lerp(_Color1, _Color2, fmod( .3 *( i.wpos.x + i.wpos.y +  i.wpos.z + _SinScale * sin(_Dilation * _Time.y)), 1));
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
