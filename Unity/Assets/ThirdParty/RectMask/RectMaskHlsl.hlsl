#ifndef SOFTMASK_INCLUDED
#define SOFTMASK_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

/*  API Reference
    -------------

    #define SOFTMASK_COORDS(idx)
        Add it to the declaration of the structure that is passed from the vertex shader
        to the fragment shader.
          idx    The number of interpolator to use. Specify the first free TEXCOORD index.

    #define SOFTMASK_CALCULATE_COORDS(OUT, pos)
        Use it in the vertex shader to calculate mask-related data.
          OUT    An instance of the output structure that will be passed to the fragment
                 shader. It should be of the type that contains a SOFTMASK_COORDS()
                 declaration.
          pos    A source vertex position that have been passed to the vertex shader.

    #define SOFTMASK_GET_MASK(IN)
        Use it in the fragment shader to finally compute the mask value.
          IN     An instance of the vertex shader output structure. It should be of type
                 that contains a SOFTMASK_COORDS() declaration.


*/

# define SOFTMASK_COORDS(idx)                  float4 maskPosition : TEXCOORD ## idx;
//# define SOFTMASK_CALCULATE_COORDS(OUT, pos) (OUT).maskPosition = mul(UNITY_MATRIX_M, pos)
# define SOFTMASK_CALCULATE_COORDS(OUT, pos) (OUT).maskPosition = float4(TransformObjectToWorld(pos), 1.0f);
# define SOFTMASK_GET_MASK(IN)                 SoftMask_GetMaskClipping((IN).maskPosition.xy)

float4 _MaskRect;

//判断是否在裁剪区域内
inline float2 SoftMask_GetMaskClipping(float2 worldPos) {
    // 如果 _MaskRect 是无效区域则不进行裁剪
    if (_MaskRect.z <= _MaskRect.x || _MaskRect.w <= _MaskRect.y) {
        return 1.0f; // 不裁剪
    }

    //UnityGet2DClipping
    float isInside = 1.0f;
    isInside *= (worldPos.x >= _MaskRect.x);
    isInside *= (worldPos.x <= _MaskRect.z);
    isInside *= (worldPos.y >= _MaskRect.y);
    isInside *= (worldPos.y <= _MaskRect.w);
    return isInside;
}

#endif