Shader "Hidden/DrawOnTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrushTexture ("Brush Texture", 2D) = "white" {}
        _BrushSize("Brush Size", float) = 0.1
        _BrushColor("Brush Color", Color) = (0,0,0,1)
        _BrushPosition("Brush Position", Vector) = (0,0,0,0)
    }

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _BrushTexture;

            float4 _BrushColor;
            float _BrushSize;
            float4 _BrushPosition;
            
            uniform fixed4 _MainTex_TexelSize;

            half when_gt(half x, half y) {
                return max(sign(x - y), 0.0);
            }

            half when_lt(half x, half y) {
                return max(sign(y - x), 0.0);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half2 brushHalf = half2(_BrushSize, _BrushSize) * 0.5;
                half2 brushMin = _BrushPosition - brushHalf;
                half2 brushMax = _BrushPosition + brushHalf;
                
                // float2 texSize = _MainTex_TexelSize.zw;
                // float2 clippedUv = floor(i.uv * texSize)  / texSize;
                // clippedUv += (0.5/texSize);
                float2 clippedUv = i.uv;
                float canDraw = when_gt(clippedUv.x, brushMin.x) * when_lt(clippedUv.x, brushMax.x) * when_gt(clippedUv.y, brushMin.y) * when_lt(clippedUv.y, brushMax.y);
                float2 brushUVs = (clippedUv - brushMin) / (brushMax - brushMin);
                fixed4 albedo = tex2D(_MainTex, clippedUv);
                fixed4 color = lerp(albedo, _BrushColor, saturate((canDraw + tex2D(_BrushTexture, brushUVs).r) - 1));

                return color;
            }
            ENDCG
        }
    }
}