Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Colour", Color) = (1,1,1,1) 
        _TextureSampleAdd ("Texture Additive Color", Color) = (0,0,0,0) 
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha     // GPU multiplies input by (1 - srcAlpha). 
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _UNITY_UI_CLIP_ALPHACLIP
            #pragma multi_compile_local _UNITY_UI_CLIP_RECT
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 texCoord : TEXCOORD0;
                float4 vertexColor : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 texCoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;       // color added onto final frag color.
            float4 _ClipRect; 

            v2f vert (appdata v) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texCoord = TRANSFORM_TEX(v.texCoord, _MainTex);
                o.color = v.vertexColor;
                return o;
            }

            fixed4 frag (const v2f i) : SV_Target
            {
                // calculate final frag color:
                half4 col = (tex2D(_MainTex, i.texCoord) + _TextureSampleAdd) * i.color;

                //  check if world position is within parent 2d rect, a = 1 if it is 0 otherwise:
                #ifdef _UNITY_UI_CLIP_RECT
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif

                // discard the pixel is alpha is <= 0:
                #ifdef _UNITY_UI_CLIP_ALPHACLIP
                clip(col.a - 0.001);
                #endif  
                
                return col;
            }
            ENDCG
        }
    }
}
