Shader "Unlit/DMTK UI/UI Gradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _StencilComp("Stencil Composition", Float) = 8
        _Stencil("Stencil ID", Float) = 0                               // Stencil IDs can range from 0 to 255
        _StencilOp("Stencil Operation", Float) = 0                      
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
    }
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent" "RenderType"="Transparent" 
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        
        Stencil
        {
            Ref [_Stencil]                      // Reference Value: compares contents of stencil buffer against this value.
            Comp [_StencilComp]                 // Comparison Operation: stencil test operation for all pixels [0 means keep contents]
            Pass [_StencilOp]                   // Pass Operation: operation GPU performs to pass stencil & depth tests.
            ReadMask [_StencilReadMask]         // ReadMask: Used as mask for stencil test.
            WriteMask [_StencilWriteMask]       // WriteMask: Used as mask for stencil test.
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (const v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
