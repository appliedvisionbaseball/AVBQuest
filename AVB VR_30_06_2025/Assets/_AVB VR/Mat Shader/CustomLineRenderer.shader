Shader "Unlit/CustomLineRenderer"
{
    Properties {
        [Header(Surface options)]

        [MainColor] _ColorTint("Tint", Color) = (1, 1, 1, 1)
        _VertexColorMap("Vertex Color Map", 2D) = "white"
    }
    SubShader{

        Tags {"RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline"}
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            HLSLPROGRAM 

            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS   : POSITION;
                uint vertexID : SV_VertexID;
            };

            struct Interpolators {
                float4 color : COLOR;
                float4 positionHCS  : SV_POSITION;
            };

            TEXTURE2D(_VertexColorMap);
            SAMPLER(sampler_VertexColorMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _VertexColorMap_ST;
                float4 _ColorTint;
                float4 _VertexColorMap_TexelSize;
            CBUFFER_END

            Interpolators Vertex(Attributes input) {
                Interpolators output;

                float2 uv = float2((input.vertexID == 0) ? _VertexColorMap_TexelSize.x : input.vertexID * _VertexColorMap_TexelSize.x, 0.5);
                output.color = SAMPLE_TEXTURE2D_ARRAY_LOD(_VertexColorMap, sampler_VertexColorMap, uv.xy, 0.0, 0.0);
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);

                return output;
            }

            float4 Fragment(Interpolators input) : SV_TARGET {
                return input.color * _ColorTint;
            }

            ENDHLSL
        }
    }
}