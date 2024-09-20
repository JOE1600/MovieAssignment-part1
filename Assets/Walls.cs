using UnityEngine;
using System.Collections.Generic;

public class WallCreator : MonoBehaviour
{
    public Material wallMaterial;

    void Awake()
    {
        // Ensure that components are added only once and correctly initialized
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer.material = wallMaterial;

        Mesh wallMesh = CreateWallMesh();
        meshFilter.mesh = wallMesh;
    }

    public static Mesh CreateWallMesh()
    {
        // Define the profile (cross-section) of the wall
        Vector3[] profile = new Vector3[]
        {
            new Vector3(-0.5f, 0, 0),
            new Vector3(0.5f, 0, 0),
            new Vector3(0.5f, 5, 0),
            new Vector3(-0.5f, 5, 0)
        };

        // Define the path (along which the wall will be swept)
        Vector3[] path = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 5),
            new Vector3(0, 0, 10),
            new Vector3(0, 0, 15),
            new Vector3(0, 0, 20)
        };

        // Convert path into Matrix4x4 for sweeping
        Matrix4x4[] pathMatrices = new Matrix4x4[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            pathMatrices[i] = Matrix4x4.Translate(path[i]);
        }

        // Use the Sweep function to create the main wall mesh
        Mesh wallMesh = MeshUtilities.Sweep(profile, pathMatrices, false);

        // Now add the caps at both ends of the wall
        AddCapsToWall(wallMesh, profile, pathMatrices);

        return wallMesh;
    }

    public static void AddCapsToWall(Mesh wallMesh, Vector3[] profile, Matrix4x4[] pathMatrices)
    {
        // Create caps by sweeping a circular path on both ends of the wall
        Vector3[] capProfile = new Vector3[] 
        {
            new Vector3(0, 0, 0),
            new Vector3(0.5f, 0, 0),
            new Vector3(0.5f, 5, 0),
            new Vector3(0, 5, 0)
        };

        // Use the existing pathMatrices for the cap's start and end positions
        Matrix4x4 startMatrix = pathMatrices[0]; // First matrix for one end
        Matrix4x4 endMatrix = pathMatrices[pathMatrices.Length - 1]; // Last matrix for the other end

        // Use the Sweep function to create the caps for both ends
        Mesh startCapMesh = MeshUtilities.Sweep(capProfile, new Matrix4x4[] { startMatrix }, true);
        Mesh endCapMesh = MeshUtilities.Sweep(capProfile, new Matrix4x4[] { endMatrix }, true);

        // Combine wall mesh with caps
        CombineMeshes(wallMesh, startCapMesh, endCapMesh);
    }

    public static void CombineMeshes(Mesh mainMesh, params Mesh[] otherMeshes)
    {
        List<CombineInstance> combine = new List<CombineInstance>();

        // Add the main mesh
        CombineInstance wallCombine = new CombineInstance();
        wallCombine.mesh = mainMesh;
        wallCombine.transform = Matrix4x4.identity;
        combine.Add(wallCombine);

        // Add each additional mesh
        foreach (Mesh mesh in otherMeshes)
        {
            CombineInstance capCombine = new CombineInstance();
            capCombine.mesh = mesh;
            capCombine.transform = Matrix4x4.identity;
            combine.Add(capCombine);
        }

        // Create the final combined mesh
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine.ToArray());

        // Replace the original mesh with the combined mesh
        mainMesh.Clear();
        mainMesh.vertices = combinedMesh.vertices;
        mainMesh.triangles = combinedMesh.triangles;
        mainMesh.normals = combinedMesh.normals;
        mainMesh.uv = combinedMesh.uv;
    }
}