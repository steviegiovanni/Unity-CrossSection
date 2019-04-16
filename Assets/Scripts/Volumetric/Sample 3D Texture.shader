// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "DX11/Sample 3D Texture" {
	Properties{
		_Volume("Texture", 3D) = "" {}
	}
		SubShader{
		Tags { "Queue"="Transparent" "RenderType" = "Transparent" }

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma exclude_renderers flash gles

		#include "UnityCG.cginc"

		struct vs_input {
			float4 vertex : POSITION;
		};

		struct ps_input {
			float4 pos : SV_POSITION;
			float3 uv : TEXCOORD0;
		};


		ps_input vert(vs_input v)
		{
			ps_input o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.vertex.xyz*0.5 + 0.5;
			return o;
		}

		sampler3D _Volume;

		/*struct ps_color {
			float4 col: COLOR;
		};

		ps_color maincol(ps_input i) {
			ps_color c;
			c.col = tex3D(_Volume, i.uv);
			return c;
		}

		float4 frag(ps_color j) : COLOR
		{
			return float4(j.col.rgb,1.0);
		}*/

		float4 frag(ps_input i) : COLOR
		{
			return tex3D(_Volume, i.uv);
			//return (1.0,1.0,1.0,0.5);
		}

		ENDCG

		}
	}

		Fallback "VertexLit"
}