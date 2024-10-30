using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanCreator : MonoBehaviour
{
    private GameObject head;
    private GameObject body;
    public GameObject[] arms;
    public GameObject[] shoulderJoints;
    public GameObject[] elbowJoints;
    public GameObject[] legs;
    public GameObject[] kneeJoints;
    public GameObject[] ankleJoints;
    private GameObject waist;
    private GameObject chest;
    private GameObject neck;

    // Start is called before the first frame update
   void Start()
{
    // Create Body
    float bodyRadius = 1.5f;
    float bodyHeight = 2.5f;
    body = new GameObject("Body");
    MeshFilter bodyMeshFilter = body.AddComponent<MeshFilter>();
    MeshRenderer bodyMeshRenderer = body.AddComponent<MeshRenderer>(); // This is the line you're referring to
    bodyMeshRenderer.material = new Material(Shader.Find("Standard")); // This is correct
    bodyMeshFilter.mesh = MeshUtilities.Cylinder(16, bodyRadius, bodyHeight);
    body.transform.parent = transform;
    body.transform.localPosition = Vector3.zero;

    // Create Chest
    float chestRad = 1.2f;
    float chestLen = 1.8f;
    chest = new GameObject("Chest");
    chest.transform.parent = body.transform;
    chest.transform.localPosition = new Vector3(0, bodyHeight / 2, 0);
    MeshFilter chestMeshFilter = chest.AddComponent<MeshFilter>();
    MeshRenderer chestMeshRenderer = chest.AddComponent<MeshRenderer>();
    chestMeshRenderer.material = new Material(Shader.Find("Standard"));
    chestMeshFilter.mesh = CreateWaistMesh(chestRad, chestLen);

    // Create Neck and Head
    CreateHeadAndNeck(bodyHeight, chestLen);

    // Create Shoulder Joints and Arms
    CreateArms(bodyHeight);

    // Create Legs with Hip, Knee, and Ankle Joints
    CreateLegs(bodyHeight);

    // Position and scale the entire human model
    transform.position = new Vector3(-1.44f, 0.851f, 7.79f);
    transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // Adjust this scale factor to decrease the size further if needed
}


    // Method to create the Neck and Head
    private void CreateHeadAndNeck(float bodyHeight, float chestLen)
    {
        float neckRadius = 0.4f;
        float neckHeight = 0.4f;
        neck = new GameObject("Neck");
        neck.transform.parent = body.transform;
        neck.transform.localPosition = new Vector3(0, bodyHeight / 2 + chestLen / 2 + neckHeight / 2, 0);
        MeshFilter neckMeshFilter = neck.AddComponent<MeshFilter>();
        MeshRenderer neckMeshRenderer = neck.AddComponent<MeshRenderer>();
        neckMeshRenderer.material = new Material(Shader.Find("Standard"));
        neckMeshFilter.mesh = MeshUtilities.Cylinder(16, neckRadius, neckHeight);

        float headRadius = 1f;
        head = new GameObject("Head");
        MeshFilter headMeshFilter = head.AddComponent<MeshFilter>();
        MeshRenderer headMeshRenderer = head.AddComponent<MeshRenderer>();
        headMeshRenderer.material = new Material(Shader.Find("Standard"));
        headMeshFilter.mesh = MeshUtilities.CreateHead(headRadius);
        head.transform.parent = neck.transform;
        head.transform.localPosition = new Vector3(0, neckHeight + headRadius, 0);
    }

    // Method to create Arms with Shoulder and Elbow Joints
    private void CreateArms(float bodyHeight)
    {
        arms = new GameObject[2];
        shoulderJoints = new GameObject[2];
        elbowJoints = new GameObject[2];

        for (int i = 0; i < 2; i++)
        {
            // Shoulder Joint (Fixed Position, No Mesh)
            shoulderJoints[i] = new GameObject("ShoulderJoint " + (i + 1));
            shoulderJoints[i].transform.parent = body.transform;
            shoulderJoints[i].transform.localPosition = new Vector3(i == 0 ? -1.5f : 1.5f, bodyHeight / 2, 0);

            // Upper Arm (Mesh) - Offset below the shoulder joint
            arms[i] = new GameObject("UpperArm " + (i + 1));
            MeshFilter armMeshFilter = arms[i].AddComponent<MeshFilter>();
            MeshRenderer armMeshRenderer = arms[i].AddComponent<MeshRenderer>();
            armMeshRenderer.material = new Material(Shader.Find("Standard"));
            armMeshFilter.mesh = CreateUpperArmMesh(0.5f, 0.5f, 2f);
            arms[i].transform.parent = shoulderJoints[i].transform;
            arms[i].transform.localPosition = new Vector3(0, -2f / 2, 0);  // Offset so that the joint is fixed

            // Elbow Joint (No Mesh) - Positioned at the bottom of the upper arm
            elbowJoints[i] = new GameObject("ElbowJoint " + (i + 1));
            elbowJoints[i].transform.parent = arms[i].transform;
            elbowJoints[i].transform.localPosition = new Vector3(0, -2f, 0);  // Position fixed at the elbow

            // Forearm (Mesh) - Offset below the elbow joint
            GameObject forearm = new GameObject("Forearm " + (i + 1));
            MeshFilter forearmMeshFilter = forearm.AddComponent<MeshFilter>();
            MeshRenderer forearmMeshRenderer = forearm.AddComponent<MeshRenderer>();
            forearmMeshRenderer.material = new Material(Shader.Find("Standard"));
            forearmMeshFilter.mesh = MeshUtilities.Cylinder(16, 0.4f, 1.5f);
            forearm.transform.parent = elbowJoints[i].transform;
            forearm.transform.localPosition = new Vector3(0, -1.5f / 2, 0);  // Offset to ensure fixed rotation at the elbow

            // Make forearm non-selectable
            forearm.hideFlags = HideFlags.NotEditable;
        }
    }

 // Method to create Legs with Hip, Knee, and Ankle Joints
    private void CreateLegs(float bodyHeight)
    {
        legs = new GameObject[2];
        kneeJoints = new GameObject[2];
        ankleJoints = new GameObject[2];

        for (int i = 0; i < 2; i++)
        {
            // Hip Joint (Fixed Position, No Mesh)
            GameObject hipJoint = new GameObject("HipJoint " + (i + 1));
            hipJoint.transform.parent = body.transform;
            hipJoint.transform.localPosition = new Vector3(i == 0 ? -0.5f : 0.5f, 0, 0);

            // Thigh (Mesh) - Offset below the hip joint
            legs[i] = new GameObject("Thigh " + (i + 1));
            MeshFilter legMeshFilter = legs[i].AddComponent<MeshFilter>();
            MeshRenderer legMeshRenderer = legs[i].AddComponent<MeshRenderer>();
            legMeshRenderer.material = new Material(Shader.Find("Standard"));
            legMeshFilter.mesh = MeshUtilities.Cylinder(16, 0.5f, 1.5f);
            legs[i].transform.parent = hipJoint.transform;
            legs[i].transform.localPosition = new Vector3(0, -1.5f / 2, 0);  // Offset to ensure the joint remains fixed

            // Knee Joint (No Mesh) - Positioned at the bottom of the thigh
            kneeJoints[i] = new GameObject("KneeJoint " + (i + 1));
            kneeJoints[i].transform.parent = legs[i].transform;
            kneeJoints[i].transform.localPosition = new Vector3(0, -1.5f, 0);

            // Calf (Mesh) - Offset below the knee joint
            GameObject calf = new GameObject("Calf " + (i + 1));
            MeshFilter calfMeshFilter = calf.AddComponent<MeshFilter>();
            MeshRenderer calfMeshRenderer = calf.AddComponent<MeshRenderer>();
            calfMeshRenderer.material = new Material(Shader.Find("Standard"));
            calfMeshFilter.mesh = MeshUtilities.Cylinder(16, 0.3f, 1.2f);
            calf.transform.parent = kneeJoints[i].transform;
            calf.transform.localPosition = new Vector3(0, -1.2f / 2, 0);  // Offset so that the knee joint remains fixed

            // Ankle Joint (No Mesh) - Positioned at the bottom of the calf
            ankleJoints[i] = new GameObject("AnkleJoint " + (i + 1));
            ankleJoints[i].transform.parent = calf.transform;
            ankleJoints[i].transform.localPosition = new Vector3(0, -1.2f, 0);

            // Foot (Mesh) - Offset below the ankle joint
            GameObject foot = new GameObject("Foot " + (i + 1));
            MeshFilter footMeshFilter = foot.AddComponent<MeshFilter>();
            MeshRenderer footMeshRenderer = foot.AddComponent<MeshRenderer>();
            footMeshRenderer.material = new Material(Shader.Find("Standard"));
            footMeshFilter.mesh = MeshUtilities.Cylinder(16, 0.5f, 0.5f);
            foot.transform.parent = ankleJoints[i].transform;
            foot.transform.localPosition = new Vector3(0, -0.5f / 2, 0);  // Offset to ensure the ankle joint remains fixed
        }
    }


 // Method to create Waist Mesh
    private Mesh CreateWaistMesh(float radius, float height)
    {
        return MeshUtilities.Cylinder(16, radius, height);  // Using a cylinder for the waist
    }

    // Method to create Upper Arm Mesh
    public static Mesh CreateUpperArmMesh(float radius, float faceLen, float height)
    {
        return MeshUtilities.Cylinder(16, radius, height);  // Using a cylinder for the upper arm
    }




}
