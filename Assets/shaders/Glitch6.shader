// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Glitch6"
{
	Properties
	{
		_MainTex ("_MainTex", 2D) = "white" {}
		_Color1("Color1", Color) = (1,1,1,1)
			_Color2("Color2", Color) = (1,1,1,1)

	//	_SecondaryTex("_SecondaryTex", 2D) = "white" {}
		//_Range("Range", Range(0,20)) = 0.0
			_WorldSpaceScale("_WorldSpaceScale", Float) = 0.0
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
				float2 uv : TEXCOORD0;
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
			float	_WorldSpaceScale;
			float	_ModConst;
			float	_SinScale;
			float	_Dilation;


		//	float	_Percent;
			float4	_Color1;
			float4	_Color2;

			
			v2f vert (appdata v)
			{  
				v2f o;
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.wpos = worldPos;

				float3 aaa = worldPos * _WorldSpaceScale;

				float bbb = sin(_Time.y) + sin(worldPos.x * _WorldSpaceScale) + sin(worldPos.y * _WorldSpaceScale) + sin(worldPos.z * _WorldSpaceScale);

			float aaa1 = aaa.x + aaa.y + aaa.z + sin(_Dilation *_Time.y);
				v.vertex.xyz += _SinScale *sin(fmod(aaa1, _ModConst));
 


				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				float4 aaa = mul(unity_ObjectToWorld, i.vertex); 
				i.wpos *= _WorldSpaceScale;
				float bbb = sin(_Time.y) + sin(i.wpos.x) + sin(i.wpos.y) + sin(i.wpos.z);
				//float bbb = fmod(abs(sin(_Dilation *(i.wpos.y + sin(_Time.y)))), .4);// -.5 *sin(i.wpos.y);

				col = lerp(_Color1, _Color2, fmod(bbb, 1));

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
