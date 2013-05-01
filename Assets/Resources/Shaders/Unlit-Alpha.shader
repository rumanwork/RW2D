// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/TransparentCustom" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" }
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			Lighting Off
           
			SetTexture [_MainTex] { combine texture } 
		}
	}
}
