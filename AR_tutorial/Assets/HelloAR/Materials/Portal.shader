﻿// (c) 2018 Guidev
// This code is licensed under MIT license (see LICENSE.txt for details)

Shader "Unlit/Portal"
{
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				float4 normal: NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal: TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = normalize(mul(v.normal, unity_WorldToObject));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 normal = normalize(i.normal);
				float3 lightDir = normalize(float3(0.0, 10.0, 10.0));
				float l = max(dot(lightDir, normal), 0.0);
				return fixed4(l, l, l, 1.0f) + fixed4(0.3, 0.3, 0.3, 1.0);
			}
			ENDCG
		}
	}
}
