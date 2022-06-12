Shader "Hidden/Roystan/Normals Texture"
{
	Properties
	{
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2_f
			{
				float4 vertex : SV_POSITION;
				float3 viewNormal : NORMAL;
			};

			v2_f vert(appdata v)
			{
				v2_f o;
				UNITY_SETUP_INSTANCE_ID(v);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.viewNormal = COMPUTE_VIEW_NORMAL;
				return o;
			}

			float4 frag(v2_f i) : SV_Target
			{
				return float4(i.viewNormal, 0);
			}
			ENDCG
		}
		
	}
	Fallback "Diffuse"
}