// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Lava"
{
    Properties
    {
        _BaseSpeed ("BaseSpeed", float) = 0.2
        _BaseScale ("BaseScale", float) = 9
        _BaseColor ("BaseColor", Color) = (1, 0.32, 0, 1)
        _FloatSpeed ("FloatSpeed", float) = 0.2
        _FloatScale ("FloatScale", float) = 4
        _FloatColor ("FloatColor", Color) = (0.67, 0.25, 0, 1)
        _FloatEdgeColor ("FloatEdgeColor", Color) = (0.7, 0.39, 0.05, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            float2 unity_gradientNoise_dir(float2 p)
            {
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }

            float unity_gradientNoise(float2 p)
            {
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(unity_gradientNoise_dir(ip), fp);
                float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
            }

            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            {
                Out = unity_gradientNoise(UV * Scale) + 0.5;
            }

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

            void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
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

                Out = t;
            }
            
            fixed _FloatSpeed;
            fixed _FloatScale;
            fixed4 _FloatColor;
            fixed4 _FloatEdgeColor;
            fixed _BaseSpeed;
            fixed _BaseScale;
            fixed4 _BaseColor;
            
            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // just pass the texture coordinate
                o.uv = v.uv;
                return o;
            }

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                //floating lava
                float gradientNoiseOut;
                Unity_GradientNoise_float(i.uv + _Time.y * _FloatSpeed, _FloatScale, gradientNoiseOut);
                
                float4 floatColor = step(gradientNoiseOut * 3, 0.9) * _FloatColor;
                if(dot(floatColor.rgb, fixed3(1, 1, 1)) == 0)
                {
                    floatColor.rgb = fixed3(1, 1, 1);
                }

                float4 floatEdgeColor = step(gradientNoiseOut * 2, 0.8) * _FloatEdgeColor;
                if(dot(floatEdgeColor.rgb, fixed3(1, 1, 1)) == 0)
                {
                    floatEdgeColor.rgb = fixed3(1, 1, 1);
                }

                //base lava
                float simpleNoiseOut;
                Unity_SimpleNoise_float(i.uv + _Time.y * _BaseSpeed, _BaseScale, simpleNoiseOut);
                
                float4 baseColor = clamp(simpleNoiseOut, 0.5, 1) * _BaseColor;

                // sample texture and return it
                fixed4 col = floatColor * floatEdgeColor * baseColor;
                //add emission
                col.rgb += baseColor.rgb;// * baseColor.a;
                //add metallic
                //col.rgb = lerp(col.rgb, fixed3(0.8, 0.8, 0.8), floatColor * floatEdgeColor);
                return col;
            }
            ENDCG
        }
    }
}
