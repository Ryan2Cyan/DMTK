Shader "DMTK/Effects/Waves"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _Amplitude ("Amplitude", Float) = 1
        _WaveLength ("Wave Length", Float) = 1
        _Speed ("Speed", Float) = 1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "PreviewType" = "Plane"
        }
        
            
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _BaseColor;
            float _Amplitude;
            float _WaveLength;
            float _Speed;
            
            v2f vert (appdata v)
            {
                // implement sine wave [y = sin(x)]:
                const float k = 2 * UNITY_PI / _WaveLength;
                const float f = k * (v.vertex.x - _Speed * _Time.y);
                float3 tangent = normalize(float3(1, k * _Amplitude * cos(f), 0));
                
                v.vertex = float4(v.vertex.x, _Amplitude * sin(f), v.vertex.zw);      // vertex position
                v.normal = float3(-tangent.y, tangent.x, 0);                          // vertex normal
                
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _BaseColor;
                return col;
            }
            ENDCG
        }
    }
}
