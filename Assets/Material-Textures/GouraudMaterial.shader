Shader "Unlit/GouraudMaterialWithTexture"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {} // Main texture to be applied
        _MShininess("Material shininess", Float) = 50.0 // Shininess for specular
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            // Enable lighting for this pass
            Lighting On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0; // Add texture UV
            };

            struct v2f
            {
                fixed4 c : COLOR;       // Lighting color at vertex
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;  // Pass UV to fragment shader
            };

            // Declare material properties
            sampler2D _MainTex; // Texture sampler
            float _MShininess;

            // Vertex shader for Gouraud shading
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 P = UnityObjectToViewPos(v.vertex);
                float3 N = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float3 L = normalize(unity_LightPosition[0].xyz - P * unity_LightPosition[0].w);

                // Sample texture color from UVs
                fixed4 textureColor = tex2D(_MainTex, v.uv);

                // Ambient lighting
                fixed4 ambient = UNITY_LIGHTMODEL_AMBIENT;

                // Diffuse lighting
                float diffuseIntensity = max(dot(N, L), 0);
                fixed4 diffuse = textureColor * unity_LightColor[0] * diffuseIntensity;

                // Specular lighting
                float3 R = normalize(reflect(-L, N));
                float3 V = normalize(-P);
                float specularIntensity = pow(max(dot(R, V), 0), _MShininess);
                fixed4 specular = unity_LightColor[0] * specularIntensity;

                // Combine lighting components and texture color
                o.c = clamp(ambient + diffuse + specular, 0, 1);

                // Pass the UVs to the fragment shader
                o.uv = v.uv;

                return o;
            }

            // Fragment shader to return the interpolated color
            fixed4 frag(v2f i) : SV_Target
            {
                return i.c;
            }

            ENDCG
        }
    }

    FallBack "Diffuse"
}
