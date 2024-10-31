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

        // Set the curves using the actual hierarchy
        clip.SetCurve("Body/ShoulderJoint 1", typeof(Transform), "localEulerAngles.x", shoulderXCurve);
        clip.SetCurve("Body/ShoulderJoint 1", typeof(Transform), "localEulerAngles.y", shoulderYCurve);
        clip.SetCurve("Body/ShoulderJoint 1", typeof(Transform), "localEulerAngles.z", shoulderZCurve);

        // Elbow follows the shoulder's movements
        clip.SetCurve("Body/ShoulderJoint 1/ElbowJoint 1", typeof(Transform), "localEulerAngles.x", elbowXCurve);
        clip.SetCurve("Body/ShoulderJoint 1/ElbowJoint 1", typeof(Transform), "localEulerAngles.y", elbowYCurve);
        clip.SetCurve("Body/ShoulderJoint 1/ElbowJoint 1", typeof(Transform), "localEulerAngles.z", elbowZCurve);

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
        // Shoulder X curve: Increased range of motion for more visible wave
        shoulderXCurve = new AnimationCurve();
        shoulderXCurve.AddKey(0f, 0f);      // At time 0, angle 0
        shoulderXCurve.AddKey(0.25f, 30f);  // At 0.25 seconds, angle 30
        shoulderXCurve.AddKey(0.5f, 60f);   // At 0.5 seconds, angle 60
        shoulderXCurve.AddKey(0.75f, 30f);  // At 0.75 seconds, back to 30
        shoulderXCurve.AddKey(1f, 0f);      // Back to 0 at 1 second

        shoulderYCurve = new AnimationCurve();
        shoulderYCurve.AddKey(0f, 0f);
        shoulderYCurve.AddKey(0.25f, 10f);  // Increase Y movement as well
        shoulderYCurve.AddKey(0.5f, 20f);
        shoulderYCurve.AddKey(0.75f, 10f);
        shoulderYCurve.AddKey(1f, 0f);

        shoulderZCurve = new AnimationCurve();
        shoulderZCurve.AddKey(0f, 0f);
        shoulderZCurve.AddKey(1f, 0f); // Keep Z rotation steady

        // Elbow X curve: Mimics shoulder X movement
        elbowXCurve = new AnimationCurve();
        elbowXCurve.AddKey(0f, 0f);      // Start at 0
        elbowXCurve.AddKey(0.25f, 30f);  // At 0.25 seconds, angle matches shoulder
        elbowXCurve.AddKey(0.5f, 60f);   // At 0.5 seconds, angle matches shoulder
        elbowXCurve.AddKey(0.75f, 30f);  // At 0.75 seconds, back to shoulder's angle
        elbowXCurve.AddKey(1f, 0f);      // Back to 0 at 1 second

        // Elbow Y curve: Mimics shoulder Y movement
        elbowYCurve = new AnimationCurve();
        elbowYCurve.AddKey(0f, 0f);
        elbowYCurve.AddKey(0.25f, 10f);  // At 0.25 seconds, angle matches shoulder
        elbowYCurve.AddKey(0.5f, 20f);   // At 0.5 seconds, angle matches shoulder
        elbowYCurve.AddKey(0.75f, 10f);  // At 0.75 seconds, back to shoulder's angle
        elbowYCurve.AddKey(1f, 0f);      // Back to 0 at 1 second

        // Elbow Z curve: Keep Z steady for elbow
        elbowZCurve = new AnimationCurve();
        elbowZCurve.AddKey(0f, 0f);
        elbowZCurve.AddKey(1f, 0f); // Keep Z rotation steady for elbow
    }
}
