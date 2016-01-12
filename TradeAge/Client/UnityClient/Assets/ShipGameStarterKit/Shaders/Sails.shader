Shader "Ship/Sails"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_Tint ("Secondary Color", Color) = (1,1,1,1)
		_MainTex ("Diffuse (RGBA)", 2D) = "white" {}
		_BumpMap ("Normal Map (RGB)", 2D) = "white" {}
		_SymbolMap ("Decal Texture (RGBA)", 2D) = "black" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_Intensity ("Color Intensity", Range(0,2)) = 1.0
	}

	SubShader
	{
		LOD 300
		Tags { "RenderType" = "TransparentCutout" }

CGPROGRAM
		#pragma surface surf Sails alphatest:_Cutoff
	
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _SymbolMap;

		float4 _Color;
		float4 _Tint;
		float _Intensity;
		
		struct Input
		{
			float2 uv_MainTex;
			float2 uv2_SymbolMap;
		};
		
		half4 LightingSails (SurfaceOutput s, half3 lightDir, half atten)
		{
			half NdotL = dot(s.Normal, lightDir);
			half frontSide = max(0.0, NdotL);
			half backSide = max(0.0, -NdotL);

			backSide *= backSide;
			NdotL = backSide * 0.65f + frontSide;

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2.0);
			c.a = s.Alpha;
			return c;
		}
	
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			half4 s = tex2D(_SymbolMap, IN.uv2_SymbolMap) * _Tint;
	
			o.Albedo = lerp(c.rgb, s.rgb, s.a) * _Intensity;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
		}
ENDCG
	}
	FallBack "Transparent/Cutout/Diffuse"
}
