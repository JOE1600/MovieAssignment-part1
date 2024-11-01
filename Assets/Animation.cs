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

    public AnimationCurve hipXCurveLeft, hipXCurveRight;
    public AnimationCurve kneeXCurveLeft, kneeXCurveRight;
    public AnimationCurve torsoYRotationCurve;

    // New curve for x-axis transition
    public AnimationCurve xPositionCurve;

    private Animation anim;

    void Awake()
    {
        InitializeCurves();
    }

    void Start()
    {
        anim = humanModel.gameObject.AddComponent<Animation>();
        AnimationClip clip = new AnimationClip { legacy = true };

        // Set curves for shoulder joints
        clip.SetCurve("Body/ShoulderJoint 1", typeof(Transform), "localEulerAngles.x", shoulderXCurve);
        clip.SetCurve("Body/ShoulderJoint 1", typeof(Transform), "localEulerAngles.y", shoulderYCurve);
        clip.SetCurve("Body/ShoulderJoint 1", typeof(Transform), "localEulerAngles.z", shoulderZCurve);
        clip.SetCurve("Body/ShoulderJoint 1/ElbowJoint 1", typeof(Transform), "localEulerAngles.x", elbowXCurve);
        clip.SetCurve("Body/ShoulderJoint 1/ElbowJoint 1", typeof(Transform), "localEulerAngles.y", elbowYCurve);
        clip.SetCurve("Body/ShoulderJoint 1/ElbowJoint 1", typeof(Transform), "localEulerAngles.z", elbowZCurve);

        clip.SetCurve("Body/ShoulderJoint 2", typeof(Transform), "localEulerAngles.x", shoulderXCurve);
        clip.SetCurve("Body/ShoulderJoint 2", typeof(Transform), "localEulerAngles.y", shoulderYCurve);
        clip.SetCurve("Body/ShoulderJoint 2", typeof(Transform), "localEulerAngles.z", shoulderZCurve);
        clip.SetCurve("Body/ShoulderJoint 2/ElbowJoint 2", typeof(Transform), "localEulerAngles.x", elbowXCurve);
        clip.SetCurve("Body/ShoulderJoint 2/ElbowJoint 2", typeof(Transform), "localEulerAngles.y", elbowYCurve);
        clip.SetCurve("Body/ShoulderJoint 2/ElbowJoint 2", typeof(Transform), "localEulerAngles.z", elbowZCurve);

        // Legs setup for walking motion with separate curves
        clip.SetCurve("Body/HipJoint 1", typeof(Transform), "localEulerAngles.x", hipXCurveLeft);
        clip.SetCurve("Body/HipJoint 1/KneeJoint 1", typeof(Transform), "localEulerAngles.x", kneeXCurveLeft);
        clip.SetCurve("Body/HipJoint 2", typeof(Transform), "localEulerAngles.x", hipXCurveRight);
        clip.SetCurve("Body/HipJoint 2/KneeJoint 2", typeof(Transform), "localEulerAngles.x", kneeXCurveRight);

        // Torso slight rotation for a more realistic walk
        clip.SetCurve("Body/Torso", typeof(Transform), "localEulerAngles.y", torsoYRotationCurve);

        // Set x-position curve for movement transition
        clip.SetCurve("", typeof(Transform), "localPosition.x", xPositionCurve);

        clip.wrapMode = WrapMode.Loop;
        anim.AddClip(clip, "walking_cycle");
        anim.Play("walking_cycle");
    }

    private void InitializeCurves()
    {
        shoulderXCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.25f, 30f), new Keyframe(0.5f, 60f), new Keyframe(0.75f, 30f), new Keyframe(1f, 0f));
        shoulderYCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.25f, 10f), new Keyframe(0.5f, 20f), new Keyframe(0.75f, 10f), new Keyframe(1f, 0f));
        shoulderZCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));

        elbowXCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.25f, 30f), new Keyframe(0.5f, 60f), new Keyframe(0.75f, 30f), new Keyframe(1f, 0f));
        elbowYCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.25f, 10f), new Keyframe(0.5f, 20f), new Keyframe(0.75f, 10f), new Keyframe(1f, 0f));
        elbowZCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));

        hipXCurveLeft = new AnimationCurve(new Keyframe(0f, 20f), new Keyframe(0.5f, -20f), new Keyframe(1f, 20f));
        kneeXCurveLeft = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 40f), new Keyframe(1f, 0f));

        hipXCurveRight = new AnimationCurve(new Keyframe(0f, -20f), new Keyframe(0.5f, 20f), new Keyframe(1f, -20f));
        kneeXCurveRight = new AnimationCurve(new Keyframe(0f, 40f), new Keyframe(0.5f, 0f), new Keyframe(1f, 40f));

        torsoYRotationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, -5f), new Keyframe(1f, 5f));

        // Define x-axis position transition curve
        xPositionCurve = new AnimationCurve(
            new Keyframe(0f, -1.14f),  // Start at x = -1.14
            new Keyframe(1f, -0.09f)   // End at x = -0.09
        );
    }

    void Update()
    {
        Debug.Log("Is playing walking_cycle: " + anim.IsPlaying("walking_cycle"));
    }
}
