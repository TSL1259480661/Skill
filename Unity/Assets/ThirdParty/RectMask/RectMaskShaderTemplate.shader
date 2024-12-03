Shader "UI/RectMaskShaderTemplate" {
    Properties {
        _MainTex ("Texture", 2D) = "white" { }

        [Enum(UnityEngine.Rendering.StencilCompareFunction)] _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]  _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        [Enum(UnityEngine.Rendering.ColorWriteMask)]  _ColorMask ("Color Mask", Float) = 15

        // Rect Mask Support
        _MaskRect ("_MaskRect", Vector) = (0, 0, 0, 0)
    }

    SubShader {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        
        
        ////特效在UI中的裁剪
        //Stencil {
        //    Ref[_Stencil]
        //    Comp [_StencilComp]
        //    Pass [_StencilOp]
        //    ReadMask[_StencilReadMask]
        //    WriteMask[_StencilWriteMask]
        //}
        
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha



        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            // Rect Mask Support
            #include "RectMaskHlsl.hlsl"

            sampler2D _MainTex;
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            CBUFFER_END

            struct appdate {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                SOFTMASK_COORDS(1) // Rect Mask Support

            };

            v2f vert(appdate v) {
                v2f o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                SOFTMASK_CALCULATE_COORDS(o, v.positionOS.xyz) // Rect Mask Support
                return o;
            }

            float4 frag(v2f i) : SV_Target {
                float4 color = tex2D(_MainTex, i.uv);
                color.a *= SOFTMASK_GET_MASK(i); // Rect Mask Support
                return color;
            }
            ENDHLSL
        }
    }
}