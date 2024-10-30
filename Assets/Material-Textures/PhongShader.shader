Shader "Unlit/PhongShaderWithTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Main texture to be applied
        _MShininess("Material shininess", Float) = 50.0 // Shininess for specular
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
                float4 vertex : SV_POSITION;
                float3 N : NORMAL;
                float3 P : TEXCOORD0;
                float2 uv : TEXCOORD1; // Pass UV to the fragment shader
            };

            // Declare material properties
            sampler2D _MainTex; // Texture sampler
            float _MShininess;

            // Phong Illumination Function
            fixed4 phongIllumination(float3 P, float3 N, float3 L, fixed4 textureColor)
            {
                // Ambient calculation
                fixed4 ambient = UNITY_LIGHTMODEL_AMBIENT;

                // Diffuse calculation
                float diffuseIntensity = max(dot(N, L), 0);
                fixed4 diffuse = textureColor * unity_LightColor[0] * diffuseIntensity;

                // Specular calculation
                float3 R = normalize(reflect(-L, N));
                float3 V = normalize(-P);
                float specularIntensity = pow(max(dot(R, V), 0), _MShininess);
                fixed4 specular = unity_LightColor[0] * specularIntensity;

                return clamp(ambient + diffuse + specular, 0, 1);
            }

            // Vertex shader: Just calculate P, N, and L
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Calculate position, normal, and light direction in view space
                float3 P = UnityObjectToViewPos(v.vertex);
                float3 N = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float3 L = unity_LightPosition[0].xyz - P * unity_LightPosition[0].w;

                // Pass the position, normal, and UV to the fragment shader
                o.P = P;
                o.N = N;
                o.uv = v.uv; // Pass UV

                return o;
            }

            // Fragment shader: Performs Phong shading using interpolated values
            fixed4 frag(v2f i) : SV_Target
            {
                // Sample texture color
                fixed4 textureColor = tex2D(_MainTex, i.uv);

                // Calculate light direction in the fragment shader
                float3 L = unity_LightPosition[0].xyz - i.P * unity_LightPosition[0].w;

                // Call phongIllumination function and return the shaded color
                return phongIllumination(i.P, i.N, L, textureColor);
            }

            ENDCG
        }
    }

    FallBack "Diffuse"
}
