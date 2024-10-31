using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimator : MonoBehaviour
{
    public Transform humanModel;

    public AnimationCurve shoulderXCurve;
    public AnimationCurve shoulderYCurve;
    public AnimationCurve shoulderZCurve;

    public AnimationCurve elbowXCurve;
    public AnimationCurve elbowYCurve;
    public AnimationCurve elbowZCurve;

    private Animation anim;

    void Awake()
    {
        // Initialize animation curves...
        InitializeCurves();
    }

    void Start()
    {
        anim = humanModel.gameObject.AddComponent<Animation>();
        AnimationClip clip = new AnimationClip { legacy = true };

        // Set the curves for ShoulderJoint 1 and ElbowJoint 1
        clip.SetCurve("Body/ShoulderJoint 1", typeof(Transform), "localEulerAngles.x", shoulderXCurve);
        clip.SetCurve("Body/ShoulderJoint 1", typeof(Transform), "localEulerAngles.y", shoulderYCurve);
        clip.SetCurve("Body/ShoulderJoint 1", typeof(Transform), "localEulerAngles.z", shoulderZCurve);
        clip.SetCurve("Body/ShoulderJoint 1/ElbowJoint 1", typeof(Transform), "localEulerAngles.x", elbowXCurve);
        clip.SetCurve("Body/ShoulderJoint 1/ElbowJoint 1", typeof(Transform), "localEulerAngles.y", elbowYCurve);
        clip.SetCurve("Body/ShoulderJoint 1/ElbowJoint 1", typeof(Transform), "localEulerAngles.z", elbowZCurve);

        // Set the curves for ShoulderJoint 2 and ElbowJoint 2
        clip.SetCurve("Body/ShoulderJoint 2", typeof(Transform), "localEulerAngles.x", shoulderXCurve);
        clip.SetCurve("Body/ShoulderJoint 2", typeof(Transform), "localEulerAngles.y", shoulderYCurve);
        clip.SetCurve("Body/ShoulderJoint 2", typeof(Transform), "localEulerAngles.z", shoulderZCurve);
        clip.SetCurve("Body/ShoulderJoint 2/ElbowJoint 2", typeof(Transform), "localEulerAngles.x", elbowXCurve);
        clip.SetCurve("Body/ShoulderJoint 2/ElbowJoint 2", typeof(Transform), "localEulerAngles.y", elbowYCurve);
        clip.SetCurve("Body/ShoulderJoint 2/ElbowJoint 2", typeof(Transform), "localEulerAngles.z", elbowZCurve);

        clip.wrapMode = WrapMode.Loop;

        anim.AddClip(clip, "arm_wave");
        anim.Play("arm_wave");
    }

    void Update()
    {
        // Debug log for animation playing state
        Debug.Log("Is playing: " + anim.IsPlaying("arm_wave"));
    }

    private void InitializeCurves()
    {
        // Shoulder X curve
        shoulderXCurve = new AnimationCurve();
        shoulderXCurve.AddKey(0f, 0f);
        shoulderXCurve.AddKey(0.25f, 30f);
        shoulderXCurve.AddKey(0.5f, 60f);
        shoulderXCurve.AddKey(0.75f, 30f);
        shoulderXCurve.AddKey(1f, 0f);

        shoulderYCurve = new AnimationCurve();
        shoulderYCurve.AddKey(0f, 0f);
        shoulderYCurve.AddKey(0.25f, 10f);
        shoulderYCurve.AddKey(0.5f, 20f);
        shoulderYCurve.AddKey(0.75f, 10f);
        shoulderYCurve.AddKey(1f, 0f);

        shoulderZCurve = new AnimationCurve();
        shoulderZCurve.AddKey(0f, 0f);
        shoulderZCurve.AddKey(1f, 0f);

        // Elbow X curve
        elbowXCurve = new AnimationCurve();
        elbowXCurve.AddKey(0f, 0f);
        elbowXCurve.AddKey(0.25f, 30f);
        elbowXCurve.AddKey(0.5f, 60f);
        elbowXCurve.AddKey(0.75f, 30f);
        elbowXCurve.AddKey(1f, 0f);

        elbowYCurve = new AnimationCurve();
        elbowYCurve.AddKey(0f, 0f);
        elbowYCurve.AddKey(0.25f, 10f);
        elbowYCurve.AddKey(0.5f, 20f);
        elbowYCurve.AddKey(0.75f, 10f);
        elbowYCurve.AddKey(1f, 0f);

        elbowZCurve = new AnimationCurve();
        elbowZCurve.AddKey(0f, 0f);
        elbowZCurve.AddKey(1f, 0f);
    }
}
