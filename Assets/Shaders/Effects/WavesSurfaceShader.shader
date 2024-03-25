Shader "DMTK/Effects/WavesSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Amplitude ("Amplitude", Float) = 1
        _WaveLength ("WaveLength", Float) = 1
        _Speed ("Speed", Float) = 1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Amplitude;
        float _WaveLength;
        float _Speed;
        
        void vert(inout appdata_full vertexData)
        {
            const float k = 2 * UNITY_PI / _WaveLength;
            const float f = k * (vertexData.vertex.x - _Speed * _Time.y);
            const float3 tangent = normalize(float3(1, k * _Amplitude * cos(f), 0));
            
            vertexData.vertex = float4(vertexData.vertex.x, _Amplitude * sin(f), vertexData.vertex.zw);
            vertexData.normal = float3(-tangent.y, tangent.x, 0);;
            
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
