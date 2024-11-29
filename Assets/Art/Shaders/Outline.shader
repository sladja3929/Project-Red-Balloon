Shader "Unlit/Outline"
{
    Properties
    {
        [HDR]_Color("Color", Color) = (1, 1, 1, 1)
        [HDR]_OutlineColor("OutlineColor", Color) = (0, 0, 0, 1)  
        _OutlineThickness("OutlineThickness", Range(0.0, 1.0)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            fixed4 _Color;
            
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _Color;
                return col; // 내부 색상
            }
            ENDCG
        }
        
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            ZWrite Off
            Cull Front
            
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
