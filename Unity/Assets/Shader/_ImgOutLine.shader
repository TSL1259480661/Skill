Shader "Custom_Shader/ImageOuterOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineWidth ("Outline Width", float) = 1
        _OutlineColor ("Outline Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _AlphaValue ("Alpha Value", Range(0, 1)) = 0.1
        
        
    }
    SubShader
    {
        Tags     
        {      
            "Queue"="Transparent"      
            "IgnoreProjector"="True"      
            "RenderType"="Transparent"      
            "PreviewType"="Plane"     
            "CanUseSpriteAtlas"="True"     
        }     

        // 源rgba*源a + 背景rgba*(1-源A值)   
        Blend SrcAlpha OneMinusSrcAlpha  
        Cull Off Lighting Off ZWrite Off ZTest Always
        //Blend SrcAlpha OneMinusSrcAlpha

        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
//Add for RectMask2D
#include "UnityUI.cginc"
//End for RectMask2D
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _MainTex_TexelSize;
            float _OutlineWidth;
            float4 _OutlineColor;
            float _AlphaValue;
				//Add for RectMask2D
float4 _ClipRect;
//End for RectMask2D
            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                half2 uv  : TEXCOORD0;
                half2 left : TEXCOORD1;
                half2 right : TEXCOORD2;
                half2 up : TEXCOORD3;
                half2 down : TEXCOORD5;
                					//Add for RectMask2D
float4 worldPosition : TEXCOORD6;
//End for RectMask2D
            };

            v2f vert(appdata i)
            {
                v2f o;
                //Add for RectMask2D
o.worldPosition = i.vertex;
//End for RectMask2D
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);
                o.left = saturate(o.uv + half2(-1, 0) * _MainTex_TexelSize.xy * _OutlineWidth);
                o.right = saturate(o.uv + half2(1, 0) * _MainTex_TexelSize.xy * _OutlineWidth);
                o.up = saturate(o.uv + half2(0, 1) * _MainTex_TexelSize.xy * _OutlineWidth);
                o.down = saturate(o.uv + half2(0, -1) * _MainTex_TexelSize.xy * _OutlineWidth);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                  // 限制UV坐标，防止超出范围
                half2 left = saturate(i.left);
                half2 right = saturate(i.right);
                half2 up = saturate(i.up);
                half2 down = saturate(i.down);

               float transparent = tex2D(_MainTex, left).a + tex2D(_MainTex, right).a + tex2D(_MainTex, up).a + tex2D(_MainTex, down).a;
                fixed4 col = tex2D(_MainTex, i.uv);
                float outlineFactor = step(0.8, col.a);
                fixed4 outlineColor = smoothstep(_AlphaValue, 1, transparent) * _OutlineColor;
                fixed4 finalColor = lerp(outlineColor, col, outlineFactor);
					//Add for RectMask2D
//finalColor.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
//clip(finalColor.a - 0.001);
#ifdef UNITY_UI_ALPHACLIP

#endif
//End for RectMask2D
                return finalColor;
            }
            ENDCG
        }
    }
}

