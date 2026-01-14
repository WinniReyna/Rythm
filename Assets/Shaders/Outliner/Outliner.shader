Shader "Unlit/Outliner2D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineSize ("Outline Size (px)", Float) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // x = 1/width, y = 1/height
            float4 _Color;
            float4 _OutlineColor;
            float _OutlineSize;

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

            float4 frag (v2f i) : SV_Target
            {
                float4 mainCol = tex2D(_MainTex, i.uv) * _Color;

                // Si el pixel es visible, dibujamos normal
                if (mainCol.a > 0.01)
                    return mainCol;

                float outlineAlpha = 0;

                float2 pixel = _MainTex_TexelSize.xy * _OutlineSize;

                float2 offsets[8] = {
                    float2( pixel.x, 0),
                    float2(-pixel.x, 0),
                    float2(0,  pixel.y),
                    float2(0, -pixel.y),
                    float2( pixel.x,  pixel.y),
                    float2(-pixel.x,  pixel.y),
                    float2( pixel.x, -pixel.y),
                    float2(-pixel.x, -pixel.y)
                };

                for (int j = 0; j < 8; j++)
                {
                    outlineAlpha = max(
                        outlineAlpha,
                        tex2D(_MainTex, i.uv + offsets[j]).a
                    );
                }

                if (outlineAlpha > 0.01)
                    return float4(_OutlineColor.rgb, _OutlineColor.a);

                return float4(0,0,0,0);
            }
            ENDCG
        }
    }
}
