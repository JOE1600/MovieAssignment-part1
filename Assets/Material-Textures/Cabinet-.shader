Shader "Unlit/Cabinet"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1, 1, 1, 1) // Base color
        _Metallic ("Metallic", Range(0, 1)) = 0.0 // Metallic property
        _Glossiness ("Smoothness", Range(0, 1)) = 0.5 // Smoothness property
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
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

            // Additional properties for color and smoothness
            fixed4 _Color;
            float _Metallic;
            float _Glossiness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // Adjust the color based on metallic and smoothness
                // For a wooden effect, we might want to simulate the glossiness
                fixed4 col = texColor;

                // Blend with base color
                col.rgb = col.rgb * (1 - _Metallic) + _Metallic; // Blend metallic
                col.a = texColor.a; // Maintain original alpha

                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
