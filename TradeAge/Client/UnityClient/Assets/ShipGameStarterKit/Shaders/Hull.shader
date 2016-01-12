Shader "Ship/Hull"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Diffuse (RGBA)", 2D) = "white" {}
		_SecondTex ("Normal Map (RGB) and AO (A)", 2D) = "white" {}
		_Occlusion ("Ambient Occlusion", Range(0,1)) = 1.0
		_Intensity ("Color Intensity", Range(0,2)) = 1.0
	}

	SubShader
	{
		LOD 300
		Tags { "RenderType" = "Opaque" }

CGPROGRAM
		#pragma surface surf Improved
	
		sampler2D _MainTex;
		sampler2D _SecondTex;
		float4 _Color;
		float _Occlusion;
		float _Intensity;
		
		struct Input
		{
		  float2 uv_MainTex;
		  float2 uv2_SecondTex;
		};
	
		half4 LightingImproved (SurfaceOutput s, half3 lightDir, half atten)
		{
			half NdotL = dot(s.Normal, lightDir);
			NdotL = max(0.0, NdotL);

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2.0);
			c.a = s.Alpha;
			return c;
		}
	
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			half3 n = tex2D(_SecondTex, IN.uv_MainTex).rgb * 2.0 - 1.0;
			float lm = tex2D(_SecondTex, IN.uv2_SecondTex).a;
	
			c.rgb *= _Intensity;
			o.Albedo = lerp(c.rgb, c.rgb * lm, _Occlusion);
			o.Alpha = c.a;
			o.Normal = normalize(n);
		}
ENDCG
	}
	FallBack "Diffuse"
}
