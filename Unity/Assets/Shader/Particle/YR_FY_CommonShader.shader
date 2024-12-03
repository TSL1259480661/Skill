//Unity 2019.4.15
//URP Version 7.5.2 December 03,2020
//CustomUI  Editor/CustomShaderGUI.cs

/*Enable Particle Data
Particle Custom Data 形式 Vecotr 4
Particle Renderer    Custom1.xyzw(TEXCOORD0.zw|xy)
*/

Shader "YR/FY_CommonShader"
{
    Properties
    {
        [Header(BlendMode)]
        [HideInInspector] _BlendMode("__mode", Float) = 0.0            
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest",Int) = 4
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", int) = 0

        [Header(Base)]
        [HDR]_MainColor ("Main Color", Color) = (1, 1, 1, 1)      
        //_MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _mainAlpha("Main Alpha", Range(0, 1)) = 1
        [Toggle(_ENABLE_MAIN_COLOR_AS_ALPHA)] _MainColorAsAlpha("MainTexture's R channel As Alpha", int) = 0
        [KeywordEnum(UV0, UV1)] _UVSet_Main ("UV Set For Texture", Float) = 0        
        _MainTex   ("Main Texture", 2D) = "white" {}
        _MainUVSpeedX("MainUVSpeed X", float) = 0
        _MainUVSpeedY("MainUVSpeed Y", float) = 0   

        [Foldout] _Fold01("扭曲面板",Range (0,1)) = 0
        [if(_Fold01)]   [Toggle(_ENABLE_DISTORT)] _Enable_Distort("Enable Distort", int) = 0
        [if(_Fold01)]   [Toggle(_ENABLE_DISTORT_SCREEN)] _Enable_DistortScreen("Enable Distort Screen", int) = 0
        [if(_Fold01)]   [KeywordEnum(UV0, UV1)] _UVSet_Distort("UV Set For Texture", Float) = 0
        [if(_Fold01)]   _DistortTex ("Distort Texture", 2D) = "white" {}
        [if(_Fold01)]   _DistortUVSpeedX("DistortUVSpeed X", float) = 0
        [if(_Fold01)]   _DistortUVSpeedY("DistortUVSpeed Y", float) = 0
        [if(_Fold01)]   _DistortPower("Distort Power", Range(0, 1)) = 0

        [Foldout] _Fold02("遮罩面板",Range (0,1)) = 0
        [if(_Fold02)]   [Toggle(_ENABLE_MASK)] _Enable_Mask("Enable Mask", int) = 0
        [if(_Fold02)]   [KeywordEnum(UV0, UV1)] _UVSet_Mask("UV Set For Texture", Float) = 0        
        [if(_Fold02)]   _MaskTex ("Mask Texture", 2D) = "white" {}                              //Texture repeat
        [if(_Fold02)]   _MaskUVSpeedX("MaskUVSpeed X", float) = 0
        [if(_Fold02)]   _MaskUVSpeedY("MaskUVSpeed Y", float) = 0        

        [Foldout] _Fold03("溶解面板",Range (0,1)) = 0
        [if(_Fold03)]   [Toggle(_ENABLE_DISSOLVE)] _Enable_Dissolve("Enable Dissolve", int) = 0
        [if(_Fold03)]   [Toggle(_ENABLE_DISSOLVE_SOFT)] _ENABLE_DISSOLVE_SOFT("Enable Dissolve Soft", int) = 0
        [if(_Fold03)]   [KeywordEnum(UV0, UV1)] _UVSet_Dissolve("UV Set For Texture", Float) = 0             
        [if(_Fold03)]   _DissolveTex  ("Dissolve Texture", 2D) = "white" {}                     //Texture repeated
        [if(_Fold03)]   _DissolveUVSpeedX("DissolveUVSpeed X", float) = 0
        [if(_Fold03)]   _DissolveUVSpeedY("DissolveUVSpeed Y", float) = 0          
        [if(_Fold03)]   [HDR]_EdgeColor    ("Dissolve Edge Color", Color) = (0, 1, 1, 1)
        

        [if(_Fold03)]   _DissolveEdge   ("Dissolve Edge Width", Range(0, 0.5)) = 0.05           
        [if(_Fold03)]   _DissolvePower  ("Dissolve Power", Range(0, 1)) = 0.3
                            
        [Foldout] _Fold04("菲涅尔面板",Range (0,1)) = 0
        [if(_Fold04)]   [Toggle(_ENABLE_FRESNEL)] _Enable_Fresnel("Enable Fresnel", int) = 0   
        [if(_Fold04)]   [HDR]_FresnelColor ("Fresnel Color", Color) = (1, 1, 0, 1)
        [if(_Fold04)]   _FresnelRange ("Fresnel Range", Range(0, 5)) = 0.2
        [if(_Fold04)]   _FresnelPower ("Fresnel Power", Range(0, 1)) = 1

        [Foldout] _Fold05("地面软过渡", Range(0,1)) = 0
        [if (_Fold05)]   [Toggle(_ENABLE_SOFT)] _ENABLE_SOFT("Enable Soft", int) = 0
        [if (_Fold05)]   _SoftEdge("Soft Edge", Range(0.01, 1)) = 1

        [Header(Particle CustomData Vector4)]
        [Toggle(_ENABLE_CUSTOMDATA1)] _Enable_CUSTOMDATA1("Enable CustomData1 (uv0&Distort&Dissolve)", int) = 0  

        //特效在UI中的裁剪
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        // Rect Mask Support
        _MaskRect ("_MaskRect", Vector) = (0, 0, 0, 0)
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        //特效在UI中的裁剪
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        
        Blend [_SrcBlend] [_DstBlend]
        ZTest [_ZTest]
        ZWrite Off
        Cull [_Cull]

        Pass
        {
            HLSLPROGRAM

            //pragma
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x ps3 xbox360 flash xboxone ps4 psp2
            #pragma target 2.0 

            #pragma vertex vert
            #pragma fragment frag
 
           

            //shader_feature 全部大写
            #pragma shader_feature _ _ENABLE_DISTORT
            #pragma shader_feature _ _ENABLE_DISTORT_SCREEN
            #pragma shader_feature _ _ENABLE_MASK
            #pragma shader_feature _ _ENABLE_DISSOLVE
            #pragma shader_feature _ _ENABLE_DISSOLVE_SOFT
            #pragma shader_feature _ _ENABLE_FRESNEL
            #pragma shader_feature _ _ENABLE_SOFT
            #pragma shader_feature _ _ENABLE_CUSTOMDATA1
            #pragma shader_feature _ _ENABLE_MAIN_COLOR_AS_ALPHA 
            
            #pragma shader_feature _UVSET_MAIN_UV0      _UVSET_MAIN_UV1  
            #pragma shader_feature _UVSET_DISTORT_UV0   _UVSET_DISTORT_UV1
            #pragma shader_feature _UVSET_MASK_UV0      _UVSET_MASK_UV1
            #pragma shader_feature _UVSET_DISSOLVE_UV0  _UVSET_DISSOLVE_UV1
            
            
            

             //include
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            // Rect Mask Support
            #include "Assets/ThirdParty/RectMask/RectMaskHlsl.hlsl"

#if _UVSET_MAIN_UV1|| _UVSET_DISTORT_UV1||_UVSET_MASK_UV1||_UVSET_DISSOLVE_UV1
    #define _UVSET_UV1
#endif
            //structs
            struct Attributes
            {
                float4 positionOS  : POSITION;
                half4  color       : COLOR;
                half3  normalOS    : NORMAL;                
                half4  uv         : TEXCOORD0;   // xy:uv0  zw:uv1   ||    xy:uv0   zw:uv0 offset
    #ifdef _ENABLE_CUSTOMDATA1
                half4  customData1 : TEXCOORD1;  //xy:uv0 offset z:Dissolve s:Distort  || x:Dissolve  y:Distort
    #endif
            };
  
            struct Varying
            {
                float4 positionHCS : SV_POSITION;
                half4  color       : COLOR;
                half3  normalWS    : TEXCOORD0;
                half4 uv0         : TEXCOORD1;    //uv0.xy(mainTex.uv)  uv0.zw(distortTex.uv) 
                half4 uv1         : TEXCOORD2;    //uv1.zw(MaskTex.uv)  uv1.xy(DissolveTex.uv)                                                     
                float3 positionWS  : TEXCOORD3;
#ifdef _ENABLE_CUSTOMDATA1
                half4  customData1 : TEXCOORD4;    //MainUV(Custom1.xy)  Distort(Custom1.w)  Dissolve(Custom1.z)
                SOFTMASK_COORDS(5) // Rect Mask Support
#else
                SOFTMASK_COORDS(4) // Rect Mask Support
#endif
            };

//CBuffer Start
            CBUFFER_START(UnityPerMaterial)
            half4 _MainColor;
            float4 _MainTex_ST;
            float _mainAlpha;

            float _MainUVSpeedX;
            float _MainUVSpeedY;

            half4 _DistortTex_ST;
            float _DistortUVSpeedX;
            float _DistortUVSpeedY;
            float _DistortPower;             

            float4 _MaskTex_ST;
            float _MaskUVSpeedX;
            float _MaskUVSpeedY; 
                       
            float4 _DissolveTex_ST;
            float _DissolveUVSpeedX;
            float _DissolveUVSpeedY;             
            float _DissolvePower;
            float _DissolveEdge;
            half4 _EdgeColor;

            half4 _FresnelColor;
            float _FresnelRange;
            float _FresnelPower;

            float _SoftEdge;
            CBUFFER_END
//CBuffer End

            //Texture & Sampler
            TEXTURE2D(_MainTex);           
            TEXTURE2D(_MaskTex);
            TEXTURE2D(_DissolveTex);
            TEXTURE2D(_DistortTex);

            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_MaskTex);
            SAMPLER(sampler_DissolveTex);
            SAMPLER(sampler_DistortTex);                        

//Vertex Start
            Varying vert (Attributes IN)
            {
                Varying OUT = (Varying)0;

                float3 positionWS =  TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionWS = positionWS;
                OUT.positionHCS  = TransformWorldToHClip(positionWS);

                OUT.color  = IN.color;

                half2 uv0 = IN.uv.xy;
#ifdef _UVSET_UV1
                half2 uv1 = IN.uv.zw;
#endif

            #ifdef _ENABLE_CUSTOMDATA1
#ifdef _UVSET_UV1
                OUT.customData1 = IN.customData1;
#else
                OUT.customData1 = half4(IN.uv.zw,IN.customData1.xy);
#endif
                uv0 += OUT.customData1.xy;
            #endif

            #if _UVSET_MAIN_UV0
                OUT.uv0.xy = TRANSFORM_TEX(uv0.xy, _MainTex); 
                OUT.uv0.xy += float2(_MainUVSpeedX, _MainUVSpeedY) * _Time.y;
            #elif _UVSET_MAIN_UV1
                OUT.uv0.xy = TRANSFORM_TEX(uv1.xy, _MainTex);
                OUT.uv0.xy += float2(_MainUVSpeedX, _MainUVSpeedY) * _Time.y;
            #endif                  


#ifdef _ENABLE_DISTORT  //uv0.xy(Custom1.xy)  uv0.zw(distortTex.uv)  uv1.xy(DissolveTex.uv)  uv1.zw(MaskTex.uv)
                #if _UVSET_DISTORT_UV0
                    float2 distortUV = uv0.xy + float2(_DistortUVSpeedX, _DistortUVSpeedY) * _Time.y;
                #elif _UVSET_DISTORT_UV1
                    float2 distortUV = uv1.xy + float2(_DistortUVSpeedX, _DistortUVSpeedY) * _Time.y;
                #endif
                OUT.uv0.zw = TRANSFORM_TEX( distortUV, _DistortTex);
#endif

#ifdef _ENABLE_DISSOLVE
                #if _UVSET_DISSOLVE_UV0
                    float2 dissolveUV = uv0.xy + float2(_DissolveUVSpeedX, _DissolveUVSpeedY) * _Time.y;
                #elif _UVSET_DISSOLVE_UV1
                    float2 dissolveUV = uv1.xy + float2(_DissolveUVSpeedX, _DissolveUVSpeedY) * _Time.y;
                #endif
                OUT.uv1.xy = TRANSFORM_TEX(dissolveUV, _DissolveTex);
#endif

#ifdef _ENABLE_MASK
                #if _UVSET_MASK_UV0
                    float2 maskUV = uv0.xy + float2(_MaskUVSpeedX, _MaskUVSpeedY) * _Time.y;
                #elif _UVSET_MASK_UV1
                    float2 maskUV = uv1.xy + float2(_MaskUVSpeedX, _MaskUVSpeedY) * _Time.y;
                #endif
                OUT.uv1.zw = TRANSFORM_TEX(maskUV, _MaskTex) ;  
#endif

#ifdef _ENABLE_FRESNEL
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
#endif

SOFTMASK_CALCULATE_COORDS(OUT, IN.positionOS.xyz) // Rect Mask Support
                return OUT;
            }
//Vertex End


//Fragment Start
            float4 frag (Varying IN) : SV_Target
            {
                float4 col = 0;

#ifdef _ENABLE_DISTORT
                float4 col_distort = SAMPLE_TEXTURE2D(_DistortTex, sampler_DistortTex, IN.uv0.zw);

                #ifdef _ENABLE_CUSTOMDATA1
                    _DistortPower = IN.customData1.z; //Custom1.z(粒子面板)
                #endif
                #ifdef _ENABLE_DISTORT_SCREEN
                    half2 screenPos = IN.positionHCS.xy / _ScreenParams.xy;
                    screenPos += _DistortPower * (col_distort.xy - half2(0.5, 0.5));
                    lerp(IN.uv0.xy, col_distort.xy, _DistortPower);
                    col = half4(SampleSceneColor(screenPos.xy ), 1);
                #else
                    float2 baseUV = IN.uv0.xy + _DistortPower * (col_distort.xy - half2(0.5, 0.5));
                    col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, baseUV);
                #endif

#else
                col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                #ifdef _ENABLE_MAIN_COLOR_AS_ALPHA
                col.a = col.r;
                #endif
#endif

#ifdef _ENABLE_MASK
                float4 col_mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, IN.uv1.zw);
                col_mask = saturate(col_mask);
                col = col * col_mask.r;
#endif  

#ifdef _ENABLE_DISSOLVE     //uv0.xy(Custom1.xy)  uv0.zw(distortTex.uv)  uv1.xy(DissolveTex.uv)  uv1.zw(MaskTex.uv)
                float4 col_dissolve = SAMPLE_TEXTURE2D(_DissolveTex, sampler_DissolveTex, IN.uv1.xy);

    #ifdef _ENABLE_CUSTOMDATA1
                //col = col * IN.customData1.x;   //customData1.x = Custom1.z(粒子面板)   控制透明度  IN.customData1.y
                _DissolvePower = IN.customData1.w;  //Custom1.w(粒子面板)
    #endif

    #ifdef _ENABLE_DISSOLVE_SOFT
                float col_percent_alpha = smoothstep(col_dissolve.r, (col_dissolve.r + _DissolveEdge), _DissolvePower);
                _EdgeColor.rgb = lerp(col.rgb, _EdgeColor.rgb, _EdgeColor.a);
                _EdgeColor.a = 0;
                col = lerp(col, _EdgeColor, col_percent_alpha);
    #else
                float col_percent_alpha = step(_DissolvePower, (col_dissolve.r + _DissolveEdge));
                float col_percent_edge = col_percent_alpha - step(_DissolvePower, (col_dissolve.r));
                col = col * (col_percent_alpha - col_percent_edge) + col.a * _EdgeColor * col_percent_edge;
    #endif

#endif

#ifdef _ENABLE_FRESNEL
                half3 N = normalize(IN.normalWS);
                half3 V = normalize(_WorldSpaceCameraPos - IN.positionWS);
                half dotNV = 1-saturate(dot(N, V));
                half4 fresnel = pow(dotNV, _FresnelRange) * _FresnelColor;
                //col = col + fresnel * _FresnelColor * _FresnelPower;
                col = lerp(col, fresnel, _FresnelPower);
#endif
                #ifdef _ENABLE_SOFT
                _mainAlpha = _mainAlpha * min(abs(IN.positionWS.y), _SoftEdge) / _SoftEdge;
                #endif
                
                col = col * _MainColor * IN.color * _mainAlpha;
                col.a *= SOFTMASK_GET_MASK(IN); // Rect Mask Support
                return col;
            }
//Fragement End

            ENDHLSL
        }
    }
}
