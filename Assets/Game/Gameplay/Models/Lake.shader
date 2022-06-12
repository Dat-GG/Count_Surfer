Shader "Custom/Lake"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Main Color", COLOR) = (1,1,1,1)
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}
		LOD 150
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd


		sampler2D _MainTex;
		float4 _Color;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c0 = tex2D(_MainTex, IN.uv_MainTex + float2(_CosTime.y, _SinTime.x));
			fixed4 c1 = tex2D(_MainTex, IN.uv_MainTex + float2(_SinTime.x, _CosTime.y));
			float r = 0.5 * (_SinTime.w + 1.0);
			o.Albedo = (c0.rgb * r + c1.rgb * (1.0 - r)) * _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}

	Fallback "Mobile/VertexLit"
}