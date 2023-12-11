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
        _FloatAmount ("FloatAmount", range(0.2, 0.4)) = 0.3
        _Tiling ("Tiling", Vector) = (1, 1, 0, 0)
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
                float3 normal : NORMAL;
                float4 tangent : TANGENT;                
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
                //tangent matrix
                half3 tspace0 : TEXCOORD1;
                half3 tspace1 : TEXCOORD2;
                half3 tspace2 : TEXCOORD3;
                float viewDir : TEXCOORD4;
            };

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
            
            float3 Unity_NormalFromHeight_Tangent_float(float In, float Strength, float3 Position, float3x3 TangentMatrix)
            {
                float3 worldDerivativeX = ddx(Position);
                float3 worldDerivativeY = ddy(Position);

                float3 crossX = cross(TangentMatrix[2].xyz, worldDerivativeX);
                float3 crossY = cross(worldDerivativeY, TangentMatrix[2].xyz);
                float d = dot(worldDerivativeX, crossY);
                float sgn = d < 0.0 ? (-1.0f) : 1.0f;
                float surface = sgn / max(0.000000000000001192093f, abs(d));

                float dHdx = ddx(In);
                float dHdy = ddy(In);
                float3 surfGrad = surface * (dHdx*crossY + dHdy*crossX);
                float3 Out = normalize(TangentMatrix[2].xyz - (Strength * surfGrad));
                Out = mul(TangentMatrix, Out);
                return Out;
            }
            
            fixed _FloatSpeed;
            fixed _FloatScale;
            fixed4 _FloatColor;
            fixed _BaseSpeed;
            fixed _BaseScale;
            fixed4 _BaseColor;
            fixed _FloatAmount;
            fixed4 _Tiling;
            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);

                half3 wNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                half3 wTangent = normalize(mul((float3x3)unity_ObjectToWorld, v.tangent.xyz));
                // compute bitangent from cross product of normal and tangent
                half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                // output the tangent space matrix
                o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);

                o.viewDir = _WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex);
                // just pass the texture coordinate
                o.uv = v.uv;
                return o;
            }

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                float2 tiling = _Tiling.xy;
                //base lava
                float baseNoise;
                Unity_SimpleNoise_float((i.uv * tiling) + _Time.y * _BaseSpeed, _BaseScale, baseNoise);
                baseNoise = clamp(baseNoise, 0.5, 1);
                float4 baseColor = baseNoise * _BaseColor;

                //float lava
                float floatNoise;
                Unity_SimpleNoise_float((i.uv * tiling) + _Time.y * _FloatSpeed, _FloatScale, floatNoise);
                float heightMap = clamp(floatNoise, 0, _FloatAmount);
                
                float4 col;
                if(floatNoise > _FloatAmount) col = baseColor * 3;
                else col = floatNoise * _FloatColor * 2;

                //create normal from height
                // Convert height to normal using partial derivatives
                float3 normal = normalize(float3(ddx(heightMap), ddy(heightMap), 1.0));                
                // Apply strength
                normal *= 0.5;
                // Transform normal to world space
                //normal = normalize(mul((float3x3)unity_ObjectToWorld, normal));

                 // Perform normal mapping
                float3 viewDir = normalize(i.viewDir);
                float NdotV = max(0.0, dot(normal, viewDir));
                // Apply lighting using Lambertian reflection model
                //col = fixed4(col.xyz * NdotV, 1.0);
                // sample texture and return it
                //add emission
                //col.rgb += baseColor.rgb;// * baseColor.a;
                //add metallic
                //col.rgb = lerp(col.rgb, fixed3(0.8, 0.8, 0.8), floatColor * floatEdgeColor);
                fixed4 ss = fixed4(normal, 1);
                return ss;
            }
            ENDCG
        }
    }
}
