using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Script : MonoBehaviour
{
    // Properties for the lamp base and its parameters
    public int size = 16;
    public float radius = 0.1f;  // Note: This radius is used for the main cylinder
    public float height = 0.01f; // Note: This height is used for the main cylinder
    public Material mat;

    private GameObject lampBase; // Reference to the lamp base object

    void Start()
    {
        // Create and set up the main lamp cylinder
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mat;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Cylinder(size, radius, height);

        // Create and set up the lamp base cylinder
        lampBase = new GameObject();
        lampBase.name = "Lamp Base";

        MeshRenderer lampBaseRenderer = lampBase.AddComponent<MeshRenderer>();
        lampBaseRenderer.sharedMaterial = mat;

        MeshFilter lampBaseFilter = lampBase.AddComponent<MeshFilter>();
        lampBaseFilter.mesh = MeshUtilities.Cylinder(16, 0.02f, 0.01f);

        // Set the lamp base as a child of the current object and adjust its position
        lampBase.transform.parent = transform;
        lampBase.transform.localPosition = new Vector3(0, 0.02f, 0);
    }

    void Update()
    {
        // Update logic can be added here if needed
    }
}
*/