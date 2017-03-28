Shader "Hidden/Snekdev/ImageEffects/ScreenTint" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Strength("Strength", Range(0,1)) = 0.5
	}

	SubShader {
		Pass {
			Tags { "LightMode" = "Always" }
			ZTest Always Cull Off ZWrite Off Fog { Mode off }

			CGPROGRAM
			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			float4	_Color;
			float	_Strength;

			v2f_img vert(appdata_img v) {
				v2f_img o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				#ifdef UNITY_HALF_TEXEL_OFFSET
					v.texcoord.y += _MainTex_TexelSize.y;
				#endif
				#if SHADER_API_D3D9
					if (_MainTex_TexelSize.y < 0)
						v.texcoord.y = 1.0 - v.texcoord.y;
				#endif
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				return o;
			}

			fixed4 frag (v2f_img i) : COLOR {
				return  lerp(tex2D(_MainTex, i.uv),tex2D(_MainTex, i.uv) * _Color, _Strength);

			}
			ENDCG
		}
	}

	Fallback off
}