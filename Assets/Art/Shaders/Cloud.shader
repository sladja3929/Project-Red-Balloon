Shader "Unlit/Cloud"
{
    Properties
    {
        [HDR]_BaseColor("Base Color", Color) = (1, 1, 1, 1)  
        _MaxAlpha("Max Alpha", float) = 1
        _MinAlpha("Min Alpha", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
        LOD 100
        cull off
        Zwrite off
        blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma nolight keepalpha noforwardadd nolightmap noambient novertexlights noshadow
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct vertexIN
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOUT
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 v : TEXCOORD1;
            };

            fixed4 _BaseColor;
            fixed _MaxAlpha;
            fixed _MinAlpha;
            
            vertexOUT vert (vertexIN v)
            {                
                vertexOUT o;
                o.v = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 Remap(float input, float2 inMinMax, float2 outMinMax)
            {
                return outMinMax.x + (input - inMinMax.x) * (outMinMax.y - outMinMax.x) / (inMinMax.y - inMinMax.x);    
            }
            
            fixed4 frag (vertexOUT i) : SV_Target
            {               
                fixed4 col;
                col.r = 0.5;
                col.g = 0.5;
                col.b = 0.5;
                //col.rgb = i.v;
                col.a = 1;
                //col.rgb = _BaseColor.rgb;
                col.a = lerp(_MaxAlpha, _MinAlpha, Remap(abs(0.5 - i.uv.y), float2(0, 0.5), float2(0, 1)));
                return col;
            }
            ENDCG
        }
    }
}
