Shader "Custom/OcclusionOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 0.05
    }
    SubShader
    {
        Tags { "Queue"="Overlay+1" "RenderType"="Opaque" }
        
        Pass
        {
            Name "Cliped"
            
            ZTest LEqual        
            ZWrite On
            ColorMask 0
            
            Stencil
            {
                Ref 3
                Comp Always
                Pass Replace
                ZFail Replace
            }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.vertex);
                return OUT;
            }

            // 프래그먼트 셰이더: ColorMask 0으로 인해 반환값은 출력되지 않습니다.
            half4 frag (Varyings IN) : SV_Target
            {
                return half4(1.0, 1.0, 0.0, 1.0);
            }
            ENDHLSL
        }
        

        Pass
        {
            Name "Outline"
            
            Tags { "LightMode"="UniversalForward" }            
            //Cull Front
            ZTest Greater        
            ZWrite Off
            
            Stencil
            {
                Ref 3
                Comp NotEqual
                Pass Keep
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
            };

            float _OutlineThickness;
            float4 _OutlineColor;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                // 노말을 월드 공간으로 변환 (UnityObjectToWorldNormal 대신 행렬 사용)
                float3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, IN.normal));
                // 오브젝트의 월드 좌표 (TransformObjectToWorld는 float3를 반환)
                float3 worldPos = TransformObjectToWorld(IN.vertex);
                // 윤곽선 효과를 위해 노말 방향으로 오프셋 적용
                worldPos += worldNormal * _OutlineThickness;
                // 클립 공간으로 변환
                OUT.position = TransformWorldToHClip(worldPos);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}
