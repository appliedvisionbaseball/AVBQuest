Shader "Custom/OutlineLineShader"
{
    Properties
    {
        _Color ("Line Color", Color) = (0, 1, 1, 1)
        _OutlineColor ("Outline Color", Color) = (1, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 worldPos : TEXCOORD0;
            };

            float4 _Color;
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = v.vertex;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float distanceToEdge = 1.0 - smoothstep(0.0, _OutlineWidth, length(i.worldPos.xy));
                return lerp(_Color, _OutlineColor, distanceToEdge);
            }
            ENDCG
        }
    }
}
