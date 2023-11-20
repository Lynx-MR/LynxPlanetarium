Shader "Lynx/VideoSphereSahder"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_FadeColor("fadeColor", Color) = (1,1,1,1)
		_NoiseTiling("Noise Tilling", Range(0,10)) = 3
		_HeadPos("Head Position", Vector) = (0,0,0,0)
		_FadeDist("Fade Distance", Range(0,1)) = 0.5
		_NoiseStrengh("Noise Strengh", Range(0,0.15)) = 0.1
        _Cutoff("alpha cutout", Range(0.0,1.0)) = 0.5
	}
		SubShader
	{
		Tags {
            "Queue"      = "AlphaTest"
            "RenderType" = "TransparentCutout"
        }
        LOD 100
        Cull Front
        ZWrite On
        AlphaTest Greater [_Cutoff]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag alphatest:_Cutoff

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 pos : TEXCOORD1;
			};
			
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float _NoiseTiling;
			float4 _HeadPos;
			float4 _MainTex_ST;
			float4 _FadeColor;
			float _NoiseStrengh;
            float _Cutoff;
			float _FadeDist;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.pos = v.vertex;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float tOffset = _Time * 1;
				float3 worldPos = mul(unity_ObjectToWorld, float4(i.pos.xyz,1.0)).xyz;
				float4 noise = tex2D(_NoiseTex, i.uv*_NoiseTiling*float2(2,1) + tOffset);

				float grad = distance(worldPos, _HeadPos.xyz);
				grad -= noise*_NoiseStrengh;
				grad = _FadeDist - grad;

				col.a = 1-clamp(grad,0,1);
				
				float sides = clamp(col.a,0,1);
				sides = step(col.a,0.9999);
				col.xyz = lerp(col.xyz,_FadeColor,sides);
                clip(col.a - _Cutoff);
				return col;
			}
			ENDCG
		}
	}
}
