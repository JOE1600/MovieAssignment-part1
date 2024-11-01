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
    public GameObject[] hipJoints;
    public GameObject[] kneeJoints;
    public GameObject[] ankleJoints;
    private GameObject waist;
    private GameObject chest;
    private GameObject neck;

    // Scale factor to resize the entire model
    private float scaleFactor = 0.5f;

    void Start()
    {
        // Create Body
        float bodyRadius = 1.0f * scaleFactor;
        float bodyHeight = 1.5f * scaleFactor;
        body = new GameObject("Body");
        MeshFilter bodyMeshFilter = body.AddComponent<MeshFilter>();
        MeshRenderer bodyMeshRenderer = body.AddComponent<MeshRenderer>();
        bodyMeshRenderer.material = new Material(Shader.Find("Standard"));
        bodyMeshFilter.mesh = MeshUtilities.Cylinder(16, bodyRadius, bodyHeight);
        body.transform.parent = transform;
        body.transform.localPosition = Vector3.zero;

        // Create Chest
        CreateChest(bodyHeight);

        // Create Neck and Head
        CreateHeadAndNeck(bodyHeight);

        // Create Shoulder Joints and Arms
        CreateArmJoints(bodyHeight);

        // Create Legs with Hip, Knee, and Ankle Joints
        CreateLegJoints(bodyHeight);

        // Position the entire human model
        transform.position = new Vector3(-8.12f, 2.2f, 7.79f);




  

    }

    private void CreateChest(float bodyHeight)
    {
        float chestRad = 1.2f * scaleFactor;
        float chestLen = 1.8f * scaleFactor;
        chest = new GameObject("Chest");
        chest.transform.parent = body.transform;
        chest.transform.localPosition = new Vector3(0, bodyHeight / 2, 0);
        MeshFilter chestMeshFilter = chest.AddComponent<MeshFilter>();
        MeshRenderer chestMeshRenderer = chest.AddComponent<MeshRenderer>();
        chestMeshRenderer.material = new Material(Shader.Find("Standard"));
        chestMeshFilter.mesh = MeshUtilities.Cylinder(16, chestRad, chestLen);
    }

    private void CreateHeadAndNeck(float bodyHeight)
    {
        float neckRadius = 0.4f * scaleFactor;
        float neckHeight = 0.4f * scaleFactor;
        neck = new GameObject("Neck");
        neck.transform.parent = body.transform;
        neck.transform.localPosition = new Vector3(0, bodyHeight / 2 + neckHeight / 2, 0);
        MeshFilter neckMeshFilter = neck.AddComponent<MeshFilter>();
        MeshRenderer neckMeshRenderer = neck.AddComponent<MeshRenderer>();
        neckMeshRenderer.material = new Material(Shader.Find("Standard"));
        neckMeshFilter.mesh = MeshUtilities.Cylinder(16, neckRadius, neckHeight);

        float headRadius = 1f * scaleFactor;
        head = new GameObject("Head");
        MeshFilter headMeshFilter = head.AddComponent<MeshFilter>();
        MeshRenderer headMeshRenderer = head.AddComponent<MeshRenderer>();
        headMeshRenderer.material = new Material(Shader.Find("Standard"));
        headMeshFilter.mesh = MeshUtilities.CreateHead(headRadius);
        head.transform.parent = neck.transform;
        head.transform.localPosition = new Vector3(0, neckHeight + headRadius, 0);
    }

    private void CreateArmJoints(float bodyHeight)
    {
        arms = new GameObject[2];
        shoulderJoints = new GameObject[2];
        elbowJoints = new GameObject[2];

        for (int i = 0; i < 2; i++)
        {
            // Shoulder Joint
            shoulderJoints[i] = new GameObject("ShoulderJoint " + (i + 1));
            shoulderJoints[i].transform.parent = body.transform;
            shoulderJoints[i].transform.localPosition = new Vector3(i == 0 ? -1.5f * scaleFactor : 1.5f * scaleFactor, bodyHeight / 2, 0);

            // Arm Segment
            arms[i] = new GameObject("UpperArm " + (i + 1));
            MeshFilter armMeshFilter = arms[i].AddComponent<MeshFilter>();
            MeshRenderer armMeshRenderer = arms[i].AddComponent<MeshRenderer>();
            armMeshRenderer.material = new Material(Shader.Find("Standard"));
            armMeshFilter.mesh = MeshUtilities.Cylinder(16, 0.5f * scaleFactor, 2f * scaleFactor);
            arms[i].transform.parent = shoulderJoints[i].transform;
            arms[i].transform.localPosition = new Vector3(0, -1f * scaleFactor, 0);

            // Elbow Joint
            elbowJoints[i] = new GameObject("ElbowJoint " + (i + 1));
            elbowJoints[i].transform.parent = shoulderJoints[i].transform;
            elbowJoints[i].transform.localPosition = new Vector3(0, -2f * scaleFactor, 0);

            // Forearm
            GameObject forearm = new GameObject("Forearm " + (i + 1));
            MeshFilter forearmMeshFilter = forearm.AddComponent<MeshFilter>();
            MeshRenderer forearmMeshRenderer = forearm.AddComponent<MeshRenderer>();
            forearmMeshRenderer.material = new Material(Shader.Find("Standard"));
            forearmMeshFilter.mesh = MeshUtilities.Cylinder(16, 0.4f * scaleFactor, 1.5f * scaleFactor);
            forearm.transform.parent = elbowJoints[i].transform;
            forearm.transform.localPosition = new Vector3(0, -0.75f * scaleFactor, 0);
        }
    }

    private void CreateLegJoints(float bodyHeight)
    {
        legs = new GameObject[2];
        hipJoints = new GameObject[2];
        kneeJoints = new GameObject[2];
        ankleJoints = new GameObject[2];

        for (int i = 0; i < 2; i++)
        {
            // Hip Joint
            hipJoints[i] = new GameObject("HipJoint " + (i + 1));
            hipJoints[i].transform.parent = body.transform;
            hipJoints[i].transform.localPosition = new Vector3(i == 0 ? -0.5f * scaleFactor : 0.5f * scaleFactor, -bodyHeight / 2, 0);

            // Thigh
            legs[i] = new GameObject("Thigh " + (i + 1));
            MeshFilter legMeshFilter = legs[i].AddComponent<MeshFilter>();
            MeshRenderer legMeshRenderer = legs[i].AddComponent<MeshRenderer>();
            legMeshRenderer.material = new Material(Shader.Find("Standard"));
            legMeshFilter.mesh = MeshUtilities.Cylinder(16, 0.5f * scaleFactor, 1.5f * scaleFactor);
            legs[i].transform.parent = hipJoints[i].transform;
            legs[i].transform.localPosition = new Vector3(0, -0.75f * scaleFactor, 0);

            // Knee Joint
            kneeJoints[i] = new GameObject("KneeJoint " + (i + 1));
            kneeJoints[i].transform.parent = hipJoints[i].transform;
            kneeJoints[i].transform.localPosition = new Vector3(0, -1.5f * scaleFactor, 0);

            // Calf/Lower Leg
            GameObject calf = new GameObject("Calf " + (i + 1));
            MeshFilter calfMeshFilter = calf.AddComponent<MeshFilter>();
            MeshRenderer calfMeshRenderer = calf.AddComponent<MeshRenderer>();
            calfMeshRenderer.material = new Material(Shader.Find("Standard"));
            calfMeshFilter.mesh = MeshUtilities.Cylinder(16, 0.4f * scaleFactor, 1.5f * scaleFactor);
            calf.transform.parent = kneeJoints[i].transform;
            calf.transform.localPosition = new Vector3(0, -0.75f * scaleFactor, 0);

            // Ankle Joint
            ankleJoints[i] = new GameObject("AnkleJoint " + (i + 1));
            ankleJoints[i].transform.parent = kneeJoints[i].transform;
            ankleJoints[i].transform.localPosition = new Vector3(0, -1.5f * scaleFactor, 0);

            // Foot
            GameObject foot = new GameObject("Foot " + (i + 1));
            MeshFilter footMeshFilter = foot.AddComponent<MeshFilter>();
            MeshRenderer footMeshRenderer = foot.AddComponent<MeshRenderer>();
            footMeshRenderer.material = new Material(Shader.Find("Standard"));
            footMeshFilter.mesh = MeshUtilities.Cube(0.5f * scaleFactor);
            foot.transform.parent = ankleJoints[i].transform;
            foot.transform.localPosition = new Vector3(0, -0.25f * scaleFactor, 0);
        }
    }
}
