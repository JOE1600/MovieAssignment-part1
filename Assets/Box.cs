using UnityEngine;

public class CubeCreator : MonoBehaviour
{
    public Vector3 cubePosition = Vector3.zero; // Position of the cube in the scene
    public Vector3 cubeScale = Vector3.one; // Scale of the cube (default is 1x1x1)
    public Material cubeMaterial; // Assign a material in the Inspector if desired

    void Start()
    {
        // Create a new cube primitive
        GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Position the cube in the scene
        cubeObject.transform.position = cubePosition;

        // Scale the cube
        cubeObject.transform.localScale = cubeScale;

        // Set the material if one is assigned
        if (cubeMaterial != null)
        {
            Renderer renderer = cubeObject.GetComponent<Renderer>();
            renderer.material = cubeMaterial;
        }
    }
}
