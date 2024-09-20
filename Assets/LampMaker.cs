using UnityEngine;

public class LampMaker : MonoBehaviour
{
    public GameObject lampbase;
    public GameObject arm1;
    public GameObject arm2;
    public GameObject lampShade;
    public GameObject baseJoint;
    public GameObject elbowJoint;
    public GameObject shadeJoint;
    public Material mat;
    private GameObject lampBase;

    // Properties for the lamp base and its parameters
    public int size = 16;
    public float radius = 0.1f;  // Note: This radius is used for the main cylinder
    public float height = 0.01f; // Note: This height is used for the main cylinder

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create and set up the main lamp cylinder
        MeshRenderer mainMeshRenderer = gameObject.AddComponent<MeshRenderer>();
        mainMeshRenderer.sharedMaterial = mat;

        MeshFilter mainMeshFilter = gameObject.AddComponent<MeshFilter>();
        mainMeshFilter.mesh = MeshUtilities.Cylinder(size, radius, height);

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

        // Create joints
        baseJoint = new GameObject("Base Joint");
        baseJoint.transform.parent = lampBase.transform;
        baseJoint.transform.localPosition = Vector3.zero;

        elbowJoint = new GameObject("Elbow Joint");
        elbowJoint.transform.parent = baseJoint.transform;
        elbowJoint.transform.localPosition = new Vector3(0, 0.2f, 0);
        elbowJoint.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 45));

        shadeJoint = new GameObject("Shade Joint");
        shadeJoint.transform.parent = elbowJoint.transform;
        shadeJoint.transform.localPosition = new Vector3(0, 0.2f, 0);
        shadeJoint.transform.localRotation = Quaternion.identity;

        // Create arm profiles and paths
        Vector3[] armProfile = new Vector3[]
        {
            new Vector3(0.0f, -0.12f, 0.0f),
            new Vector3(0.014f, -0.114f, 0.0f),
            new Vector3(0.02f, -0.1f, 0.0f),
            new Vector3(0.02f, 0.1f, 0.0f),
            new Vector3(0.014f, 0.114f, 0.0f),
            new Vector3(0.0f, 0.12f, 0.0f),
            new Vector3(-0.014f, 0.114f, 0.0f),
            new Vector3(-0.02f, 0.1f, 0.0f),
            new Vector3(-0.02f, -0.1f, 0.0f),
            new Vector3(-0.014f, -0.114f, 0.0f)
        };

        Matrix4x4[] armPath = new Matrix4x4[]
        {
            Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, -0.01f)),
            Matrix4x4.Scale(new Vector3(0.9f, 0.98f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, -0.01f)),
            Matrix4x4.Scale(new Vector3(0.9f, 0.98f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, -0.01f)),
            Matrix4x4.Translate(new Vector3(0, 0, -0.0075f)),
            Matrix4x4.Translate(new Vector3(0, 0, -0.0075f)),
            Matrix4x4.Translate(new Vector3(0, 0, 0.0075f)),
            Matrix4x4.Translate(new Vector3(0, 0, 0.0075f)),
            Matrix4x4.Scale(new Vector3(0.9f, 0.98f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.01f)),
            Matrix4x4.Scale(new Vector3(0.9f, 0.98f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.01f)),
            Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.01f))
        };

        // Create arm objects
        arm1 = new GameObject("Lamp Arm1");
        arm2 = new GameObject("Lamp Arm2");

        MeshRenderer arm1Renderer = arm1.AddComponent<MeshRenderer>();
        arm1Renderer.sharedMaterial = mat;
        MeshFilter arm1Filter = arm1.AddComponent<MeshFilter>();
        arm1Filter.mesh = MeshUtilities.Sweep(armProfile, armPath, false);

        arm1.transform.parent = lampBase.transform;
        arm1.transform.localPosition = new Vector3(0, 0.3f, 0);
        arm1.transform.parent = baseJoint.transform;
        arm1.transform.localPosition = new Vector3(0, 0.1f, 0);
        arm1.transform.localRotation = Quaternion.identity;

        // Create arm2 and set it up
        MeshRenderer arm2Renderer = arm2.AddComponent<MeshRenderer>();
        arm2Renderer.sharedMaterial = mat;
        MeshFilter arm2Filter = arm2.AddComponent<MeshFilter>();
        arm2Filter.mesh = MeshUtilities.Sweep(armProfile, armPath, false);

        arm2.transform.parent = elbowJoint.transform;
        arm2.transform.localPosition = new Vector3(0, 0.1f, 0);
        arm2.transform.localRotation = Quaternion.identity;

        // Define the lamp shade profile
        Vector3[] shadeProfile = new Vector3[]
        {
            // Outer edge of the shade
            new Vector3(-0.1f, 0.0f, 0.0f),
            new Vector3(-0.1f, 0.1f, 0.0f),
            new Vector3(0.1f, 0.1f, 0.0f),
            new Vector3(0.1f, 0.0f, 0.0f),
            // Inner edge of the shade (duplicated for sharp edges)
            new Vector3(-0.09f, 0.0f, 0.0f),
            new Vector3(-0.09f, 0.09f, 0.0f),
            new Vector3(0.09f, 0.09f, 0.0f),
            new Vector3(0.09f, 0.0f, 0.0f)
        };

        // Define the path
        Matrix4x4[] shadePath = MeshUtilities.MakeCirclePath(0.0f, 16);

        // Create the lamp shade
        lampShade = new GameObject("Lamp Shade");
        MeshRenderer shadeRenderer = lampShade.AddComponent<MeshRenderer>();
        shadeRenderer.sharedMaterial = mat;
        MeshFilter shadeFilter = lampShade.AddComponent<MeshFilter>();
        shadeFilter.mesh = MeshUtilities.Sweep(shadeProfile, shadePath, true);

        // Set the lamp shade's position and parent
        lampShade.transform.parent = shadeJoint.transform;
        lampShade.transform.localPosition = Vector3.zero;
        lampShade.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
