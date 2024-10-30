using UnityEngine;

public class HumanAnimation : MonoBehaviour
{
    public Transform human; // Reference to the HumanCreator transform
    private Animation anim; // Animation component

    void Start()
    {
        // Add Animation component
        anim = gameObject.AddComponent<Animation>();
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;

        // Animation for bending the knees
        Keyframe[] kneeXKeys = new Keyframe[5];
        kneeXKeys[0] = new Keyframe(0.0f, 0.0f);   // Start with knee straight at 0 degrees
        kneeXKeys[1] = new Keyframe(0.2f, -45.0f); // Bend knee to -45 degrees at t=0.2s (pre-jump)
        kneeXKeys[2] = new Keyframe(0.5f, 0.0f);   // Straight knee at peak jump (t=0.5s)
        kneeXKeys[3] = new Keyframe(0.8f, -45.0f); // Bend knee again at t=0.8s (landing preparation)
        kneeXKeys[4] = new Keyframe(1.0f, 0.0f);   // Return knee to straight position at t=1s

        AnimationCurve kneeXCurve = new AnimationCurve(kneeXKeys);
        clip.SetCurve("Legs/Thigh Joint/KneeJoint", typeof(Transform), "localEulerAngles.x", kneeXCurve);

        // Animation for bending the hip
        Keyframe[] hipXKeys = new Keyframe[5];
        hipXKeys[0] = new Keyframe(0.0f, 0.0f);   // Start with hip straight at 0 degrees
        hipXKeys[1] = new Keyframe(0.2f, -10.0f); // Bend hip slightly at t=0.2s (pre-jump)
        hipXKeys[2] = new Keyframe(0.5f, 0.0f);   // Straight hip at peak jump (t=0.5s)
        hipXKeys[3] = new Keyframe(0.8f, -10.0f); // Bend hip again at t=0.8s (landing preparation)
        hipXKeys[4] = new Keyframe(1.0f, 0.0f);   // Return hip to straight position at t=1s

        AnimationCurve hipXCurve = new AnimationCurve(hipXKeys);
        clip.SetCurve("Legs/Thigh Joint/HipJoint", typeof(Transform), "localEulerAngles.x", hipXCurve);

        // Animation for jumping (vertical movement)
        Keyframe[] jumpYKeys = new Keyframe[5];
        jumpYKeys[0] = new Keyframe(0.0f, 0.0f);   // Start at y = 0
        jumpYKeys[1] = new Keyframe(0.2f, 0.5f);   // Lower position (pre-jump) at t=0.2s
        jumpYKeys[2] = new Keyframe(0.5f, 2.0f);   // Jump up to y = 2 at t=0.5 seconds
        jumpYKeys[3] = new Keyframe(0.8f, 0.5f);   // Lower position (preparing to land) at t=0.8s
        jumpYKeys[4] = new Keyframe(1.0f, 0.0f);   // Fall back down to y = 0 at t=1 second

        AnimationCurve jumpYCurve = new AnimationCurve(jumpYKeys);
        clip.SetCurve("", typeof(Transform), "localPosition.y", jumpYCurve);

        // Set wrap mode to once to avoid continuous looping
        clip.wrapMode = WrapMode.Once;

        // Add and play the animation clip
        anim.AddClip(clip, "JumpAndFold");
        anim.Play("JumpAndFold");

        Debug.Log("Animation started!");
    }

    void Update()
    {
        // Ensure the human model's position and rotation are updated
        if (human != null)
        {
            transform.position = human.position;
            transform.rotation = human.rotation;
        }
        else
        {
            Debug.LogWarning("Human transform is not assigned!");
        }
    }
}
