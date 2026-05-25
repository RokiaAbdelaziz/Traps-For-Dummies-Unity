Shader "Custom/WobblyLavaSphere"
{
    Properties
    {
       [HDR] _BaseColor("Base Color", Color) = (1,1,1,1)
        _MainTex("Lava Texture", 2D) = "white" {}
        _WobbleSpeed("Wobble Speed", Float) = 2.0
        _WobbleStrength("Wobble Strength", Float) = 0.5
    }
    
    SubShader 
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        Pass 
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            struct Attributes 
            {
                float4 positionOS : POSITION; 
                float2 uv : TEXCOORD0;
            };

            struct Varyings 
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Texture Declarations
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float _WobbleSpeed;
                float _WobbleStrength;
            CBUFFER_END

            // 3. THE VERTEX SHADER 
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float4 pos = IN.positionOS; 
                // WOBBLE logic
                pos.x += sin(pos.y * 5.0 + _Time.y * _WobbleSpeed) * _WobbleStrength;
                pos.z += cos(pos.y * 5.0 + _Time.y * _WobbleSpeed) * _WobbleStrength;
                OUT.positionHCS = TransformObjectToHClip(pos.xyz);
                OUT.uv = IN.uv;
                
                return OUT;
            }

            // 4. THE FRAGMENT SHADER
            half4 frag(Varyings IN) : SV_Target
            {
                half4 lavaColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                
                // Return texture multiplied by color tint
                return lavaColor * _BaseColor;
            }
            
            ENDHLSL
        }
    }
}