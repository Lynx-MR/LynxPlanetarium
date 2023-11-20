Shader "Lynx/Sun_VR"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InnerValue ("Inner value", Range(0,1)) = 0.5
        _OuterValue ("Outer value", Range(0,1)) = 0.8
        _InnerCol ("Inner Color", Color) = (1,1,1,1)
        _OuterCol ("Outer color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector" = "True" "RenderType"="Transparent"}
        Blend SrcAlpha One
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _InnerValue;
            float _OuterValue;
            float4 _InnerCol;
            float4 _OuterCol;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float dist = length(i.uv-0.5)*2.0;
                float grad = clamp(smoothstep(_InnerValue,_OuterValue,dist),0,1);
                float3 gradCol = lerp(_InnerCol, _OuterCol, grad);
                fixed4 col = float4(gradCol,1-grad);
                return col;
            }
            ENDCG
        }
    }
}
