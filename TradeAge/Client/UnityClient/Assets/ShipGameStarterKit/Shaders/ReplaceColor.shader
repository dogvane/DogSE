Shader "Projector/ReplaceColor"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_ShadowTex ("Cookie", 2D) = "gray" { TexGen ObjectLinear }
	}
	
	Subshader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent-1" }
		Pass
		{
			
			ZWrite Off
			Lighting Off
			Fog { Color (0, 0, 0, 0) }
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			Offset -1, -1
			Color [_Color]

			SetTexture [_ShadowTex]
			{
				combine texture * primary
				Matrix [_Projector]
			}
		}
	}
}