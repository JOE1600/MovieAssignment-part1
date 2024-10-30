using UnityEngine;

public class TexturedVase : MonoBehaviour
{
    public Material vaseMaterial;    // Material for the vase
    public Texture vaseTexture;      // Main texture for the vase
    public Texture sculptureTexture; // Sculpture texture to blend on the vase sides

    void Start()
    {
        // Create two vases and rotate each by 180 degrees on the x-axis
        GameObject vase1 = CreateVase();
        GameObject vase2 = CreateVase();

        // Position the vases slightly apart and set the y position to 0.928
        vase1.transform.position = new Vector3(-1.5f, 0.928f, 0);
        vase2.transform.position = new Vector3(1.5f, 0.928f, 0);

        // Rotate both vases
        vase1.transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        vase2.transform.rotation = Quaternion.Euler(180f, 0f, 0f);

        // Assign textures and materials to the vases
        AssignMaterialAndTexture(vase1, vaseMaterial, vaseTexture, sculptureTexture);
        AssignMaterialAndTexture(vase2, vaseMaterial, vaseTexture, sculptureTexture);
    }

    // Method to create a vase mesh with manual UV mapping
    GameObject CreateVase()
    {
        GameObject vase = new GameObject("Vase");
        vase.AddComponent<MeshFilter>();
        vase.AddComponent<MeshRenderer>();

        Mesh vaseMesh = new Mesh();

        // Define the vertices (Non-coplanar structure)
        Vector3[] vertices = new Vector3[]
        {
            // Bottom ring
            new Vector3(0f, 0f, 0f),  // Center of base
            new Vector3(0.5f, 0f, 0f), // Base edge 1
            new Vector3(0.35f, 0f, 0.35f), // Base edge 2
            new Vector3(0f, 0f, 0.5f), // Base edge 3
            new Vector3(-0.35f, 0f, 0.35f), // Base edge 4
            new Vector3(-0.5f, 0f, 0f), // Base edge 5
            new Vector3(-0.35f, 0f, -0.35f), // Base edge 6
            new Vector3(0f, 0f, -0.5f), // Base edge 7
            new Vector3(0.35f, 0f, -0.35f), // Base edge 8

            // Top ring (non-coplanar, tapered neck)
            new Vector3(0.25f, 1f, 0f), // Top edge 1
            new Vector3(0.18f, 1f, 0.18f), // Top edge 2
            new Vector3(0f, 1f, 0.25f), // Top edge 3
            new Vector3(-0.18f, 1f, 0.18f), // Top edge 4
            new Vector3(-0.25f, 1f, 0f), // Top edge 5
            new Vector3(-0.18f, 1f, -0.18f), // Top edge 6
            new Vector3(0f, 1f, -0.25f), // Top edge 7
            new Vector3(0.18f, 1f, -0.18f)  // Top edge 8
        };

        // Define the triangles for the vase
        int[] triangles = new int[]
        {
            // Bottom
            0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5,
            0, 5, 6, 0, 6, 7, 0, 7, 8, 0, 8, 1,

            // Side faces
            1, 9, 10, 1, 10, 2, 2, 10, 11, 2, 11, 3,
            3, 11, 12, 3, 12, 4, 4, 12, 13, 4, 13, 5,
            5, 13, 14, 5, 14, 6, 6, 14, 15, 6, 15, 7,
            7, 15, 16, 7, 16, 8, 8, 16, 9, 8, 9, 1
        };

        // Define manual UV mapping for the vertices
        Vector2[] uv = new Vector2[]
        {
            new Vector2(0.5f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0.85f, 0.85f),
            new Vector2(0.5f, 1f), new Vector2(0.15f, 0.85f), new Vector2(0f, 0.5f),
            new Vector2(0.15f, 0.15f), new Vector2(0.5f, 0f), new Vector2(0.85f, 0.15f),
            new Vector2(0f, 1f), new Vector2(0.25f, 1f), new Vector2(0.5f, 1f),
            new Vector2(0.75f, 1f), new Vector2(1f, 1f), new Vector2(0f, 0.75f),
            new Vector2(0.25f, 0.75f), new Vector2(0.5f, 0.75f)
        };

        vaseMesh.vertices = vertices;
        vaseMesh.triangles = triangles;
        vaseMesh.uv = uv;
        vaseMesh.RecalculateNormals();

        vase.GetComponent<MeshFilter>().mesh = vaseMesh;
        return vase;
    }

    // Method to assign material and texture to a GameObject
    void AssignMaterialAndTexture(GameObject obj, Material material, Texture vaseTexture, Texture sculptureTexture)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        renderer.material = material;

        if (vaseTexture != null && sculptureTexture != null)
        {
            material.SetTexture("_MainTex", vaseTexture);
            material.SetTexture("_DetailAlbedoMap", sculptureTexture);
            material.SetFloat("_DetailNormalMapScale", 1f);
            material.SetFloat("_DetailMask", 0.5f);

            Debug.Log("Main Vase Texture applied: " + vaseTexture.name);
            Debug.Log("Sculpture Texture applied: " + sculptureTexture.name);
        }
        else
        {
            Debug.LogWarning("Missing one or more textures.");
        }
    }
}
