using System.Collections;
using UnityEngine;

public class DogCreator : MonoBehaviour
{
    // Declare all GameObject components for the dog's body parts
    private GameObject torso;
    private GameObject head;
    private GameObject neckJoint;
    private GameObject[] frontLegs;
    private GameObject[] backLegs;
    private GameObject[] shoulderJoints;
    private GameObject[] hipJoints;
    private GameObject[] kneeJoints;
    private GameObject[] ankleJoints;
    private GameObject tailBase;
    private GameObject tail;
    public GameObject dogPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Create the torso using Unity's built-in Capsule primitive
        // The torso will be the main body of the dog
        torso = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        torso.name = "Torso";  // Name the torso for clarity in the scene
        MeshRenderer torsoMeshRenderer = torso.GetComponent<MeshRenderer>();
        torsoMeshRenderer.material = new Material(Shader.Find("Standard")); // Use a basic material for the torso
        torso.transform.parent = transform;  // Set this object as the parent of the torso
        torso.transform.localScale = new Vector3(1.0f, 0.4f, 1.8f);  // Adjust the scale to fit a dog-like body shape
        torso.transform.localPosition = Vector3.zero;  // Set the torso at the origin of the parent

        // Create the neck joint to connect the head to the torso
        neckJoint = new GameObject("NeckJoint");
        neckJoint.transform.parent = torso.transform;  // Attach neck joint to the torso
        neckJoint.transform.localPosition = new Vector3(0, 0.61f, 0.487f);  // Position the neck joint to fit the head correctly

        // Create the head and attach it to the neck joint
        head = new GameObject("Head");
        MeshFilter headMeshFilter = head.AddComponent<MeshFilter>();  // Create mesh filter for the head
        MeshRenderer headMeshRenderer = head.AddComponent<MeshRenderer>();  // Create renderer for the head
        headMeshRenderer.material = new Material(Shader.Find("Standard"));  // Assign a basic material to the head
        headMeshFilter.mesh = MeshUtilities.CreateHead(0.45f);  // Use a utility method to create a custom head mesh
        head.transform.parent = neckJoint.transform;  // Attach the head to the neck joint
        head.transform.localPosition = new Vector3(0, 0.3f, 0);  // Position the head slightly above the neck joint

        // Create the front legs of the dog
        CreateFrontLegs();

        // Create the back legs of the dog
        CreateBackLegs();

        // Create the tail of the dog
        CreateTail();
    }

    // Method to create front legs
    private void CreateFrontLegs()
    {
        frontLegs = new GameObject[2];  // Array to store the front legs
        shoulderJoints = new GameObject[2];  // Array to store the shoulder joints
        kneeJoints = new GameObject[2];  // Array to store the knee joints
        ankleJoints = new GameObject[2];  // Array to store the ankle joints

        // Loop to create two front legs (left and right)
        for (int i = 0; i < 2; i++)
        {
            // Create the shoulder joint and attach it to the torso
            shoulderJoints[i] = new GameObject("ShoulderJoint " + (i + 1));
            shoulderJoints[i].transform.parent = torso.transform;
            shoulderJoints[i].transform.localPosition = new Vector3(i == 0 ? -0.362f : 0.4f, -0.256f, 0.393f);  // Position under the torso

            // Create the front thigh and attach it to the shoulder joint
            frontLegs[i] = new GameObject("FrontThigh " + (i + 1));
            MeshFilter frontThighMeshFilter = frontLegs[i].AddComponent<MeshFilter>();  // Add mesh filter
            MeshRenderer frontThighMeshRenderer = frontLegs[i].AddComponent<MeshRenderer>();  // Add renderer
            frontThighMeshRenderer.material = new Material(Shader.Find("Standard"));  // Assign a material
            frontThighMeshFilter.mesh = MeshUtilities.Cylinder(12, 0.3f, 0.85f);  // Create a cylindrical mesh for the thigh
            frontLegs[i].transform.parent = shoulderJoints[i].transform;  // Attach the thigh to the shoulder joint
            frontLegs[i].transform.localPosition = new Vector3(0, -0.55f, 0);  // Position the thigh

            // Create the knee joint and attach it to the front thigh
            kneeJoints[i] = new GameObject("KneeJoint " + (i + 1));
            kneeJoints[i].transform.parent = frontLegs[i].transform;
            kneeJoints[i].transform.localPosition = new Vector3(0, -0.85f, 0);  // Position the knee joint below the thigh

            // Create the front calf and attach it to the knee joint
            GameObject frontCalf = new GameObject("FrontCalf " + (i + 1));
            MeshFilter frontCalfMeshFilter = frontCalf.AddComponent<MeshFilter>();
            MeshRenderer frontCalfMeshRenderer = frontCalf.AddComponent<MeshRenderer>();
            frontCalfMeshRenderer.material = new Material(Shader.Find("Standard"));  // Assign material
            frontCalfMeshFilter.mesh = MeshUtilities.Cylinder(12, 0.22f, 0.65f);  // Create a cylindrical mesh for the calf
            frontCalf.transform.parent = kneeJoints[i].transform;
            frontCalf.transform.localPosition = new Vector3(0, -0.325f, 0);  // Position the calf below the knee joint

            // Create the ankle joint and attach it to the calf
            ankleJoints[i] = new GameObject("AnkleJoint " + (i + 1));
            ankleJoints[i].transform.parent = frontCalf.transform;
            ankleJoints[i].transform.localPosition = new Vector3(0, -0.65f, 0);  // Position the ankle joint

            // Create the front foot and attach it to the ankle joint
            GameObject frontFoot = new GameObject("FrontFoot " + (i + 1));
            MeshFilter frontFootMeshFilter = frontFoot.AddComponent<MeshFilter>();
            MeshRenderer frontFootMeshRenderer = frontFoot.AddComponent<MeshRenderer>();
            frontFootMeshRenderer.material = new Material(Shader.Find("Standard"));
            frontFootMeshFilter.mesh = MeshUtilities.Cylinder(12, 0.24f, 0.22f);  // Create cylindrical mesh for the foot
            frontFoot.transform.parent = ankleJoints[i].transform;
            frontFoot.transform.localPosition = Vector3.zero;  // Position the foot at the ankle joint
        }
    }

    // Method to create back legs
    private void CreateBackLegs()
    {
        backLegs = new GameObject[2];  // Array to store back legs
        hipJoints = new GameObject[2];  // Array to store hip joints
        kneeJoints = new GameObject[2];  // Array to store knee joints
        ankleJoints = new GameObject[2];  // Array to store ankle joints

        // Loop to create two back legs (left and right)
        for (int i = 0; i < 2; i++)
        {
            // Create the hip joint and attach it to the torso
            hipJoints[i] = new GameObject("HipJoint " + (i + 1));
            hipJoints[i].transform.parent = torso.transform;
            hipJoints[i].transform.localPosition = new Vector3(i == 0 ? -0.45f : 0.45f, -0.427f, -0.5f);  // Position the hip joint

            // Create the back thigh and attach it to the hip joint
            backLegs[i] = new GameObject("BackThigh " + (i + 1));
            MeshFilter backThighMeshFilter = backLegs[i].AddComponent<MeshFilter>();
            MeshRenderer backThighMeshRenderer = backLegs[i].AddComponent<MeshRenderer>();
            backThighMeshRenderer.material = new Material(Shader.Find("Standard"));
            backThighMeshFilter.mesh = MeshUtilities.Cylinder(12, 0.3f, 0.85f);  // Create a cylindrical mesh for the thigh
            backLegs[i].transform.parent = hipJoints[i].transform;
            backLegs[i].transform.localPosition = new Vector3(0, -0.425f, 0.0f);  // Position the thigh below the hip joint

            // Create the knee joint and attach it to the back thigh
            kneeJoints[i] = new GameObject("KneeJoint " + (i + 1));
            kneeJoints[i].transform.parent = backLegs[i].transform;
            kneeJoints[i].transform.localPosition = new Vector3(0, -0.85f, 0);  // Position the knee joint

            // Create the back calf and attach it to the knee joint
            GameObject backCalf = new GameObject("BackCalf " + (i + 1));
            MeshFilter backCalfMeshFilter = backCalf.AddComponent<MeshFilter>();
            MeshRenderer backCalfMeshRenderer = backCalf.AddComponent<MeshRenderer>();
            backCalfMeshRenderer.material = new Material(Shader.Find("Standard"));
            backCalfMeshFilter.mesh = MeshUtilities.Cylinder(12, 0.22f, 0.65f);  // Create cylindrical mesh for the calf
            backCalf.transform.parent = kneeJoints[i].transform;
            backCalf.transform.localPosition = new Vector3(0, -0.325f, 0);  // Position the calf below the knee joint

            // Create the ankle joint and attach it to the calf
            ankleJoints[i] = new GameObject("AnkleJoint " + (i + 1));
            ankleJoints[i].transform.parent = backCalf.transform;
            ankleJoints[i].transform.localPosition = new Vector3(0, -0.65f, 0);  // Position the ankle joint

            // Create the back foot and attach it to the ankle joint
            GameObject backFoot = new GameObject("BackFoot " + (i + 1));
            MeshFilter backFootMeshFilter = backFoot.AddComponent<MeshFilter>();
            MeshRenderer backFootMeshRenderer = backFoot.AddComponent<MeshRenderer>();
            backFootMeshRenderer.material = new Material(Shader.Find("Standard"));
            backFootMeshFilter.mesh = MeshUtilities.Cylinder(12, 0.24f, 0.22f);  // Create cylindrical mesh for the foot
            backFoot.transform.parent = ankleJoints[i].transform;
            backFoot.transform.localPosition = Vector3.zero;  // Position the foot
        }
    }

    // Method to create the dog's tail
    private void CreateTail()
    {
        // Create the base of the tail and attach it to the torso
        tailBase = new GameObject("TailBase");
        tailBase.transform.parent = torso.transform;
        tailBase.transform.localPosition = new Vector3(0, -0.2f, -0.462f);  // Position at the back of the torso

        // Create the tail and attach it to the tail base
        tail = new GameObject("Tail");
        MeshFilter tailMeshFilter = tail.AddComponent<MeshFilter>();
        MeshRenderer tailMeshRenderer = tail.AddComponent<MeshRenderer>();
        tailMeshRenderer.material = new Material(Shader.Find("Standard"));
        tailMeshFilter.mesh = CreateDogTail(0.16f, 0.05f, 1.0f);  // Create a tapered cylinder for the tail
        tail.transform.parent = tailBase.transform;
        tail.transform.localPosition = Vector3.zero;  // Position the tail at the base
    }

    // Helper method to create a tapered cylinder for the dog's tail
    private Mesh CreateDogTail(float baseRadius, float tipRadius, float length)
    {
        return MeshUtilities.TaperedCylinder(12, baseRadius, tipRadius, length);
    }
}
