Shader "lit/Water"
{
    Properties
    {
        _DepthFade ("Depth Fade", float) = 0        
        _WindDirection("Wind Direction", Vector) = (0, 0, 0, 0)
        _ShallowColor("Shallow Color", color) = (0, 0, 0, 1)
        _DeepColor("Deep Color", color) = (0, 0.674, 0.835, 1)
        _SmoothPercent("Smooth Percent", range(0, 1)) = 0
        _WaterHeight("Water Height", range(0, 3)) = 0
        _WaterScrollSpeed ("Water Scroll Speed", range(0, 4)) = 0.2
        _NoiseScale("Noise Scale", range(0, 10)) = 0.2
        _FoamColor("Foam Color", color) = (0, 0, 0, 1)
        _FoamWidth("Foam Width", float) = 0        
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vertexIN
            {
                float4 vertex : POSITION;
            };

            struct vertexOUT
            {
                float4 vertex : SV_POSITION;
                float4 depth : TEXCOORD0;
            };

            fixed _DepthFade;
            fixed4 _WindDirection;
            fixed4 _ShallowColor;
            fixed4 _DeepColor;
            fixed _SmoothPercent;
            fixed _WaterHeight;
            fixed _WaterScrollSpeed;
            fixed _NoiseScale;
            fixed4 _FoamColor;
            fixed _FoamWidth;
            
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
            
            float OceanNoise(float octaveFactor, vertexIN v)
            {
                float gradientNoiseOut;
                float2 UV = v.vertex.xz;
                float2 tiling = float2(1, 1);
                float2 offset = _Time.y * normalize(_WindDirection.xy) * _WaterScrollSpeed * octaveFactor;
                Unity_GradientNoise_float(UV * tiling + offset, _NoiseScale / octaveFactor, gradientNoiseOut);
                return gradientNoiseOut * octaveFactor;
            }
          
            vertexOUT vert (vertexIN v)
            {
                float octaveNoise = 0;
                float i = 1;
                while(i <= 32)
                {
                    octaveNoise += OceanNoise(i, v);
                    i = i * 2;
                }

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);                
                v.vertex.xyz = octaveNoise * _WaterHeight + worldPos;
                
                vertexOUT o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (vertexOUT i) : SV_Target
            {
                float sceneDepth = LinearEyeDepth(UNITY_SAMPLE_DEPTH (i.depth));
                float4 screenPos = i.depth / i.depth.w;
                float4 baseColor = lerp(_ShallowColor, _DeepColor, saturate(sceneDepth - screenPos / _DepthFade));
                fixed4 col = baseColor;
                return col;
            }
            ENDCG
        }
    }
}
