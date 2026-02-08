Shader "Unlit/RimLit" {
    Properties {
		[Header(Texture Properties)]
		[NoScaleOffset] [MainTexture] _MainTex ("Main Texture", 2D) = "white" {}
        [SPACE]
        
		[Header(RimLight Properties)]
		[RimColor] _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        [PowerSlider(3.0)] _RimIntensity ("Rim Intensity", Range (0, 1)) = 0
        [PowerSlider(3.0)] _RimPower ("Rim Power", Range (0, 5)) = 1

        [SPACE]
        [Toggle] _RimToggle ("Rim Toggle", Float) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {

            HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			CBUFFER_START(UnityPerMaterial)
                float4 _RimColor;
                float _RimIntensity;
                float _RimPower;
                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _RimToggle;
			CBUFFER_END

            
			struct appdata {
				float2 texUV : TEXCOORD0;
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
			};

			struct v2f {
				float2 texUV : TEXCOORD0;
				float4 positionCS : SV_POSITION;
				float3 positionWS : TEXCOORD2;
				half3 normalWS : TEXCOORD3;
                float3 viewDirWS : TEXCOORD4;

			};

            
			v2f vert (appdata v) { 
                v2f o;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normalOS);

				o.texUV = TRANSFORM_TEX(v.texUV, _MainTex);
				o.positionCS = vertexInput.positionCS;
				o.positionWS = vertexInput.positionWS.xyz;
				o.normalWS = vertexNormalInput.normalWS;
				o.viewDirWS = GetWorldSpaceNormalizeViewDir(o.positionWS).xyz;
                return o;
            }

            float4 rimLight(float4 color, float3 normal, float3 viewDirection) {
                float NdotV = 1 - dot(normal, viewDirection);
                NdotV = pow(NdotV, _RimPower);
                NdotV *= _RimIntensity * _RimToggle;
                float4 finalColor = lerp(color, _RimColor, NdotV);

                return finalColor;
            }
            
			half4 frag( v2f i ) : SV_Target {
                half4 col = tex2D(_MainTex, i.texUV);
    
                col = rimLight(col, i.normalWS, normalize(i.viewDirWS));

                return col;
            }
            ENDHLSL
        }
    }
}
