Shader "Unlit/NewUnlitShader"
{
    // Properties:
    //----------------------------------------
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Colour", Color) = (1,1,1,1) 
        _TextureSampleAdd ("Texture Additive Color", Color) = (0,0,0,0) 
        
        _StencilComp("Stencil Composition", Float) = 8
        _Stencil("Stencil ID", Float) = 0                              
        _StencilOp("Stencil Operation", Float) = 0                      
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        
        _ColorMask ("Color Mask", Float) = 15
    }
    
    SubShader
    {
        // Tags:
        //----------------------------------------
        Tags 
        { 
            "Queue" = "Transparent"         // transparent objects rendered after opaque and from farthest depth to closest depth
            "RenderType" = "Transparent"    // changes sub shader behaviour - can only be swapped with other sub shaders of same type 
            "PreviewType" = "Plane"         // display preview as a plane (instead of sphere)
            "IgnoreProjector" = "True"      // not effected by "projectors" - (e.g. decal effects, blob shadows, stylised lighting...
            "CanUseSpriteAtlas" = "True"    // can use LegacySpritePacker (depreciated Unity 2020.1)
        }
        
        // Stencil:
        //----------------------------------------
        Stencil
        {
            Ref [_Stencil]                      // Reference Value: passed into stencil buffer
            Comp [_StencilComp]                 // Comparison Operation: comparison operation on Ref to determine Pass/Fail.
            Pass [_StencilOp]                   // Pass Operation: operation GPU performs on fragment on pass.
            ReadMask [_StencilReadMask]         // ReadMask: Used as mask for stencil test.
            WriteMask [_StencilWriteMask]       // WriteMask: Used as mask for stencil test.
        }
        
        // Settings:
        //----------------------------------------
        Blend SrcAlpha OneMinusSrcAlpha     // calculates blended alpha when applying fragment - determines how much light is absorbed by
                                            // the final pixel.
        ZWrite Off                          // disable for semi-transparent, handles order of geometry rendering, when
                                            // disabled will render regardless or object order.
        Lighting Off                        // material & lighting parameters light the vertex (built-in RP only)
        ZTest Always                        // determines depth test pass/failure - always pass (always draw)       
        Cull Off                            // do not cull any polygons
        ColorMask [_ColorMask]              // ???
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _UNITY_UI_CLIP_ALPHACLIP
            #pragma multi_compile_local _UNITY_UI_CLIP_RECT

            // vertex shader inputs:
            struct appdata
            {
                float4 vertex : POSITION;       // vertex positions (object-space)
                float2 texCoord : TEXCOORD0;    // texture coordinate [0] - where on a texture (which pixel) corresponds with this vertex.
                float4 vertexColor : COLOR;     // vertex color
            };

            // vertex shader outputs & fragment shader inputs:
            struct v2f
            {
                float4 vertex : SV_POSITION;        // fragment position (screen-space)
                float4 color : COLOR;               // fragment colour
                float2 texCoord : TEXCOORD0;        // fragment texture coordinate
                float4 worldPosition : TEXCOORD1;   // fragment world position
            };

            sampler2D _MainTex;             // main texture assigned to the material
            float4 _MainTex_ST;
            fixed4 _Color;                  // base color
            fixed4 _TextureSampleAdd;       // color added onto final frag color
            float4 _ClipRect;               // value taken from 2D Rect Mask component

            v2f vert (appdata v) 
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);                         // Set up per instance ID to be accessed within the shader
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(out);         // VR related functionality
                o.vertex = UnityObjectToClipPos(v.vertex);          
                o.texCoord = TRANSFORM_TEX(v.texCoord, _MainTex);
                o.color = v.vertexColor * _Color;
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
