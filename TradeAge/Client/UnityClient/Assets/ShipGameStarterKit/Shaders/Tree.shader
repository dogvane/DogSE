Shader "Game/Tree"
{
 Properties
	{
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_Cutoff ("Base Alpha cutoff", Range (0,.9)) = .5
	}

 	SubShader
	{
		Tags
		{
			"Queue"="AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}

		Cull Off
		ZWrite On
		LOD 300

		Pass
		{
			// We actually get softer edges if we do write to RGB
			//ColorMask A

			AlphaTest GEqual [_Cutoff]
			Blend SrcAlpha OneMinusSrcAlpha

			SetTexture [_MainTex]
			{
				constantColor [_Color]
				Combine texture * constant, texture * constant 
			}
		}
		
		CGPROGRAM
		#pragma surface surf Lambert alpha
		sampler2D _MainTex;
		float4 _Color;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb * _Color.rgb;
			o.Alpha = tex.a * _Color.a;
		}
		ENDCG
	}
	Fallback "Transparent/Cutout/VertexLit"
}
