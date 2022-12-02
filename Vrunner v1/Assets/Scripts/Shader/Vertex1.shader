Shader "Unlit/Vertex1"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_CurvatureY("CurvatureY",float) = 0.004
		_CurvatureX("CurvatureX",float) = 0.004
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

#pragma surface surf Lambert vertex:vert addshadow

		uniform sampler2D _MainTex;
		uniform float _CurvatureY;
		uniform float _CurvatureX;

		struct Input
		{
			float2 uv_MainTex;
		};
		
		fixed4 _Color;

		void vert(inout appdata_full v)
		{
			float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
			worldSpace.xyz -= _WorldSpaceCameraPos.xyz;
			worldSpace = float4((worldSpace.z * worldSpace.z) * -_CurvatureX, (worldSpace.z * worldSpace.z) * -_CurvatureY, 0.0f, 0.0f);

			v.vertex += mul(unity_WorldToObject, worldSpace);
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG
	}
	FallBack "Mobile/Diffuse"
}
