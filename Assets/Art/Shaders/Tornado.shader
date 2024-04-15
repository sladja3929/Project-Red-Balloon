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
        cull off
        
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

            inline float unity_noise_randomValue (float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
            }

            inline float unity_noise_interpolate (float a, float b, float t)
            {
                return (1.0-t)*a + (t*b);
            }

            inline float unity_valueNoise (float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                uv = abs(frac(uv) - 0.5);
                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = unity_noise_randomValue(c0);
                float r1 = unity_noise_randomValue(c1);
                float r2 = unity_noise_randomValue(c2);
                float r3 = unity_noise_randomValue(c3);

                float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
                float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
                float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            float SimpleNoise(float2 UV, float Scale)
            {
                float t = 0.0;

                float freq = pow(2.0, float(0));
                float amp = pow(0.5, float(3-0));
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                freq = pow(2.0, float(1));
                amp = pow(0.5, float(3-1));
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                freq = pow(2.0, float(2));
                amp = pow(0.5, float(3-2));
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                return t;
            }

            float2 Twirl(float2 UV, float2 Center, float Strength, float2 Offset)
            {
                float2 delta = UV - Center;
                float angle = Strength * length(delta);
                float x = cos(angle) * delta.x - sin(angle) * delta.y;
                float y = sin(angle) * delta.x + cos(angle) * delta.y;
                return float2(x + Center.x + Offset.x, y + Center.y + Offset.y);
            }

            float2 RadialSheer(float2 UV, float2 Center, float Strength, float2 Offset)
            {
                float2 delta = UV - Center;
                float delta2 = dot(delta.xy, delta.xy);
                float2 delta_offset = delta2 * Strength;
                return UV + float2(delta.y, -delta.x) * delta_offset + Offset;
            }

            float2 Remap(float input, float2 inMinMax, float2 outMinMax)
            {
                return outMinMax.x + (input - inMinMax.x) * (outMinMax.y - outMinMax.x) / (inMinMax.y - inMinMax.x);    
            }
            
            vertexOUT vert (vertexIN v)
            {
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);
                float sinP = (worldPos.y * _WobbleIntensity) + (_Time.y * _WobbleSpeed);
                float wobblePosX = v.vertex.x + sin(sinP) * _WobbleFrequency;
                v.vertex.x = lerp(v.vertex.x, wobblePosX, _WobbleAmount);
                
                vertexOUT o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (vertexOUT i) : SV_Target
            {
                //상하 왜곡
                const float2 center = float2(0.5, 0.5);
                const float2 strength = float2(5, 5);
                float2 radialSheer = RadialSheer(i.uv, center, strength, _Time.y * _NoiseSpeed.xy);
                float distortY = SimpleNoise(radialSheer, _NoiseScale);
                distortY = Remap(distortY, float2(-0.5, 1), float2(0, 1));
                
                //좌우 변형
                float2 twirl = Twirl(i.uv, _TwirlCenter.xy, _TwirlAmount, _Time.y * _TwirlSpeed.xy);
                float twistX = SimpleNoise(twirl, 20);
                twistX = Remap(twistX, float2(-0.5, 1), float2(0, 1));

                //상단 클리핑
                float alphaClipping = Remap(i.uv.y, float2(0, 1), _TopEdgeSize.xy);
                alphaClipping = clamp(alphaClipping, 0, 1);
                alphaClipping = SimpleNoise(i.uv, alphaClipping * 100) * alphaClipping;
                
                fixed4 col;
                float baseTexture = pow(distortY * twistX, _NoisePower);
                col.rgb = baseTexture * _NoiseColor.rgb;
                col.a = Remap(_Dissolve, float2(0, 1), float2(1, 0));
                clip(col.a - float(baseTexture + alphaClipping));
                return col;
            }
            ENDCG
        }
    }
}
