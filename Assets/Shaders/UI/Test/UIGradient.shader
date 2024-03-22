Shader "Unlit/DMTK UI/UI Gradient"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _StencilComp("Stencil Composition", Float) = 8
        _Stencil("Stencil ID", Float) = 0                               // Stencil IDs can range from 0 to 255
        _StencilOp("Stencil Operation", Float) = 0                      
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent" 
            "RenderType"= "Transparent" 
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        
        Stencil
        {
            Ref [_Stencil]                      // Reference Value: passed into stencil buffer
            Comp [_StencilComp]                 // Comparison Operation: comparison operation on Ref to determine Pass/Fail.
            Pass [_StencilOp]                   // Pass Operation: operation GPU performs on fragment on pass.
            ReadMask [_StencilReadMask]         // ReadMask: Used as mask for stencil test.
            WriteMask [_StencilWriteMask]       // WriteMask: Used as mask for stencil test.
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]                  // fiddles around with color channels.
        
        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _UNITY_UI_CLIP_ALPHACLIP

            // vertex shader input:
            struct appdata_t
            {
                float4 vertex : POSITION;           // vert positions (object-space) 
                float4 color : COLOR;               // color of vertex
                float2 texcoord : TEXCOORD0;        // texture coordinate of texture 0 
                UNITY_VERTEX_INPUT_INSTANCE_ID      // defines instance ID
            };

            // fragment shader output:
            struct v2f
            {
                float4 vertex : SV_POSITION;            // pixel position (screen-space)
                float4 color : COLOR;                   // pixel color
                float2 texcoord : TEXCOORD0;            // texture coordinate of texture 0 
                float4 worldPosition : TEXCOORD1;       // texture coordinate of texture 1 
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // shader parameters:
            sampler2D _MainTex;             
            float4 _MainTex_ST;             
            fixed4 _Color;                  
            fixed4 _TextureSampleAdd;       
            float4 _ClipRect;               

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);                         // access instance id of vertex shader
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(out);         // VR related functionality
                o.worldPosition = v.vertex;                         
                o.vertex = UnityObjectToClipPos(o.worldPosition);   // convert vertex into screen space
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);   // make sure texture scale and offset applied correctly
                o.color = v.color * _Color;                         // multiplicative colour blending
                return o;
            }

            fixed4 frag (const v2f i) : SV_Target
            {
                half4 col = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd) * i.color;        // calculate pixel colour

                #ifdef _UNITY_UI_CLIP_RECT
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif

                #ifdef _UNITY_UI_CLIP_ALPHACLIP
                clip(col.a - 0.001);
                #endif
                
                return col;
            }
            ENDCG
        }
    }
}
