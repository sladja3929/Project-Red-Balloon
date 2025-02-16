Shader "Unlit/Gradient_Outline_Occlusion"
{
    Properties
    {
        [HDR]_Color("Color", Color) = (1, 1, 1, 1)
        [HDR]_OutlineColor("OutlineColor", Color) = (0, 0, 0, 1)
        [HDR]_ChargeColor("ChargeColor", Color) = (1, 1, 1, 1)
        _OutlineThickness("OutlineThickness", Range(0.0, 1.0)) = 0.02
        _ChargeRate("ChargeRate", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Overlay+3" "RenderType"="Opaque" }
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            ZTest LEqual
            ZWrite On             
            
            Stencil
            {
                Ref 3
                Comp Equal
                Pass Keep
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            fixed4 _ChargeColor;
            fixed _ChargeRate;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // UV.y 값을 사용하여 그라데이션 비율 계산
                float t = saturate(i.uv.y);

                // 선택한 색상과 반전된 색상 계산
                fixed4 invertedColor = fixed4(0,0,0,1);//fixed4(1.0 - _Color.rgb, _Color.a);
                fixed4 col = lerp(_Color, invertedColor, t);

                if(i.uv.y <= _ChargeRate)
                {
                    col = _ChargeColor;
                }
                
                return col;
            }
            ENDCG
        }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            
            ZTest LEqual
            ZWrite On  
            Cull Front
            
            Stencil
            {
                Ref 3
                Comp Equal
                Pass Keep
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };
            
            fixed4 _OutlineColor;
            fixed _OutlineThickness;
            

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);
                
                // 외곽선 두께 적용
                v.vertex.xyz += norm * _OutlineThickness; 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = _OutlineColor; // 외곽선 색상
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}
