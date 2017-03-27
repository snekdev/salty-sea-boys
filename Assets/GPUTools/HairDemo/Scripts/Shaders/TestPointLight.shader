// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "GPUTools/TestMultiLight" {
	Properties{
		_Color("Diffuse Material Color", Color) = (1,1,1,1)
	}
		SubShader{
	Pass{
		Tags{ "LightMode" = "ForwardBase" }
		// pass for first light source

		CGPROGRAM

#pragma vertex vert  
#pragma fragment frag 

#include "UnityCG.cginc"

		uniform float4 _LightColor0;
	// color of light source (from "Lighting.cginc")

	uniform float4 _Color; // define shader property for shaders

	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 col : COLOR;
	};

	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;

		float4x4 modelMatrix = unity_ObjectToWorld;
		float4x4 modelMatrixInverse = unity_WorldToObject;

		float3 normalDirection = normalize(
			mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
		float3 lightDirection;
		float attenuation;

		attenuation = 1.0; // no attenuation
		lightDirection = normalize(_WorldSpaceLightPos0.xyz);

		float3 diffuseReflection =
			attenuation * _LightColor0.rgb * _Color.rgb
			* max(0.0, dot(normalDirection, lightDirection));

		output.col = float4(diffuseReflection, 1.0);
		output.pos = mul(UNITY_MATRIX_MVP, input.vertex += float4(input.normal, 0));
		return output;
	}

	float4 frag(vertexOutput input) : COLOR
	{
		return fixed4(0,1,0,1);
	}

		ENDCG
	}

		Pass{
		Tags{ "LightMode" = "ForwardAdd" }
		// pass for additional light sources
		Blend One One // additive blending 

		CGPROGRAM

#pragma vertex vert  
#pragma fragment frag 

#include "UnityCG.cginc"

		uniform float4 _LightColor0;
	// color of light source (from "Lighting.cginc")

	uniform float4 _Color; // define shader property for shaders

	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 col : COLOR;
	};

	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;

		float4x4 modelMatrix = unity_ObjectToWorld;
		float4x4 modelMatrixInverse = unity_WorldToObject;

		float3 normalDirection = normalize(
			mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
		
		/*float3 lightDirection;
		float attenuation;

		if (0.0 == _WorldSpaceLightPos0.w) // directional light?
		{
			attenuation = 1.0; // no attenuation
			lightDirection = normalize(_WorldSpaceLightPos0.xyz);
		}
		else // point or spot light
		{
			float3 vertexToLightSource = _WorldSpaceLightPos0.xyz
				- mul(modelMatrix, input.vertex).xyz;
			float distance = length(vertexToLightSource);
			attenuation = 1.0 / distance; // linear attenuation 
			lightDirection = normalize(vertexToLightSource);
		}*/

		float3 vertexToLightSource =
			_WorldSpaceLightPos0.xyz - mul(modelMatrix,
				input.vertex * _WorldSpaceLightPos0.w).xyz;
		float one_over_distance =
			1.0 / length(vertexToLightSource);
		float attenuation =
			lerp(1.0, one_over_distance, _WorldSpaceLightPos0.w);
		float3 lightDirection =
			vertexToLightSource * one_over_distance;

		float3 diffuseReflection =
			attenuation * _LightColor0.rgb * _Color.rgb
			* max(0.0, dot(normalDirection, lightDirection));

		output.col = float4(diffuseReflection, 1.0);
		output.pos = mul(UNITY_MATRIX_MVP, input.vertex += float4(input.normal, 0)*2);
		return output;
	}

	float4 frag(vertexOutput input) : COLOR
	{
		return fixed4(1,0,0,1);
	}

		ENDCG
	}
}
Fallback "Diffuse"
}