// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Building"
{
	Properties
	{
		_Color("Color", COLOR) = (1,1,1,1)
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2_f
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			float4 _Color;

			v2_f vert(appdata v)
			{
				v2_f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				const float3 world_normal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);
				const float brightness = max(0, dot(world_normal, _WorldSpaceLightPos0.xyz));
				const float4 lit = lerp(UNITY_LIGHTMODEL_AMBIENT * _Color, _Color, brightness);
				o.color = lerp(unity_FogColor, lit, v.uv.y);
				return o;
			}

			fixed4 frag(const v2_f i) : SV_Target
			{
				// sample the texture
				return i.color;
			}
			ENDCG
		}
	}
}