using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundCreator : MonoBehaviour
{
    public Material groundMaterial;
    public Material postMaterial;

    void Start()
    {
        // Create the expanded playground
        GameObject ground = CreatePlayingGround();
        ground.transform.position = new Vector3(0, 0, 0);

        // Set the playground ground material to green
        groundMaterial.color = Color.green;
        ground.GetComponent<Renderer>().material = groundMaterial;

        // Place checkposts diagonally across the playground
        PlaceCheckpostsDiagonally(new Vector3(-150, 0, -150), new Vector3(150, 0, 150), 10); // Adjusted to match the playground size
    }

    // Create a larger playing ground using the Sweep function
    GameObject CreatePlayingGround()
    {
        // Enlarged ground profile (a much wider rectangle)
        Vector3[] groundProfile = new Vector3[]
        {
            new Vector3(-150, 0, 0), // Left side of the large ground
            new Vector3(150, 0, 0)   // Right side of the large ground
        };

        // Path for the ground (longer straight path along the z-axis)
        Matrix4x4[] groundPath = new Matrix4x4[]
        {
            Matrix4x4.Translate(new Vector3(0, 0, -150)),
            Matrix4x4.Translate(new Vector3(0, 0, 150))
        };

        // Create the ground mesh using the sweep method
        Mesh groundMesh = MeshUtilities.Sweep(groundProfile, groundPath, false);

        // Create a game object for the enlarged ground
        GameObject ground = new GameObject("PlayingGround");
        MeshRenderer meshRenderer = ground.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = ground.AddComponent<MeshFilter>();
        meshFilter.mesh = groundMesh;

        return ground;
    }

    // Place checkposts along a diagonal from startPoint to endPoint
    void PlaceCheckpostsDiagonally(Vector3 startPoint, Vector3 endPoint, int numberOfCheckposts)
    {
        // Calculate the step size for each checkpost
        Vector3 step = (endPoint - startPoint) / (numberOfCheckposts - 1);

        // Create checkposts along the diagonal
        for (int i = 0; i < numberOfCheckposts; i++)
        {
            Vector3 position = startPoint + step * i;
            CreateCheckpost(position);
        }
    }

    // Create a checkpost with a cylinder (post) and a pyramid (top)
    void CreateCheckpost(Vector3 position)
    {
        // Create the checkpost post (cylinder)
        GameObject post = new GameObject("Checkpost");
        post.tag = "Checkpost"; // Ensure the tag is set here
        post.transform.position = position;

        // Cylinder
        MeshFilter postFilter = post.AddComponent<MeshFilter>();
        postFilter.mesh = MeshUtilities.Cylinder(20, 0.5f, 3f); // 20 segments, radius 0.5, height 3
        MeshRenderer postRenderer = post.AddComponent<MeshRenderer>();
        postRenderer.material = postMaterial;

        // Create the checkpost top (pyramid)
        GameObject top = new GameObject("CheckpostTop");
        top.transform.position = new Vector3(position.x, position.y + 3f, position.z); // Offset for height
        MeshFilter topFilter = top.AddComponent<MeshFilter>();
        topFilter.mesh = MeshUtilities.Pyramid(1.2f); // Slightly larger pyramid with size 1.2
        MeshRenderer topRenderer = top.AddComponent<MeshRenderer>();
        topRenderer.material = postMaterial;

        top.transform.SetParent(post.transform); // Make the top a child of the post
    }
}
