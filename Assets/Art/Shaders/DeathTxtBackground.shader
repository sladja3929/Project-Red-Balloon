Shader "Unlit/DeathTxtBackground"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _GradientColor ("Gradient Color", Color) = (1,1,1,0)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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
            fixed4 _Color;
            fixed4 _GradientColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // 방사형 그라데이션 계산
                float2 center = float2(0.5, 0.5);
                float distance = length(i.uv - center);
                float gradient = smoothstep(0.15, 0.5, distance); // 그라데이션 범위 조정
                col.a *= lerp(0.6,0.0, gradient);

                // 마름모 모양을 만들기 위한 조건
                // float diamond = abs(i.uv.x - 0.5) + abs(i.uv.y - 0.5);
                // if (diamond > 0.5)
                // {
                //     discard;
                // }

                return col;
            }
            ENDCG
        }
    }
}