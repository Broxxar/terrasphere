Shader "ASmallWorld/Clouds"
{
	Properties
	{
		_LightingRamp("Lighting Ramp", 2D) = "black" {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent+1"
			"RenderType"="Transparent"
		}
		
			CGINCLUDE		
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 objectPos : TEXCOORD0;
				float3 lightDir : TEXCOORD2;
			};

			samplerCUBE _CubePaintingTest;
			float2 _CubePaintingTest_TexelSize;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.objectPos = v.vertex;

				float3 objectCamera = mul(unity_WorldToObject, _WorldSpaceCameraPos.xyz);
				o.lightDir = normalize(UnityWorldToObjectDir(_WorldSpaceLightPos0));

				return o;
			}

			sampler2D _LightingRamp;

			float3 normalsExperiment(samplerCUBE cube, float3 dir, float t)
			{
				float s = texCUBE(cube, dir).r;

				float h0 = texCUBE(cube, dir + float3(t, 0, 0)).r;
				float h1 = texCUBE(cube, dir + float3(-t, 0, 0)).r;
				float h2 = texCUBE(cube, dir + float3(0, t, 0)).r;
				float h3 = texCUBE(cube, dir + float3(0, -t, 0)).r;
				float h4 = texCUBE(cube, dir + float3(0, 0, t)).r;
				float h5 = texCUBE(cube, dir + float3(0, 0, -t)).r;

				float3 n;

				n.x = h1 - h0;
				n.y = h3 - h2;
				n.z = h5 - h4;

				// GOOD ENOUGH
				return normalize(n + dir);
			}

			float blurCubeSample(samplerCUBE cube, float3 dir, float t)
			{
				float s = 0;

				s += texCUBE(cube, dir + float3(t, 0, 0)).b;
				s += texCUBE(cube, dir + float3(-t, 0, 0)).b;
				s += texCUBE(cube, dir + float3(0, t, 0)).b;
				s += texCUBE(cube, dir + float3(0, -t, 0)).b;
				s += texCUBE(cube, dir + float3(0, 0, t)).b;
				s += texCUBE(cube, dir + float3(0, 0, -t)).b;

				return s / 6.0;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float3 samplePos = normalize(i.objectPos);
				float3 normal = samplePos;
				float cloud = blurCubeSample(_CubePaintingTest, samplePos, _CubePaintingTest_TexelSize * 2);

				float ndotl = saturate(dot(normal, i.lightDir));

				fixed4 lighting = tex2D(_LightingRamp, float2(ndotl, 0));

				return fixed4(lighting.rgb, cloud);
			}
			ENDCG
			
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			Blend SrcAlpha OneMinusSrcAlpha
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
