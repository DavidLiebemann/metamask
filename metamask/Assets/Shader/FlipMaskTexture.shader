Shader "Custom/FlipMaskTexture"
{
    Properties
    {
        [MainColor] _MainColor("Main Color", Color) = (1, 1, 1, 1)
        [MainTexture] _MainTex("Main Texture", 2D) = "white" {}
        [FlipVariant] _FlipVariant("Flip Variant", Range(0, 8)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

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

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                half4 _MainColor;
                float4 _MainTex_ST;
                int _FlipVariant;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {

                float2 newUV = IN.uv;
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;

                switch (int(_FlipVariant)) {

                case 0: {
                        newUV = float2(abs(abs(0.5 - IN.uv.x) - 0.5), IN.uv.y); //mirrored On X
                        color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;
                        return color;
                    }
                case 1: {
                        newUV = float2(IN.uv.x, abs(abs(0.5 - IN.uv.y) - 0.5)); // mirrored on Y
                        color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;
                        return color;
                    }
                case 2: {
                        newUV = float2(abs(0.5 - IN.uv.x), IN.uv.y); // inverse mirrored on X
                        color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;
                        return color;
                    }
                case 3: {
                        newUV = float2(IN.uv.x, abs(0.5 - IN.uv.y)); // inverse mirrored on Y
                        color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;
                        return color;
                    }
                case 4: {
                        newUV = (mul(float2x2(cos(90), -sin(90), sin(90), cos(90)), float2(IN.uv - 0.5))) + 0.5; // rotated
                        color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;
                        return color;
                    }
                case 5: {
                        newUV = (mul(float2x2(cos(-90), -sin(-90), sin(-90), cos(-90)), float2(IN.uv - 0.5))) + 0.5; // rotated inverse
                        color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;
                        return color;
                    }
                case 6: {
                        newUV = float2(1 - IN.uv.x, IN.uv.y); // simple flip on X
                        color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;
                        return color;
                    }
                case 7: {
                        newUV = float2(IN.uv.x, 1 - IN.uv.y); // simple flip on Y
                        color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;
                        return color;
                    }
                case 8: {
                        //newUV = float2(min(IN.uv.x, IN.uv.x - 0.5 + max(0, (2 * IN.uv.x - 1))), IN.uv.y);
                        newUV = float2(min(IN.uv.x, abs(IN.uv.x - 0.5)), IN.uv.y);
                        color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV) * _MainColor;
                        return color;
                    }
                default: {
                        return color;
                    }
                }
            }

            ENDHLSL
        }
    }
}
 