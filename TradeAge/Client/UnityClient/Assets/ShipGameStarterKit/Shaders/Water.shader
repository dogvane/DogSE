Shader "Game/Water"
{ 
	Properties
	{
		_WaveScale ("Wave scale", Range (0.02,0.15)) = 0.063
		_ReflDistort ("Reflection distort", Range (0,1.5)) = 0.44
		_RefrDistort ("Refraction distort", Range (0,1.5)) = 0.40
		_FresnelBias ("Fresnel Bias", Range (0.0, 1.0)) = 0.0
		_FresnelScale ("Fresnel Scale", Range (1.0, 10.0)) = 3.0
		_FresnelPower ("Fresnel Power", Range (1.0, 5.0)) = 1.5
		_WaterColor ("Water color (RGB) and Alpha (A)", 2D) = "" {}
		_BumpMap ("Normalmap ", 2D) = "bump" {}
		_WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
		_ReflectionTex ("Internal Reflection", 2D) = "" {}
		_RefractionTex ("Internal Refraction", 2D) = "" {}
	}
	
	Subshader
	{ 
		Tags { "WaterMode"="Refractive" "RenderType"="Opaque" }
		Pass
		{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#pragma multi_compile WATER_REFRACTIVE WATER_REFLECTIVE WATER_SIMPLE
	
#if defined (WATER_REFLECTIVE) || defined (WATER_REFRACTIVE)
#define HAS_REFLECTION 1
#endif
#if defined (WATER_REFRACTIVE)
#define HAS_REFRACTION 1
#endif
	
	
#include "UnityCG.cginc"
	
	uniform float4 _WaveScale4;
	uniform float4 _WaveOffset;
	uniform float4 _OffsetScale;
	
#if HAS_REFLECTION
	uniform float _ReflDistort;
#endif

#if HAS_REFRACTION
	uniform float _RefrDistort;
	uniform float _FresnelBias;
	uniform float _FresnelScale;
	uniform float _FresnelPower;
#endif
	
	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};
	
	struct v2f {
		float4 pos 				: SV_POSITION;
		float2 uv_WaterColor 	: TEXCOORD0;
		float2 bumpuv0			: TEXCOORD1;
		float2 bumpuv1			: TEXCOORD2;
		float3 viewDir			: TEXCOORD3;
#if defined(HAS_REFLECTION) || defined(HAS_REFRACTION)
		float4 ref 				: TEXCOORD4;
#endif
	
	};
	
	v2f vert(appdata v)
	{
		v2f o;
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		
		// OpenGL
		//o.uv = (v.vertex.xz + _OffsetScale.xy) * _OffsetScale.zw;
		
		// DirectX
		o.uv_WaterColor = (v.vertex.xz + _OffsetScale.xy) * _OffsetScale.zw + 0.5 / 1024.0;
		
		// scroll bump waves
		float4 temp;
		temp.xyzw = v.vertex.xzxz * _WaveScale4 / unity_Scale.w + _WaveOffset;
		o.bumpuv0 = temp.xy;
		o.bumpuv1 = temp.wz;
		
		// object space view direction (will normalize per pixel)
		o.viewDir.xzy = ObjSpaceViewDir(v.vertex);
		
#if defined(HAS_REFLECTION) || defined(HAS_REFRACTION)
		o.ref = ComputeScreenPos(o.pos);
#endif
		
		return o;
	}
	
#if defined (WATER_REFLECTIVE) || defined (WATER_REFRACTIVE)
	sampler2D _ReflectionTex;
#endif
	
#if defined (WATER_REFRACTIVE)
	sampler2D _RefractionTex;
#endif
	
	sampler2D _BumpMap;
	sampler2D _WaterColor;
	
	half4 frag( v2f i ) : COLOR
	{
		i.viewDir = normalize(i.viewDir);
		
		// combine two scrolling bumpmaps into one
		half3 bump1 = UnpackNormal(tex2D( _BumpMap, i.bumpuv0 )).rgb;
		half3 bump2 = UnpackNormal(tex2D( _BumpMap, i.bumpuv1 )).rgb;
		half3 bump = (bump1 + bump2) * 0.5;
	
		// Water color	
		half4 color = tex2D( _WaterColor, i.uv_WaterColor );
		
		// NOTE: Unity has a bug where the texture coordinates won't work the first
		// time this shader is attached. To fix this, just rescale the object.
		//color = half4(i.uv_WaterColor.x, i.uv_WaterColor.y, 0.0, 1.0);
		
		// perturb reflection/refraction UVs by bumpmap, and lookup colors
		
#if HAS_REFLECTION
		float4 uv1 = i.ref;
		uv1.xy += bump * _ReflDistort;
		half4 refl = tex2Dproj( _ReflectionTex, UNITY_PROJ_COORD(uv1) );
#endif
	
#if HAS_REFRACTION
		float4 uv2 = i.ref;
		uv2.xy -= bump * _RefrDistort;
		half4 refr = tex2Dproj( _RefractionTex, UNITY_PROJ_COORD(uv2) );
		refr = lerp(refr, color, color.a);
#endif
		
#if defined(WATER_REFRACTIVE)
		float eyeDot = dot(-i.viewDir, bump);
		float fresnel = min( _FresnelBias + _FresnelScale * pow(1.0 + eyeDot, _FresnelPower), 1.0 );
		color = lerp( refr, refl, fresnel * color.a );
#endif
	
#if defined(WATER_REFLECTIVE)
		color = refl * color;
#endif
		return color;
	}
ENDCG
		}
	}
	FallBack "Transparent/Diffuse"
}
