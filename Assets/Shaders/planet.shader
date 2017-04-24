Shader "ASmallWorld/Planet"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ShoreLine("Shore", 2D) = "black" {}
		_RimPower("Rim Power", float) = 1
		_RimIntensity("Rim Intensity", float) = 1
		_RimColor("Rim Color", Color) = (1, 1, 1, 0)
		_LightingRamp("Lighting Ramp", 2D) = "black" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 objectPos : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float3 lightDir : TEXCOORD2;
			};

			samplerCUBE _CubePaintingTest;
			float2 _CubePaintingTest_TexelSize;

			v2f vert (appdata v)
			{
				v2f o;

				float height = texCUBElod(_CubePaintingTest, float4(v.vertex.xyz, 0)).r;
				float3 extrude = normalize(v.vertex) * height;

				v.vertex.xyz += extrude * 0.10;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.objectPos = v.vertex;

				float3 objectCamera = mul(unity_WorldToObject, _WorldSpaceCameraPos.xyz);
				o.viewDir = normalize(objectCamera - v.vertex.xyx);
				o.lightDir = normalize(UnityWorldToObjectDir(_WorldSpaceLightPos0));

				return o;
			}

			sampler2D _MainTex;
			sampler2D _ShoreLine;
			sampler2D _LightingRamp;

			float _RimPower;
			float _RimIntensity;
			fixed4 _RimColor;

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

			fixed4 frag (v2f i) : SV_Target
			{
				float3 samplePos = normalize(i.objectPos);

				float3 data = texCUBE(_CubePaintingTest, samplePos);
				float height = data.r;
				float clouds = data.b;

				float3 normal = normalsExperiment(_CubePaintingTest, samplePos, _CubePaintingTest_TexelSize.x * 1);

				float ndotl = saturate(dot(normal, i.lightDir));

				fixed4 lighting = min(tex2D(_LightingRamp, float2(ndotl, 0)), (1 - clouds * 0.5));

				float ndotv = 1 - pow(saturate(dot(normalize(i.objectPos), i.viewDir)), _RimPower);
				fixed4 rim = _RimColor * ndotv * _RimIntensity;

				float shore = tex2D(_ShoreLine, float2(height, 0));
				float waveCrest = shore * (sin(_Time.y * 0.5 + i.objectPos.y * 8) - 1) * 0.05;

				return tex2D(_MainTex, float2(height + waveCrest, 0)) * lighting + rim;
			}
			ENDCG
		}
	}
}
