Shader "Projector/MultiplyNoAlpha"
{
	Properties
	{
		_ShadowTex ("Cookie", 2D) = "gray" { TexGen ObjectLinear }
	}

	Subshader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent-1" }
		Pass
		{
			ZWrite Off
			Fog { Color (1, 1, 1) }
			AlphaTest Off
			ColorMask RGB
			Blend DstColor Zero
			Offset -1, -1
			SetTexture [_ShadowTex]
			{
				combine texture, ONE - texture
				Matrix [_Projector]
			}
		}
	}
}