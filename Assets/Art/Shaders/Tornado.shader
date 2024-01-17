Shader "Unlit/Tornado"
{
    Properties
    {
        [HDR]_NoiseColor("Noise Color", Color) = (1, 1, 1, 1)  
        _NoiseSpeed("Noise Speed", Vector) = ( 0, -0.8, 0, 0)
        _NoiseScale("Noise Scale", float) = 25
        _Dissolve("Dissolve", range(0, 1)) = 0
        _TwirlAmount("Twirl Amount", float) = 25
        _TwirlCenter("Twirl Center", Vector) = (0.5, 0, 0, 0)
        _TwirlSpeed("Twirl Speed", Vector) = (0.8, 0, 0, 0)
        _NoisePower("Noise Power", float) = 0.4
        _WobbleSpeed("Wobble Speed", float) = 0.4
        _WobbleIntensity("Wobble Intensity", float) = 1
        _WobbleFrequency("Wobble Frequency", float) = 2
        _WobbleAmount("Wobble Amount", float) = 0.1
        _TopEdgeSize("Top Edge Size", Vector) = (-3, 1, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            };
            
            fixed4 _NoiseColor;
            fixed4 _NoiseSpeed;
            fixed _NoiseScale;
            fixed _Dissolve;
            fixed _TwirlAmount;
            fixed4 _TwirlCenter;
            fixed4 _TwirlSpeed;
            fixed _NoisePower;
            fixed _WobbleSpeed;
            fixed _WobbleIntensity;
            fixed _WobbleFrequency;
            fixed _WobbleAmount;
            fixed4 _TopEdgeSize;
            
            vertexOUT vert (vertexIN v)
            {
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);
                float sinP = (worldPos.y * _WobbleIntensity) + (_Time.y * _WobbleSpeed);
                float wobblePosX = v.vertex.x + (sinP) * _WobbleFrequency;
                v.vertex.x = lerp(v.vertex.x, wobblePosX, _WobbleAmount);
                
                vertexOUT o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (vertexOUT i) : SV_Target
            {
                fixed4 col;
                return col;
            }
            ENDCG
        }
    }
}
