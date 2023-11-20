Shader "Lynx/Skybox_WithSwitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "White" {}
        _outlineColor("outline Color", Color) = (0,0,0,1)
        _OutlineSize ("outline size", Range(0,0.1)) = 0.1
        _VeiwAxis ("Veiw Axis", Vector) = (1,0,0,0)
        _Switch ("switch AR/VR", Range(0,1)) = 1
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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

            // make fog work
            #pragma multi_compile_fog

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
            float4 _MainTex_ST;
            float _OutlineSize;
            float _Switch;
            float _Cutoff;
            float4 _outlineColor;
            float4 _VeiwAxis;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.pos = v.vertex;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float4 noise = tex2D(_NoiseTex, i.uv*3.0);
                float gradOffset = _Switch*1.2 -0.1;
                float viewGrad = dot(i.pos.xyz, _VeiwAxis.xyz);
                gradOffset = viewGrad + gradOffset;
                float outlineMask = smoothstep(0.45-_OutlineSize,0.55-_OutlineSize,gradOffset);
                gradOffset = smoothstep(0.45,0.55,gradOffset);


                float alpha = smoothstep(gradOffset-0.05, gradOffset+0.05,noise+0.1);
                outlineMask = smoothstep(outlineMask-0.05, outlineMask+0.05,noise+0.1);
                col = lerp(_outlineColor, col, outlineMask);


                clip(alpha - _Cutoff);
                return float4(col.xyz, alpha);
            }
            ENDCG
        }
    }
}
