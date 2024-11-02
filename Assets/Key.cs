using UnityEngine;
using System.Collections;

public class TreasureKeyScene : MonoBehaviour
{
    public Material keyMaterial;
    public Texture keyTexture;
    public AudioClip keySound;
    public GameObject humanModel;
    public AudioClip destructionSound; // Sound for when the key is destroyed
    private GameObject key;
    private GameObject fireKey;
    private AudioSource keyAudioSource;
    private AudioSource fireKeyAudioSource;

    void Start()
    {
        // Enable fog in the scene
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.gray;
        RenderSettings.fogDensity = 0.05f;

        // Create the key and add sound setup
        key = CreateKey();
        keyAudioSource = AddSoundEffect(key);

        // Create the fire key and add sound setup
        fireKey = CreateFireKey(new Vector3(2.6f, 2.38f, -3.67f));
        fireKeyAudioSource = AddSoundEffect(fireKey);

        // Start the coroutine to destroy the key at 35 seconds
        StartCoroutine(DestroyKeyAtTime(35f));

    }

    void Update()
{
    // Check distance between humanModel and key
    if (humanModel != null)
    {
        // Check if key and fireKey still exist
        if (key != null)
        {
            float distanceToKey = Vector3.Distance(humanModel.transform.position, key.transform.position);

            // Play or stop sound for the key if within range
            if (distanceToKey <= 4f)
            {
                if (!keyAudioSource.isPlaying)
                {
                    keyAudioSource.Play();
                }
            }
            else
            {
                if (keyAudioSource.isPlaying)
                {
                    keyAudioSource.Stop();
                }
            }
        }

        if (fireKey != null)
        {
            float distanceToFireKey = Vector3.Distance(humanModel.transform.position, fireKey.transform.position);

            // Play or stop sound for the fire key if within range
            if (distanceToFireKey <= 4f)
            {
                if (!fireKeyAudioSource.isPlaying)
                {
                    fireKeyAudioSource.Play();
                }
            }
            else
            {
                if (fireKeyAudioSource.isPlaying)
                {
                    fireKeyAudioSource.Stop();
                }
            }
        }
    }
}

    private IEnumerator DestroyKeyAtTime(float time)
{
    // Wait for the specified time
    yield return new WaitForSeconds(time);

    // Play the destruction sound effect before destroying the key
    PlayDestructionSound();

    // Destroy the key if it exists
    if (key != null)
    {
        Destroy(key);
        Debug.Log("Key destroyed at " + time + " seconds.");
    }
}

private void PlayDestructionSound()
{
    // Create a temporary GameObject to play the sound
    GameObject soundObject = new GameObject("DestructionSound");
    AudioSource audioSource = soundObject.AddComponent<AudioSource>();
    
    // Set the clip and properties
    audioSource.clip = keySound; // Use the appropriate destruction sound clip
    audioSource.playOnAwake = false;
    audioSource.volume = 1.0f; // Adjust volume as needed
    
    // Play the sound
    audioSource.Play();
    
    // Destroy the sound object after the clip finishes playing
    Destroy(soundObject, audioSource.clip.length);
}

    GameObject CreateKey()
    {
        GameObject key = new GameObject("Key");
        key.transform.position = new Vector3(-3.95f, 2.38f, 3.29f);

        MeshFilter meshFilter = key.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = key.AddComponent<MeshRenderer>();

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

        meshFilter.mesh = CreateKeyMesh();
        AddLighting(key);

        return key;
    }

    AudioSource AddSoundEffect(GameObject key)
    {
        // Add an AudioSource component to the key and set properties
        AudioSource audioSource = key.AddComponent<AudioSource>();
        audioSource.clip = keySound;
        audioSource.playOnAwake = false; // Play only when within range
        audioSource.loop = true;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 1.0f;

        return audioSource;
    }

    GameObject CreateFireKey(Vector3 position)
    {
        GameObject fireKey = new GameObject("FireKey");
        fireKey.transform.position = position;

        MeshFilter meshFilter = fireKey.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = fireKey.AddComponent<MeshRenderer>();

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
            Debug.LogWarning("No material assigned to the fire key. Please assign a material in the Unity Editor.");
        }

        meshFilter.mesh = CreateKeyMesh();
        AddLighting(fireKey);

        return fireKey;
    }

    void AddLighting(GameObject key)
    {
        GameObject pointLightObj = new GameObject("KeyPointLight");
        Light pointLight = pointLightObj.AddComponent<Light>();
        pointLight.type = LightType.Point;
        pointLight.intensity = 5f;
        pointLight.range = 5f;
        pointLight.color = Color.yellow;
        pointLight.transform.position = key.transform.position;
        pointLight.transform.parent = key.transform;

        GameObject spotLightObj = new GameObject("KeySpotLight");
        Light spotLight = spotLightObj.AddComponent<Light>();
        spotLight.type = LightType.Spot;
        spotLight.spotAngle = 45f;
        spotLight.intensity = 3f;
        spotLight.range = 5f;
        spotLight.color = Color.yellow;
        spotLight.transform.position = key.transform.position + Vector3.up;
        spotLight.transform.rotation = Quaternion.Euler(90, 0, 0);
        spotLight.transform.parent = key.transform;
    }

    Mesh CreateKeyMesh()
    {
        Mesh mesh = new Mesh();
        Mesh headMesh = CreateKeyHead();
        Mesh shaftMesh = CreateKeyShaft();
        Mesh teethMesh = CreateKeyTeeth();

        CombineInstance[] combine = new CombineInstance[3];
        combine[0].mesh = headMesh;
        combine[0].transform = Matrix4x4.TRS(Vector3.up * 0.1f, Quaternion.identity, Vector3.one);

        combine[1].mesh = shaftMesh;
        combine[1].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        combine[2].mesh = teethMesh;
        combine[2].transform = Matrix4x4.TRS(Vector3.down * 0.05f, Quaternion.identity, Vector3.one);

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
