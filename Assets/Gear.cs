using UnityEngine;

public class GearCreator : MonoBehaviour
{
    // Define parameters for the gear
    public float innerRadius = 0.5f;
    public float outerRadius = 1.0f;
    public float hubRadius = 0.3f;
    public int numTeeth = 12;
    public float toothWidth = 0.1f;
    public float depth = 0.2f;
    

    // Start is called before the first frame update
    void Start()
    {
        // Create the gear mesh using the MeshUtilities.Gear method
        Mesh gearMesh = MeshUtilities.Gear(innerRadius, outerRadius, hubRadius, numTeeth, toothWidth, depth);

        // Create a GameObject to display the gear in the scene
        GameObject gearObject = new GameObject("Gear");

        // Add a MeshFilter and MeshRenderer to the GameObject to display the mesh
        MeshFilter meshFilter = gearObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gearObject.AddComponent<MeshRenderer>();

        // Assign the gear mesh to the MeshFilter
        meshFilter.mesh = gearMesh;

        // Set a basic material for the MeshRenderer
        meshRenderer.material = new Material(Shader.Find("Standard"));

        // Position the gear in the scene (optional)
        gearObject.transform.position = Vector3.zero;  // Set at the center of the scene
        gearObject.transform.localScale = Vector3.one;  // Default scale
    }
}
