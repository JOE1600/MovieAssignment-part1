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

    // Separate curves for each leg and torso movement
    public AnimationCurve hipXCurveLeft, hipXCurveRight;
    public AnimationCurve kneeXCurveLeft, kneeXCurveRight;
    public AnimationCurve torsoYRotationCurve;

    // New curves for position transitions
    public AnimationCurve xPositionCurve;
    public AnimationCurve yPositionCurve;
    public AnimationCurve zPositionCurve;

    private Animation anim;

    void Awake()
    {
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

        // Legs setup for walking motion with separate curves
        clip.SetCurve("Body/HipJoint 1", typeof(Transform), "localEulerAngles.x", hipXCurveLeft);
        clip.SetCurve("Body/HipJoint 1/KneeJoint 1", typeof(Transform), "localEulerAngles.x", kneeXCurveLeft);
        clip.SetCurve("Body/HipJoint 2", typeof(Transform), "localEulerAngles.x", hipXCurveRight);
        clip.SetCurve("Body/HipJoint 2/KneeJoint 2", typeof(Transform), "localEulerAngles.x", kneeXCurveRight);

        // Torso slight rotation for a more realistic walk
        clip.SetCurve("Body/Torso", typeof(Transform), "localEulerAngles.y", torsoYRotationCurve);

        // Set the position transition curves for moving the human model
        clip.SetCurve("", typeof(Transform), "localPosition.x", xPositionCurve);
        clip.SetCurve("", typeof(Transform), "localPosition.y", yPositionCurve);
        clip.SetCurve("", typeof(Transform), "localPosition.z", zPositionCurve);

        clip.wrapMode = WrapMode.Once;
        anim.AddClip(clip, "walking_cycle");

        // New: Create and set up a rotation animation for the human model
        AnimationClip rotationClip = new AnimationClip { legacy = true };
        rotationClip.SetCurve("", typeof(Transform), "localEulerAngles.y", new AnimationCurve(
            new Keyframe(0f, 0f),    // At time 0, Y rotation is 0 degrees
            new Keyframe(3f, 90f)    // At time 3, Y rotation is 90 degrees
        ));


        
        rotationClip.wrapMode = WrapMode.Once; // Play this animation only once
        anim.AddClip(rotationClip, "rotate_90");

        // New: Play the rotation animation before the walking cycle
        anim.Play("rotate_90");
        


        // After rotation, invoke walking cycle playback
        Invoke("PlayWalkingCycle", 5f); // Adjust the delay as needed (5 second here)



        AnimationClip turn180Clip = new AnimationClip { legacy = true };
        turn180Clip.SetCurve("", typeof(Transform), "localEulerAngles.y", new AnimationCurve(
        new Keyframe(0f, 90f),     // At time 0, Y rotation is 90 degrees
        new Keyframe(3f, 0f)    // At time 3, Y rotation is 0 degrees
    ));

    turn180Clip.wrapMode = WrapMode.Once; // Play this animation only once
    anim.AddClip(turn180Clip, "turn_180");


    StartCoroutine(CheckIfAtFinalPosition());


    

  


    }

    private void PlayWalkingCycle()
{
    anim.Play("walking_cycle");

    // Retrieve the final position after the walking cycle
    Vector3 finalPosition = GetFinalPosition();
    
    // Wait until the walking cycle is finished before turning
    Invoke("PlayTurnAnimation", 5f); // Adjust this delay to match the length of your walking animation

}


    private void InitializeCurves()
{
    shoulderXCurve = new AnimationCurve(
    new Keyframe(0f, 0f),
    new Keyframe(0.25f, 5f),      // Rise to 5 degrees at 0.25 seconds
    new Keyframe(0.5f, 0f),       // Return to 0 degrees at 0.5 seconds
    new Keyframe(0.75f, -5f),     // Drop to -5 degrees at 0.75 seconds
    new Keyframe(1f, 0f),         // Return to 0 degrees at 1 second
    new Keyframe(1.25f, 5f),      // Rise to 5 degrees at 1.25 seconds
    new Keyframe(1.5f, 0f),       // Return to 0 degrees at 1.5 seconds
    new Keyframe(1.75f, -5f),     // Drop to -5 degrees at 1.75 seconds
    new Keyframe(2f, 0f),         // Return to 0 degrees at 2 seconds
    
    new Keyframe(2.25f, 5f),
    new Keyframe(2.5f, 0f),
    new Keyframe(2.75f, -5f),
    new Keyframe(3f, 0f),
    new Keyframe(3.25f, 5f),
    new Keyframe(3.5f, 0f),
    new Keyframe(3.75f, -5f),
    new Keyframe(4f, 0f),
    new Keyframe(4.25f, 5f),
    new Keyframe(4.5f, 0f),
    new Keyframe(4.75f, -5f),
    new Keyframe(5f, 0f),
    new Keyframe(5.25f, 5f),
    new Keyframe(5.5f, 0f),
    new Keyframe(5.75f, -5f),
    new Keyframe(6f, 0f));


shoulderYCurve = new AnimationCurve(
    new Keyframe(0f, 0f),
    new Keyframe(0.25f, 2.5f),    // Rise to 2.5 degrees at 0.25 seconds
    new Keyframe(0.5f, 0f),        // Return to 0 degrees at 0.5 seconds
    new Keyframe(0.75f, -2.5f),    // Drop to -2.5 degrees at 0.75 seconds
    new Keyframe(1f, 0f),          // Return to 0 degrees at 1 second
    new Keyframe(1.25f, 2.5f),     // Rise to 2.5 degrees at 1.25 seconds
    new Keyframe(1.5f, 0f),        // Return to 0 degrees at 1.5 seconds
    new Keyframe(1.75f, -2.5f),    // Drop to -2.5 degrees at 1.75 seconds
    new Keyframe(2f, 0f),          // Return to 0 degrees at 2 seconds
  
    new Keyframe(2.25f, 2.5f),
    new Keyframe(2.5f, 0f),
    new Keyframe(2.75f, -2.5f),
    new Keyframe(3f, 0f),
    new Keyframe(3.25f, 2.5f),
    new Keyframe(3.5f, 0f),
    new Keyframe(3.75f, -2.5f),
    new Keyframe(4f, 0f),
    new Keyframe(4.25f, 2.5f),
    new Keyframe(4.5f, 0f),
    new Keyframe(4.75f, -2.5f),
    new Keyframe(5f, 0f),
    new Keyframe(5.25f, 2.5f),
    new Keyframe(5.5f, 0f),
    new Keyframe(5.75f, -2.5f),
    new Keyframe(6f, 0f));
    

shoulderZCurve = new AnimationCurve(
    new Keyframe(0f, 0f), 
    new Keyframe(6f, 0f));          // Hold at 0 degrees for the entire duration

elbowXCurve = new AnimationCurve(
    new Keyframe(0f, 0f), 
    new Keyframe(0.25f, 2.5f),   // Slight rise at 0.25 seconds
    new Keyframe(0.5f, 5f),       // Bend to 5 degrees at 0.5 seconds
    new Keyframe(0.75f, 7.5f),    // Bend to 7.5 degrees at 0.75 seconds
    new Keyframe(1f, 10f),        // Bend to 10 degrees at 1 second
    new Keyframe(1.25f, 20f),     // Bend to 20 degrees at 1.25 seconds
    new Keyframe(1.5f, 30f),      // Bend to 30 degrees at 1.5 seconds
    new Keyframe(1.75f, 40f),     // Bend to 40 degrees at 1.75 seconds
    new Keyframe(2f, 50f),        // Bend to 50 degrees at 2 seconds
    new Keyframe(2.25f, 55f),     // Bend to 55 degrees at 2.25 seconds
    new Keyframe(2.5f, 60f),      // Full bend at 2.5 seconds
    new Keyframe(2.75f, 55f),     // Drop back to 55 degrees at 2.75 seconds
    new Keyframe(3f, 50f),        // Drop to 50 degrees at 3 seconds
    new Keyframe(3.25f, 40f),     // Drop to 40 degrees at 3.25 seconds
    new Keyframe(3.5f, 30f),      // Drop to 30 degrees at 3.5 seconds
    new Keyframe(3.75f, 20f),     // Drop to 20 degrees at 3.75 seconds
    new Keyframe(4f, 10f),        // Drop to 10 degrees at 4 seconds
    new Keyframe(4.25f, 5f),      // Drop to 5 degrees at 4.25 seconds
    new Keyframe(4.5f, 0f),       // Return to 0 degrees at 4.5 seconds
    new Keyframe(4.75f, 0f),      // Hold at 0 degrees at 4.75 seconds
    new Keyframe(5f, 0f),         // Hold at 0 degrees at 5 seconds
    new Keyframe(5.25f, 0f),      // Hold at 0 degrees at 5.25 seconds
    new Keyframe(5.5f, 0f),       // Hold at 0 degrees at 5.5 seconds
    new Keyframe(5.75f, 0f),      // Hold at 0 degrees at 5.75 seconds
    new Keyframe(6f, 0f));        // Return to 0 degrees at 6 seconds

elbowYCurve = new AnimationCurve(
    new Keyframe(0f, 0f), 
    new Keyframe(0.25f, 1.25f),   // Slight rise at 0.25 seconds
    new Keyframe(0.5f, 2.5f),      // Bend to 2.5 degrees at 0.5 seconds
    new Keyframe(0.75f, 3.75f),    // Bend to 3.75 degrees at 0.75 seconds
    new Keyframe(1f, 5f),          // Bend to 5 degrees at 1 second
    new Keyframe(1.25f, 10f),      // Bend to 10 degrees at 1.25 seconds
    new Keyframe(1.5f, 15f),       // Bend to 15 degrees at 1.5 seconds
    new Keyframe(1.75f, 10f),      // Drop to 10 degrees at 1.75 seconds
    new Keyframe(2f, 5f),          // Drop to 5 degrees at 2 seconds
    new Keyframe(2.25f, 3f),       // Drop to 3 degrees at 2.25 seconds
    new Keyframe(2.5f, 2f),        // Drop to 2 degrees at 2.5 seconds
    new Keyframe(2.75f, 1f),       // Drop to 1 degree at 2.75 seconds
    new Keyframe(3f, 0f),          // Return to 0 degrees at 3 seconds
    new Keyframe(3.25f, 0f),       // Hold at 0 degrees at 3.25 seconds
    new Keyframe(3.5f, 0f),        // Hold at 0 degrees at 3.5 seconds
    new Keyframe(3.75f, 0f),       // Hold at 0 degrees at 3.75 seconds
    new Keyframe(4f, 0f),          // Hold at 0 degrees at 4 seconds
    new Keyframe(4.25f, 0f),       // Hold at 0 degrees at 4.25 seconds
    new Keyframe(4.5f, 0f),        // Hold at 0 degrees at 4.5 seconds
    new Keyframe(4.75f, 0f),       // Hold at 0 degrees at 4.75 seconds
    new Keyframe(5f, 0f),          // Hold at 0 degrees at 5 seconds
    new Keyframe(5.25f, 0f),       // Hold at 0 degrees at 5.25 seconds
    new Keyframe(5.5f, 0f),        // Hold at 0 degrees at 5.5 seconds
    new Keyframe(5.75f, 0f),       // Hold at 0 degrees at 5.75 seconds
    new Keyframe(6f, 0f));         // Return to 0 degrees at 6 seconds

elbowZCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(6f, 0f));


// Left hip (forward and backward motion)
hipXCurveLeft = new AnimationCurve(
    new Keyframe(0f, 0f), 
    new Keyframe(0.25f, 5f),   // Slight rise at 0.25 seconds
    new Keyframe(0.5f, 10f),    // Move forward to 10 degrees at 0.5 seconds
    new Keyframe(0.75f, 15f),   // Move to 15 degrees at 0.75 seconds
    new Keyframe(1f, 20f),      // Move to 20 degrees at 1 second
    new Keyframe(1.25f, 15f),   // Move back to 15 degrees at 1.25 seconds
    new Keyframe(1.5f, 10f),    // Move back to 10 degrees at 1.5 seconds
    new Keyframe(1.75f, 5f),    // Move back to 5 degrees at 1.75 seconds
    new Keyframe(2f, 0f),       // Return to neutral position at 2 seconds
    new Keyframe(2.25f, -5f),   // Move back to -5 degrees at 2.25 seconds
    new Keyframe(2.5f, -10f),   // Move back to -10 degrees at 2.5 seconds
    new Keyframe(2.75f, -15f),  // Move back to -15 degrees at 2.75 seconds
    new Keyframe(3f, -20f),     // Move back to -20 degrees at 3 seconds
    new Keyframe(3.25f, -15f),  // Move back to -15 degrees at 3.25 seconds
    new Keyframe(3.5f, -10f),   // Move back to -10 degrees at 3.5 seconds
    new Keyframe(3.75f, -5f),   // Move back to -5 degrees at 3.75 seconds
    new Keyframe(4f, 0f),       // Return to neutral position at 4 seconds
    new Keyframe(4.25f, 5f),    // Move back to 5 degrees at 4.25 seconds
    new Keyframe(4.5f, 10f),    // Move forward to 10 degrees at 4.5 seconds
    new Keyframe(4.75f, 15f),   // Move to 15 degrees at 4.75 seconds
    new Keyframe(5f, 20f),      // Move to 20 degrees at 5 seconds
    new Keyframe(5.25f, 15f),   // Move back to 15 degrees at 5.25 seconds
    new Keyframe(5.5f, 10f),    // Move back to 10 degrees at 5.5 seconds
    new Keyframe(5.75f, 5f),    // Move back to 5 degrees at 5.75 seconds
    new Keyframe(6f, 0f));      // Return to 0 degrees at 6 seconds

// Right hip (opposite of left hip)
hipXCurveRight = new AnimationCurve(
    new Keyframe(0f, 0f), 
    new Keyframe(0.25f, -5f),  // Slight drop at 0.25 seconds
    new Keyframe(0.5f, -10f),   // Move backward to -10 degrees at 0.5 seconds
    new Keyframe(0.75f, -15f),  // Move to -15 degrees at 0.75 seconds
    new Keyframe(1f, -20f),     // Move to -20 degrees at 1 second
    new Keyframe(1.25f, -15f),  // Move back to -15 degrees at 1.25 seconds
    new Keyframe(1.5f, -10f),   // Move back to -10 degrees at 1.5 seconds
    new Keyframe(1.75f, -5f),    // Move back to -5 degrees at 1.75 seconds
    new Keyframe(2f, 0f),       // Return to neutral position at 2 seconds
    new Keyframe(2.25f, 5f),    // Move forward to 5 degrees at 2.25 seconds
    new Keyframe(2.5f, 10f),    // Move forward to 10 degrees at 2.5 seconds
    new Keyframe(2.75f, 15f),   // Move to 15 degrees at 2.75 seconds
    new Keyframe(3f, 20f),      // Move to 20 degrees at 3 seconds
    new Keyframe(3.25f, 15f),   // Move back to 15 degrees at 3.25 seconds
    new Keyframe(3.5f, 10f),    // Move back to 10 degrees at 3.5 seconds
    new Keyframe(3.75f, 5f),    // Move back to 5 degrees at 3.75 seconds
    new Keyframe(4f, 0f),       // Return to neutral position at 4 seconds
    new Keyframe(4.25f, -5f),   // Move back to -5 degrees at 4.25 seconds
    new Keyframe(4.5f, -10f),   // Move backward to -10 degrees at 4.5 seconds
    new Keyframe(4.75f, -15f),  // Move to -15 degrees at 4.75 seconds
    new Keyframe(5f, -20f),     // Move to -20 degrees at 5 seconds
    new Keyframe(5.25f, -15f),  // Move back to -15 degrees at 5.25 seconds
    new Keyframe(5.5f, -10f),   // Move back to -10 degrees at 5.5 seconds
    new Keyframe(5.75f, -5f),   // Move back to -5 degrees at 5.75 seconds
    new Keyframe(6f, 0f));      // Return to 0 degrees at 6 seconds



// Left knee (bending motion)
kneeXCurveLeft = new AnimationCurve(
    new Keyframe(0f, 0f), 
    new Keyframe(0.25f, 15f),  // Bend at 0.25 seconds
    new Keyframe(0.5f, 30f),   // Bend to 30 degrees at 0.5 seconds
    new Keyframe(0.75f, 45f),  // Bend to 45 degrees at 0.75 seconds
    new Keyframe(1f, 60f),     // Full bend at 1 second
    new Keyframe(1.25f, 45f),  // Straighten to 45 degrees at 1.25 seconds
    new Keyframe(1.5f, 30f),   // Straighten to 30 degrees at 1.5 seconds
    new Keyframe(1.75f, 15f),  // Straighten to 15 degrees at 1.75 seconds
    new Keyframe(2f, 0f),      // Return to neutral position at 2 seconds
    new Keyframe(2.25f, 15f),  // Bend to 15 degrees at 2.25 seconds
    new Keyframe(2.5f, 30f),   // Bend to 30 degrees at 2.5 seconds
    new Keyframe(2.75f, 45f),  // Bend to 45 degrees at 2.75 seconds
    new Keyframe(3f, 60f),     // Full bend at 3 seconds
    new Keyframe(3.25f, 45f),  // Straighten to 45 degrees at 3.25 seconds
    new Keyframe(3.5f, 30f),   // Straighten to 30 degrees at 3.5 seconds
    new Keyframe(3.75f, 15f),  // Straighten to 15 degrees at 3.75 seconds
    new Keyframe(4f, 0f),      // Return to neutral position at 4 seconds
    new Keyframe(4.25f, 15f),  // Bend to 15 degrees at 4.25 seconds
    new Keyframe(4.5f, 30f),   // Bend to 30 degrees at 4.5 seconds
    new Keyframe(4.75f, 45f),  // Bend to 45 degrees at 4.75 seconds
    new Keyframe(5f, 60f),     // Full bend at 5 seconds
    new Keyframe(5.25f, 45f),  // Straighten to 45 degrees at 5.25 seconds
    new Keyframe(5.5f, 30f),   // Straighten to 30 degrees at 5.5 seconds
    new Keyframe(5.75f, 15f),  // Straighten to 15 degrees at 5.75 seconds
    new Keyframe(6f, 0f));     // Return to 0 degrees at 6 seconds

// Right knee (opposite of left knee)
kneeXCurveRight = new AnimationCurve(
    new Keyframe(0f, 0f), 
    new Keyframe(0.25f, -15f), // Bend at 0.25 seconds (opposite direction)
    new Keyframe(0.5f, -30f),   // Bend to -30 degrees at 0.5 seconds
    new Keyframe(0.75f, -45f),  // Bend to -45 degrees at 0.75 seconds
    new Keyframe(1f, -60f),     // Full bend at 1 second
    new Keyframe(1.25f, -45f),  // Straighten to -45 degrees at 1.25 seconds
    new Keyframe(1.5f, -30f),   // Straighten to -30 degrees at 1.5 seconds
    new Keyframe(1.75f, -15f),  // Straighten to -15 degrees at 1.75 seconds
    new Keyframe(2f, 0f),       // Return to neutral position at 2 seconds
    new Keyframe(2.25f, -15f),  // Bend to -15 degrees at 2.25 seconds
    new Keyframe(2.5f, -30f),   // Bend to -30 degrees at 2.5 seconds
    new Keyframe(2.75f, -45f),  // Bend to -45 degrees at 2.75 seconds
    new Keyframe(3f, -60f),     // Full bend at 3 seconds
    new Keyframe(3.25f, -45f),  // Straighten to -45 degrees at 3.25 seconds
    new Keyframe(3.5f, -30f),   // Straighten to -30 degrees at 3.5 seconds
    new Keyframe(3.75f, -15f),  // Straighten to -15 degrees at 3.75 seconds
    new Keyframe(4f, 0f),       // Return to neutral position at 4 seconds
    new Keyframe(4.25f, -15f),  // Bend to -15 degrees at 4.25 seconds
    new Keyframe(4.5f, -30f),   // Bend to -30 degrees at 4.5 seconds
    new Keyframe(4.75f, -45f),  // Bend to -45 degrees at 4.75 seconds
    new Keyframe(5f, -60f),     // Full bend at 5 seconds
    new Keyframe(5.25f, -45f),  // Straighten to -45 degrees at 5.25 seconds
    new Keyframe(5.5f, -30f),   // Straighten to -30 degrees at 5.5 seconds
    new Keyframe(5.75f, -15f),  // Straighten to -15 degrees at 5.75 seconds
    new Keyframe(6f, 0f));      // Return to 0 degrees at 6 seconds



        // Torso rotation for subtle side-to-side movement
        torsoYRotationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, -5f), new Keyframe(1f, 5f));

        // Define x-axis position transition curve from (-8.12, 2.2, 7.79) to (0.02, 2.2, 7.79)
        xPositionCurve = new AnimationCurve(
            new Keyframe(0f, -8.12f),  // Start position on the x-axis
            new Keyframe(8f, 0.02f)   // End position on the x-axis
        );

        // Add keyframes for constant y and z positions
        yPositionCurve = new AnimationCurve(
            new Keyframe(0f, 2.2f),    // Constant y position
            new Keyframe(8f, 2.2f)     // Constant y position
        );

        zPositionCurve = new AnimationCurve(
            new Keyframe(0f, 7.79f),   // Constant z position
            new Keyframe(8f, 7.79f)    // Constant z position
        );
    }




private Vector3 GetFinalPosition()
{
    float finalX = xPositionCurve.keys[xPositionCurve.length - 1].value;
    float finalY = yPositionCurve.keys[yPositionCurve.length - 1].value;
    float finalZ = zPositionCurve.keys[zPositionCurve.length - 1].value;

    return new Vector3(finalX, finalY, finalZ);
}



// Coroutine to check if the human has reached the final position
private IEnumerator CheckIfAtFinalPosition()
{
    Vector3 finalPosition = GetFinalPosition();
    
    // Wait until the human model reaches the final position
    while (Vector3.Distance(humanModel.localPosition, finalPosition) > 0.1f)
    {
        yield return null; // Wait until next frame
    }

    // Play the 180-degree turn animation
    anim.Play("turn_180");

    Debug.Log("Reached Final Position. Resting...");
    yield return new WaitForSeconds(3f); // Rest for 3 seconds

    // Move to the new position at z = 3.1 while keeping x and y constant
    Vector3 currentPosition = humanModel.localPosition; // Current position
    Vector3 newTargetPosition = new Vector3(currentPosition.x, currentPosition.y, 3.1f); // New target position

    // Move the model to the new target position
    float moveDuration = 2f; // Duration to reach the target position (increase for slower movement)
    float elapsedTime = 0f;

    while (elapsedTime < moveDuration)
    {
        // Interpolate the position smoothly over time
        humanModel.localPosition = Vector3.Lerp(currentPosition, newTargetPosition, elapsedTime / moveDuration);
        elapsedTime += Time.deltaTime;
        yield return null; // Wait for the next frame
    }

    // Ensure the final position is set correctly at the end of the transition
    humanModel.localPosition = newTargetPosition;
    Debug.Log($"Moved to New Position: {humanModel.localPosition}");
}


private bool IsAtFinalPosition(Vector3 targetPosition)
{
    float distance = Vector3.Distance(humanModel.localPosition, targetPosition);
    Debug.Log($"Distance to Target Position: {distance}"); // Log the distance
    return distance < 0.1f; // You may adjust this threshold as needed
}










}
