using UnityEngine;

public class TreasureKeyScene : MonoBehaviour
{
    public Material keyMaterial;
    public Texture keyTexture;
    public GameObject fireParticlePrefab; // Reference to the fire particle prefab

    void Start()
    {
        // Enable fog in the scene
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.gray; // Set fog color
        RenderSettings.fogDensity = 0.05f; // Adjust fog density

        // Create the key
        CreateKey();

        // Create the second key with fire effect
        CreateFireKey(new Vector3(2.6f, 2.38f, -3.67f));
    }

    void CreateFireKey(Vector3 position)
    {
        // Create a new GameObject for the fire key
        GameObject fireKey = new GameObject("FireKey");

        // Set the position of the fire key
        fireKey.transform.position = position;

        // Create the mesh filter and renderer
        MeshFilter meshFilter = fireKey.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = fireKey.AddComponent<MeshRenderer>();

        // Assign the material and texture for the fire key
        if (keyMaterial != null)
        {
            if (keyTexture != null)
            {
                keyMaterial.mainTexture = keyTexture; // Use the same texture or a different one if desired
            }
            meshRenderer.material = keyMaterial; // Assign the same material or a different one
        }
        else
        {
            Debug.LogWarning("No material assigned to the fire key. Please assign a material in the Unity Editor.");
        }

        // Create the key mesh
        meshFilter.mesh = CreateKeyMesh(); // You can create a different mesh if desired

        // Add fire effect
        AddFireEffect(fireKey);

        // Instantiate the fire particle prefab
        if (fireParticlePrefab != null)
        {
            GameObject fireParticles = Instantiate(fireParticlePrefab, fireKey.transform);
            fireParticles.transform.localPosition = Vector3.zero; // Position the particles at the key's location
            fireParticles.transform.localScale = Vector3.one; // Set the scale if needed
        }

        // Add lights
        AddLighting(fireKey);
    }
    void CreateKey()
    {
        // Create a new GameObject for the key
        GameObject key = new GameObject("Key");

        // Set the position of the key
        key.transform.position = new Vector3(-3.95f, 2.38f, 3.29f);

        // Create the mesh filter and renderer
        MeshFilter meshFilter = key.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = key.AddComponent<MeshRenderer>();

        // Assign the material and texture
        if (keyMaterial != null)
        {
            if (keyTexture != null)
            {
                keyMaterial.mainTexture = keyTexture;
            }
            meshRenderer.material = keyMaterial;
        }
        else
        {
            Debug.LogWarning("No material assigned to the key. Please assign a material in the Unity Editor.");
        }

        // Create the key mesh
        meshFilter.mesh = CreateKeyMesh();

        // Add lights
        AddLighting(key);
    }

    

    void AddFireEffect(GameObject fireKey)
{
    // Create a Particle System for the fire effect
    ParticleSystem fireParticles = fireKey.AddComponent<ParticleSystem>();
    ParticleSystem.MainModule mainModule = fireParticles.main;
    mainModule.startLifetime = 1.5f;
    mainModule.startSpeed = 1.5f; // Increased speed for upward movement
    mainModule.startSize = 0.5f;
    mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.yellow, Color.red);

    // Set the gravity modifier to zero for floating effect
    mainModule.gravityModifier = 0f; // No gravity for upward movement

    // Configure emission
    ParticleSystem.EmissionModule emissionModule = fireParticles.emission;
    emissionModule.rateOverTime = 50; // Particles per second

    // Configure shape
    ParticleSystem.ShapeModule shapeModule = fireParticles.shape;
    shapeModule.shapeType = ParticleSystemShapeType.Cone;
    shapeModule.angle = 25f;
    shapeModule.radius = 0.1f;

    // Adjust start color to fade to transparency
    ParticleSystem.ColorOverLifetimeModule colorModule = fireParticles.colorOverLifetime;
    colorModule.enabled = true;
    Gradient gradient = new Gradient();
    gradient.SetKeys(
        new GradientColorKey[] { new GradientColorKey(Color.yellow, 0f), new GradientColorKey(Color.red, 0.5f), new GradientColorKey(new Color(1, 0, 0, 0), 1f) },
        new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 1f) }
    );
    colorModule.color = gradient;

    fireParticles.Play(); // Start the particle system
}


    void AddLighting(GameObject key)
    {
        // Create a Point Light
        GameObject pointLightObj = new GameObject("KeyPointLight");
        Light pointLight = pointLightObj.AddComponent<Light>();
        pointLight.type = LightType.Point;
        pointLight.intensity = 5f;
        pointLight.range = 5f;
        pointLight.color = Color.yellow; // Same color as the key
        pointLight.transform.position = key.transform.position; // Position it at the key's location
        pointLight.transform.parent = key.transform; // Parent to key for easy management

        // Create a Spotlight
        GameObject spotLightObj = new GameObject("KeySpotLight");
        Light spotLight = spotLightObj.AddComponent<Light>();
        spotLight.type = LightType.Spot;
        spotLight.spotAngle = 45f;
        spotLight.intensity = 3f;
        spotLight.range = 5f;
        spotLight.color = Color.yellow; // Match the color of the key
        spotLight.transform.position = key.transform.position + Vector3.up; // Position above the key
        spotLight.transform.rotation = Quaternion.Euler(90, 0, 0); // Pointing downwards
        spotLight.transform.parent = key.transform; // Parent to key

        // Add a flickering effect to the point light
        pointLightObj.AddComponent<FlickerLight>();
    }


    Mesh CreateKeyMesh()
    {
        Mesh mesh = new Mesh();

        // Create components for the key using basic shapes
        Mesh headMesh = CreateKeyHead();
        Mesh shaftMesh = CreateKeyShaft();
        Mesh teethMesh = CreateKeyTeeth();

        // Combine the meshes
        CombineInstance[] combine = new CombineInstance[3];
        combine[0].mesh = headMesh;
        combine[0].transform = Matrix4x4.TRS(Vector3.up * 0.1f, Quaternion.identity, Vector3.one); // Adjusted position for head

        combine[1].mesh = shaftMesh;
        combine[1].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one); // Positioning the shaft

        combine[2].mesh = teethMesh;
        combine[2].transform = Matrix4x4.TRS(Vector3.down * 0.05f, Quaternion.identity, Vector3.one); // Positioned closer to the shaft's tip

        mesh.CombineMeshes(combine);
        mesh.RecalculateNormals();

        return mesh;
    }

    Mesh CreateKeyHead()
    {
        float radius = 0.05f;
        GameObject head = new GameObject("KeyHead");

        // Generate the sphere
        MeshUtilities.Sphere(head, radius);
        Mesh headMesh = head.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = headMesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        // Spherical mapping based on vertex positions
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = vertices[i].normalized;
            uvs[i] = new Vector2(
                0.5f + Mathf.Atan2(v.z, v.x) / (2f * Mathf.PI),
                0.5f - Mathf.Asin(v.y) / Mathf.PI
            );
        }

        headMesh.uv = uvs;
        GameObject.Destroy(head);
        return headMesh;
    }

    Mesh CreateKeyShaft()
    {
        int segments = 12;
        float radius = 0.02f;
        float height = 0.1f;
        Mesh shaftMesh = MeshUtilities.Cylinder(segments, radius, height);
        Vector3[] vertices = shaftMesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        // Cylindrical UV mapping along the shaft
        for (int i = 0; i < vertices.Length; i++)
        {
            float u = Mathf.Atan2(vertices[i].z, vertices[i].x) / (2 * Mathf.PI) + 0.5f;
            float v = vertices[i].y / height + 0.5f;
            uvs[i] = new Vector2(u, v);
        }

        shaftMesh.uv = uvs;
        return shaftMesh;
    }

    Mesh CreateKeyTeeth()
    {
        float toothWidth = 0.03f;
        float toothHeight = 0.05f;
        float toothDepth = 0.02f;

        Mesh teeth = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            // Front face
            new Vector3(-toothWidth, 0, -toothDepth),
            new Vector3(toothWidth, 0, -toothDepth),
            new Vector3(toothWidth, toothHeight, -toothDepth),
            new Vector3(-toothWidth, toothHeight, -toothDepth),
            // Back face
            new Vector3(-toothWidth, 0, toothDepth),
            new Vector3(toothWidth, 0, toothDepth),
            new Vector3(toothWidth, toothHeight, toothDepth),
            new Vector3(-toothWidth, toothHeight, toothDepth)
        };

        int[] tris = new int[]
        {
            // Front face
            0, 2, 1,
            0, 3, 2,
            // Back face
            4, 5, 6,
            4, 6, 7,
            // Left face
            0, 3, 7,
            0, 7, 4,
            // Right face
            1, 2, 6,
            1, 6, 5,
            // Top face
            2, 3, 7,
            2, 7, 6,
            // Bottom face
            0, 4, 5,
            0, 5, 1
        };

        teeth.vertices = vertices;
        teeth.triangles = tris;

        // Ensure the UV array matches the vertices array
        Vector2[] uvs = new Vector2[vertices.Length];

        // Map each face individually
        uvs[0] = new Vector2(0, 0); uvs[1] = new Vector2(1, 0); uvs[2] = new Vector2(1, 1); uvs[3] = new Vector2(0, 1); // Front face
        uvs[4] = new Vector2(0, 0); uvs[5] = new Vector2(1, 0); uvs[6] = new Vector2(1, 1); uvs[7] = new Vector2(0, 1); // Back face

        teeth.uv = uvs;
        teeth.RecalculateNormals();

        return teeth;
    }
}

public class FlickerLight : MonoBehaviour
{
    private Light pointLight;
    public float minIntensity = 2f;
    public float maxIntensity = 8f;
    public float flickerSpeed = 4f;

    void Start()
    {
        pointLight = GetComponent<Light>();
    }

    void Update()
    {
        pointLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * flickerSpeed, 1));
    }
}
