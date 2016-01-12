Shader "Game/Untextured"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Lighting On
			
			Material
			{
				Diffuse [_Color]
				Ambient [_Color]
			}
		}
	}
	Fallback "VertexLit"
}
