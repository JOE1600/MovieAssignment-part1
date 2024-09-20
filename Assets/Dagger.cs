using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerCreator : MonoBehaviour
{
    void Start()
    {
        // Define the profile for the grip of the dagger
        // This creates a 2D cross-sectional shape that will be swept along a 3D path
        Vector3[] gripProfile = new Vector3[] {
            new Vector3(0.0f, -0.05f, 0.0f),  // Bottom center
            new Vector3(0.02f, -0.025f, 0.0f), // Lower right edge
            new Vector3(0.03f, -0.015f, 0.0f), // Mid-right outer curve
            new Vector3(0.03f, 0.015f, 0.0f),  // Mid-right inner curve
            new Vector3(0.02f, 0.025f, 0.0f),  // Upper right edge
            new Vector3(0.0f, 0.05f, 0.0f),   // Top center
            new Vector3(-0.02f, 0.025f, 0.0f), // Upper left edge
            new Vector3(-0.03f, 0.015f, 0.0f), // Mid-left inner curve
            new Vector3(-0.03f, -0.015f, 0.0f), // Mid-left outer curve
            new Vector3(-0.02f, -0.025f, 0.0f) // Lower left edge
        };

        // Define the profile for the blade of the dagger, creating a cross-sectional shape for the blade
        Vector3[] bladeProfile = new Vector3[] {
            new Vector3(0.0f, -0.05f, 0.0f),  // Bottom center of blade
            new Vector3(0.02f, -0.05f, 0.0f), // Lower right edge
            new Vector3(0.03f, -0.03f, 0.0f), // Lower-right taper
            new Vector3(0.03f, 0.03f, 0.0f),  // Upper-right taper
            new Vector3(0.02f, 0.05f, 0.0f),  // Upper-right edge
            new Vector3(0.0f, 0.05f, 0.0f),   // Top center of blade
            new Vector3(-0.02f, 0.05f, 0.0f), // Upper-left edge
            new Vector3(-0.03f, 0.03f, 0.0f), // Upper-left taper
            new Vector3(-0.03f, -0.03f, 0.0f), // Lower-left taper
            new Vector3(-0.02f, -0.05f, 0.0f) // Lower-left edge
        };

        // Define the path for the grip (how the profile is extruded along the Z axis to give it 3D form)
        Matrix4x4[] gripPath = new Matrix4x4[6];
        // Narrow start at the base of the grip
        gripPath[0] = Matrix4x4.Scale(new Vector3(0, 0, 1)) *
                      Matrix4x4.Translate(new Vector3(0, 0, -0.1f));
        // Slightly wider in the middle part of the grip
        gripPath[1] = Matrix4x4.Scale(new Vector3(1.2f, 1.2f, 1)) *
                      Matrix4x4.Translate(new Vector3(0, 0, -0.1f));
        // Central section
        gripPath[2] = Matrix4x4.Translate(new Vector3(0, 0, -0.06f));
        gripPath[3] = Matrix4x4.Translate(new Vector3(0, 0, 0.06f));
        // Slightly wider towards the upper part of the grip
        gripPath[4] = Matrix4x4.Scale(new Vector3(1.2f, 1.2f, 1)) *
                      Matrix4x4.Translate(new Vector3(0, 0, 0.1f));
        // Tapering at the end of the grip
        gripPath[5] = Matrix4x4.Scale(new Vector3(0, 0, 1)) *
                      Matrix4x4.Translate(new Vector3(0, 0, 0.1f));

        // Define the path for the blade (how the blade profile is extruded along its length)
        Matrix4x4[] bladePath = new Matrix4x4[6];
        // Start of the blade, narrow at the base
        bladePath[0] = Matrix4x4.Scale(new Vector3(0, 0, 1)) *
                       Matrix4x4.Translate(new Vector3(0, 0, 0f)); // Start at base
        // Middle of the blade
        bladePath[1] = Matrix4x4.Scale(new Vector3(1.0f, 1.0f, 1)) *
                       Matrix4x4.Translate(new Vector3(0, 0, -0.2f));
        bladePath[2] = Matrix4x4.Translate(new Vector3(0, 0, -0.1f)); // Straight midsection
        bladePath[3] = Matrix4x4.Translate(new Vector3(0, 0, 0.1f));  // Another straight section
        // Middle section tapering off
        bladePath[4] = Matrix4x4.Scale(new Vector3(1.0f, 1.0f, 1)) *
                       Matrix4x4.Translate(new Vector3(0, 0, 0.2f));
        // Taper to the tip of the blade
        bladePath[5] = Matrix4x4.Scale(new Vector3(0, 0, 1)) *
                       Matrix4x4.Translate(new Vector3(0, 0, 0.2f));

        // Create the blade GameObject and assign the generated mesh
        GameObject blade = new GameObject("DaggerBlade");
        MeshRenderer bladeRenderer = blade.AddComponent<MeshRenderer>(); // Add renderer
        Material bladeMaterial = new Material(Shader.Find("Standard"));
        bladeMaterial.color = Color.gray; // Set blade color to gray
        bladeRenderer.sharedMaterial = bladeMaterial; // Assign material
        MeshFilter bladeFilter = blade.AddComponent<MeshFilter>(); // Add mesh filter
        Mesh bladeMesh = MeshUtilities.Sweep(bladeProfile, bladePath, false); // Create mesh from profile and path
        bladeFilter.mesh = bladeMesh; // Assign the mesh to the filter

        blade.transform.parent = this.transform; // Attach the blade to this object
        blade.transform.localPosition = new Vector3(0, 0, 0); // Set position of the blade
        blade.transform.localRotation = Quaternion.identity; // Set rotation of the blade

        // Create the grip GameObject and assign the generated mesh
        GameObject grip = new GameObject("DaggerGrip");
        MeshRenderer gripRenderer = grip.AddComponent<MeshRenderer>(); // Add renderer
        Material gripMaterial = new Material(Shader.Find("Standard"));
        gripMaterial.color = Color.red; // Set grip color to red
        gripRenderer.sharedMaterial = gripMaterial; // Assign material
        MeshFilter gripFilter = grip.AddComponent<MeshFilter>(); // Add mesh filter
        Mesh gripMesh = MeshUtilities.Sweep(gripProfile, gripPath, false); // Create mesh from profile and path
        gripFilter.mesh = gripMesh; // Assign the mesh to the filter

        // Attach the grip to the blade GameObject
        grip.transform.parent = blade.transform;
        grip.transform.localPosition = new Vector3(0, 0, -0.2f); // Set position relative to blade
        grip.transform.localRotation = Quaternion.identity; // Ensure no rotation offset
    }
}
