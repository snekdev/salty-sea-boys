Shader "Unlit/coral1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color1("Color1", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (1,1,1,1)

		_WorldSpaceScale("_WorldSpaceScale", Float) = 0.0
		_BumpScale("BumpScale", Float) = 0.0
		//_Dilation("_Dilation", Float) = 0.0
		//_ModConst("_ModConst", Float) = 0.0
		//_SinScale("_SinScale", Float) = 0.0

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float	_BumpScale;
			float	_WorldSpaceScale;
		//	float	_ModConst;
			//float	_SinScale;
			//float	_Dilation;

			float4	_Color1;
			float4	_Color2;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPosition : TEXCOORD1;
			};

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				float3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 aaa = worldPosition * _WorldSpaceScale;
				 
				o.worldPosition = worldPosition;
				//_SinScale *
				v.vertex.xz += sin(aaa.y);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
