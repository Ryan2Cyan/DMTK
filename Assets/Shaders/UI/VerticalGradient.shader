Shader "DMTK/UI/VerticalGradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TopColor ("Top Colour", Color) = (1,1,1,1) 
        _BottomColor ("Bottom Colour", Color) = (0,0,0,1) 
        _GradientOffset ("Gradient Offset", Range(-1, 1)) = 0
    }
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent"
            "RenderType" = "Transparent" 
            "PreviewType" = "Plane"
            "IgnoreProjector" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off 
        Lighting Off 
        Cull Off
        ZTest Always
        
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
                float4 gradient : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _TopColor;
            fixed4 _BottomColor;
            float _GradientOffset;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.gradient = lerp(_BottomColor, _TopColor, o.uv.y + _GradientOffset);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.gradient;
            }
            ENDCG
        }
    }
}
