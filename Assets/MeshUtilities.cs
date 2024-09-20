using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MeshUtilities
{




public static Mesh Pyramid(float size)
    {
        Mesh mesh = new Mesh();

        // 4 x 3 for faces + 4 for the bottom
        Vector3[] vertices = new Vector3[4 * 3 + 4]
        {
            //front
		    new Vector3(-size,-size,-size), //0
            new Vector3(size, -size, -size), //1
            new Vector3(0, size, 0), //2
            
            // back
            new Vector3(size, -size, size), //3
            new Vector3(-size, -size, size), //4
            new Vector3(0, size, 0), //5

            
            // left
            new Vector3(-size,-size,-size), //6
            new Vector3(-size, -size, size), //7
            new Vector3(0, size, 0), //8

            
            // right
            new Vector3(size, -size, -size), //9
            new Vector3(size, -size, size), //10
            new Vector3(0, size, 0), //11

            
            // bottom
            new Vector3(-size, -size, -size), //12
            new Vector3(size, -size, -size), //13
            new Vector3(size, -size, size), //14
            new Vector3(-size, -size, size) //15

        };
        mesh.vertices = vertices;

        // 4 * 3 for faces + 2 * 3 for the bottom
        int[] tris = new int[6 * 3]
        {
            //front
            0,2,1,

            // back
            3,5,4,

            // left
            6,7,8,

            // right
            9,11,10,

            // bottom
            12,13,14,
            14,15,12
        };
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        return mesh;
    }


	public static Mesh Cube(float size)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * 6]
        {
            //front
		    new Vector3(-size,-size,-size),
            new Vector3(size, -size, -size),
            new Vector3(size, size, -size),
            new Vector3(-size, size, -size),
            
            // back
            new Vector3(-size, -size, size),
            new Vector3(size, -size, size),
            new Vector3(size, size, size),
            new Vector3(-size, size, size),
            
            // left
            new Vector3(-size, -size, -size),
            new Vector3(-size, size, -size),
            new Vector3(-size, size, size),
            new Vector3(-size, -size, size),
            
            // right
            new Vector3(size, -size, -size),
            new Vector3(size, size, -size),
            new Vector3(size, size, size),
            new Vector3(size, -size, size),
            
            // bottom
            new Vector3(-size, -size, -size),
            new Vector3(-size, -size, size),
            new Vector3(size, -size, size),
            new Vector3(size, -size, -size),
            
            // top
            new Vector3(-size, size, -size),
            new Vector3(-size, size, size),
            new Vector3(size, size, size),
            new Vector3(size, size, -size)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6 * 2 * 3]
        {
            //front
            3, 2, 1,
            3, 1, 0,

            // back
            4,5,6,
            4,6,7,

            // left
            11,10,9,
            11,9,8,

            // right
            12,13,14,
            12,14,15,

            // bottom
            19,18,17,
            19,17,16,

            // top
            20,21,22,
            20,22,23
        };
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        return mesh;
    }

    
 public static Mesh Cylinder(int d, float r, float h)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * d + 2]; // 4 d: (top, top copy, bottom, bottom copy) + 2 center vertices
        float dTheta = Mathf.PI * 2.0f / d;

        int topstart = 0; // start position of top vertices. will have d of these
        int botstart = d; // start pos of bottom vertices, will have d of these
        int topstart2 = 2 * d; // copy of vertices for caps, will have d of these
        int botstart2 = 3 * d; // copy of vertices for caps, will have d of these
        int topcent = 4 * d; // center top vertex, only 1
        int botcent = 4 * d + 1; // center bottom vertex, only 1

        for (int i = 0; i < d; i++)
        {
            float theta = i * dTheta;
            float x = r * Mathf.Cos(theta);
            float z = r * Mathf.Sin(theta);
            // top vertex
            vertices[topstart + i] = new Vector4(x, h, z);
            // bottom vertex
            vertices[botstart + i] = new Vector4(x, -h, z);

            //remake verticies for caps
            vertices[topstart2 + i] = new Vector4(x, h, z);
            vertices[botstart2 + i] = new Vector4(x, -h, z);
        }

        vertices[topcent] = new Vector4(0, h, 0);
        vertices[botcent] = new Vector4(0, -h, 0);

        mesh.vertices = vertices;

        int[] tris = new int[(d * 6) + (3 * d * 2)]; // two tris for each side
        int pos = 0;
        for (int i = 0; i < d; i++)
        {
            // LINK UP THE FACES: 2 triangles for each face
            tris[pos++] = topstart + i;                        // current top vertex
            tris[pos++] = topstart + (i + 1) % d;          // next top vertex (wrapping)
            tris[pos++] = botstart + (i + 1) % d;      // next bottom vertex (wrapping)

            tris[pos++] = topstart + i;                    // current top vertex
            tris[pos++] = botstart + (i + 1) % d;      // next bottom vertex (wrapping)
            tris[pos++] = botstart + i;              // current bottom vertex

            // LINK UP THE CAPS
            tris[pos++] = topcent; // center top vertex
            tris[pos++] = topstart2 + (i + 1) % d; // next top vertex (wrapping)
            tris[pos++] = topstart2 + i; // current top vertex

            tris[pos++] = botstart2 + i; // current bottom vertex
            tris[pos++] = botstart2 + (i + 1) % d; // next bottom vertex (wrapping)
            tris[pos++] = botcent; // center bottom vertex
        }

        mesh.triangles = tris;

        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh Sweep(Vector3[] profile, Matrix4x4[] path, bool closed)
    {
		Mesh mesh = new Mesh();

		int numVerts = path.Length * profile.Length;
		int numTris;

		if (closed)
			numTris = 2 * path.Length * profile.Length;
		else
			numTris = 2 * (path.Length-1) * profile.Length;


		Vector3[] vertices = new Vector3[numVerts];
		int[]tris = new int[numTris * 3];

		for (int i = 0; i < path.Length; i++)
		{
			for (int j = 0; j < profile.Length; j++)
			{
				Vector3 v = path[i].MultiplyPoint(profile[j]);
				vertices[i*profile.Length+j] = v;

				if (closed || i < path.Length - 1)
				{

					tris[6 * (i * profile.Length + j)] = (j + i * profile.Length);
					tris[6 * (i * profile.Length + j) + 1] = ((j + 1) % profile.Length + i * profile.Length);
					tris[6 * (i * profile.Length + j) + 2] = ((j + 1) % profile.Length + ((i + 1) % path.Length) * profile.Length);
					tris[6 * (i * profile.Length + j) + 3] = (j + i * profile.Length);
					tris[6 * (i * profile.Length + j) + 4] = ((j + 1) % profile.Length + ((i + 1) % path.Length) * profile.Length);
					tris[6 * (i * profile.Length + j) + 5] = (j + ((i + 1) % path.Length) * profile.Length);
				}
			}
		}

		mesh.vertices = vertices;

		mesh.triangles = tris;

		mesh.RecalculateNormals();

		return mesh;
	}

	public static Matrix4x4[] MakeCirclePath(float radius, int density)
	{
		Matrix4x4[] path = new Matrix4x4[density];
		for (int i = 0; i < density; i++)
		{
			float angle = (360.0f * i) / density;
			path[i] = Matrix4x4.Rotate(Quaternion.Euler(0, -angle, 0))* Matrix4x4.Translate(new Vector3(radius,0,0));
		}
		return path;
	}


    public static Vector3[] MakeCircleProfile(float radius, int density)
	{
		Vector3[] profile = new Vector3[density];
		for (int i = 0; i < density; i++)
		{
			float angle = (2.0f * Mathf.PI * i) / density;
			profile[i] = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle),0);
		}
		return profile;
	}

    public static Vector3[] MakeSemiCircleProfile(float radius, int density)
    {
        Vector3[] profile = new Vector3[density];
        for (int i = 0; i < density; i++)
        {
            float angle = (Mathf.PI * i) / density;
            profile[i] = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
        }
        return profile;
    }

    public static GameObject Sphere(GameObject sphere, float radius)
    {
        // Profile
        Vector3[] profile = MakeCircleProfile(radius, 20);
        Matrix4x4[] path = MakeCirclePath(0, 40);

        MeshRenderer meshRenderer = sphere.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        MeshFilter meshFilter = sphere.AddComponent<MeshFilter>();
        meshFilter.mesh = Sweep(profile, path, true);
        return sphere;
    }

  


public static Mesh CreateWall(float width, float height, float thickness)
{
    Mesh mesh = new Mesh();

    // Define the vertices of the wall
    Vector3[] vertices = new Vector3[]
    {
        // Front face
        new Vector3(-width / 2, -height / 2, thickness / 2),
        new Vector3(width / 2, -height / 2, thickness / 2),
        new Vector3(width / 2, height / 2, thickness / 2),
        new Vector3(-width / 2, height / 2, thickness / 2),

        // Back face
        new Vector3(-width / 2, -height / 2, -thickness / 2),
        new Vector3(width / 2, -height / 2, -thickness / 2),
        new Vector3(width / 2, height / 2, -thickness / 2),
        new Vector3(-width / 2, height / 2, -thickness / 2),
    };

    // Define the triangles of the wall
    int[] triangles = new int[]
    {
        // Front face
        0, 2, 1,
        0, 3, 2,

        // Back face
        4, 5, 6,
        4, 6, 7,

        // Top face
        3, 2, 6,
        3, 6, 7,

        // Bottom face
        0, 1, 5,
        0, 5, 4,

        // Left face
        0, 4, 7,
        0, 7, 3,

        // Right face
        1, 2, 6,
        1, 6, 5,
    };

    // Define the UVs for texture mapping
    Vector2[] uv = new Vector2[]
    {
        // Front face
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),

        // Back face
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),
    };

    // Ensure that UVs array matches the vertices count
    if (vertices.Length == uv.Length)
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }
    else
    {
        Debug.LogError("UVs array does not match vertices array size.");
    }

    return mesh;
}










public static Mesh CreateHead(float radius)
{
    Mesh mesh = new Mesh();

    // Profile and path for a sphere
    Vector3[] profile = MakeCircleProfile(radius, 20);
    Matrix4x4[] path = MakeCirclePath(0, 40);

    mesh = Sweep(profile, path, true);

    return mesh;
}


public static Mesh CreateTorso(float width, float height, float depth)
{
    Mesh mesh = new Mesh();

    Vector3[] vertices = new Vector3[]
    {
        // Front face
        new Vector3(-width / 2, -height / 2, depth / 2),
        new Vector3(width / 2, -height / 2, depth / 2),
        new Vector3(width / 2, height / 2, depth / 2),
        new Vector3(-width / 2, height / 2, depth / 2),

        // Back face
        new Vector3(-width / 2, -height / 2, -depth / 2),
        new Vector3(width / 2, -height / 2, -depth / 2),
        new Vector3(width / 2, height / 2, -depth / 2),
        new Vector3(-width / 2, height / 2, -depth / 2),
    };

    int[] triangles = new int[]
    {
        // Front face
        0, 2, 1,
        0, 3, 2,

        // Back face
        4, 5, 6,
        4, 6, 7,

        // Top face
        3, 2, 6,
        3, 6, 7,

        // Bottom face
        0, 1, 5,
        0, 5, 4,

        // Left face
        0, 4, 7,
        0, 7, 3,

        // Right face
        1, 2, 6,
        1, 6, 5,
    };

    Vector2[] uv = new Vector2[]
    {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),

        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),
    };

    if (vertices.Length == uv.Length)
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }
    else
    {
        Debug.LogError("UVs array does not match vertices array size.");
    }

    return mesh;
}

public static Mesh CreateArm(float length, float radius)
{
    // Using a cylinder for simplicity
    return Cylinder(20, radius, length);
}

public static Mesh CreateLeg(float length, float radius)
{
    // Using a cylinder for simplicity
    return Cylinder(20, radius, length);
}

public static GameObject CreateHuman(float headRadius, float torsoWidth, float torsoHeight, float torsoDepth, float limbRadius, float armLength, float legLength)
{
    GameObject human = new GameObject("Human");

    GameObject head = new GameObject("Head");
    head.transform.parent = human.transform;
    MeshFilter headFilter = head.AddComponent<MeshFilter>();
    headFilter.mesh = CreateHead(headRadius);
    head.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

    GameObject torso = new GameObject("Torso");
    torso.transform.parent = human.transform;
    MeshFilter torsoFilter = torso.AddComponent<MeshFilter>();
    torsoFilter.mesh = CreateTorso(torsoWidth, torsoHeight, torsoDepth);
    torso.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

    GameObject leftArm = new GameObject("LeftArm");
    leftArm.transform.parent = human.transform;
    MeshFilter leftArmFilter = leftArm.AddComponent<MeshFilter>();
    leftArmFilter.mesh = CreateArm(armLength, limbRadius);
    leftArm.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

    GameObject rightArm = new GameObject("RightArm");
    rightArm.transform.parent = human.transform;
    MeshFilter rightArmFilter = rightArm.AddComponent<MeshFilter>();
    rightArmFilter.mesh = CreateArm(armLength, limbRadius);
    rightArm.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

    GameObject leftLeg = new GameObject("LeftLeg");
    leftLeg.transform.parent = human.transform;
    MeshFilter leftLegFilter = leftLeg.AddComponent<MeshFilter>();
    leftLegFilter.mesh = CreateLeg(legLength, limbRadius);
    leftLeg.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

    GameObject rightLeg = new GameObject("RightLeg");
    rightLeg.transform.parent = human.transform;
    MeshFilter rightLegFilter = rightLeg.AddComponent<MeshFilter>();
    rightLegFilter.mesh = CreateLeg(legLength, limbRadius);
    rightLeg.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

    // Position the limbs relative to the torso
    // Adjust these positions as needed
    leftArm.transform.localPosition = new Vector3(-torsoWidth / 2 - limbRadius, torsoHeight / 2, 0);
    rightArm.transform.localPosition = new Vector3(torsoWidth / 2 + limbRadius, torsoHeight / 2, 0);
    leftLeg.transform.localPosition = new Vector3(-torsoWidth / 2 + limbRadius, -torsoHeight / 2 - legLength / 2, 0);
    rightLeg.transform.localPosition = new Vector3(torsoWidth / 2 - limbRadius, -torsoHeight / 2 - legLength / 2, 0);

    return human;
}




public static Mesh TaperedCylinder(int segments, float baseRadius, float tipRadius, float height)
{
    Mesh mesh = new Mesh();

    Vector3[] vertices = new Vector3[4 * segments + 2]; // 4 * segments (top, top copy, bottom, bottom copy) + 2 center vertices for caps
    float angleStep = Mathf.PI * 2.0f / segments;

    int topStart = 0; // Start position of top vertices
    int bottomStart = segments; // Start position of bottom vertices
    int topCapStart = 2 * segments; // Copy of top vertices for cap
    int bottomCapStart = 3 * segments; // Copy of bottom vertices for cap
    int topCenter = 4 * segments; // Center top vertex (for the cap)
    int bottomCenter = 4 * segments + 1; // Center bottom vertex (for the cap)

    // Create vertices for the top and bottom circles
    for (int i = 0; i < segments; i++)
    {
        float angle = i * angleStep;
        float xBase = baseRadius * Mathf.Cos(angle);
        float zBase = baseRadius * Mathf.Sin(angle);
        float xTip = tipRadius * Mathf.Cos(angle);
        float zTip = tipRadius * Mathf.Sin(angle);

        // Top circle (tip)
        vertices[topStart + i] = new Vector3(xTip, height, zTip);

        // Bottom circle (base)
        vertices[bottomStart + i] = new Vector3(xBase, 0, zBase);

        // Top cap vertices (copy)
        vertices[topCapStart + i] = new Vector3(xTip, height, zTip);

        // Bottom cap vertices (copy)
        vertices[bottomCapStart + i] = new Vector3(xBase, 0, zBase);
    }

    // Center vertices for the caps
    vertices[topCenter] = new Vector3(0, height, 0); // Top center
    vertices[bottomCenter] = new Vector3(0, 0, 0);   // Bottom center

    // Create triangles
    int[] triangles = new int[6 * segments + 6 * segments]; // 6 * segments for side faces, 6 * segments for caps
    int triangleIndex = 0;

    for (int i = 0; i < segments; i++)
    {
        int nextIndex = (i + 1) % segments;

        // Side triangles
        triangles[triangleIndex++] = topStart + i;
        triangles[triangleIndex++] = topStart + nextIndex;
        triangles[triangleIndex++] = bottomStart + nextIndex;

        triangles[triangleIndex++] = topStart + i;
        triangles[triangleIndex++] = bottomStart + nextIndex;
        triangles[triangleIndex++] = bottomStart + i;

        // Cap triangles (top)
        triangles[triangleIndex++] = topCenter;
        triangles[triangleIndex++] = topCapStart + nextIndex;
        triangles[triangleIndex++] = topCapStart + i;

        // Cap triangles (bottom)
        triangles[triangleIndex++] = bottomCapStart + i;
        triangles[triangleIndex++] = bottomCapStart + nextIndex;
        triangles[triangleIndex++] = bottomCenter;
    }

    // Assign vertices and triangles to the mesh
    mesh.vertices = vertices;
    mesh.triangles = triangles;

    // Recalculate normals for proper shading
    mesh.RecalculateNormals();

    return mesh;
}

   public static Mesh EllipticalCylinder(int segments, float width, float depth, float length)
{
    Mesh mesh = new Mesh();

    Vector3[] vertices = new Vector3[4 * segments + 2]; // 4 * segments (top, top copy, bottom, bottom copy) + 2 center vertices for caps
    float angleStep = Mathf.PI * 2.0f / segments;

    int topStart = 0; // Start position of top vertices
    int bottomStart = segments; // Start position of bottom vertices
    int topCapStart = 2 * segments; // Copy of top vertices for cap
    int bottomCapStart = 3 * segments; // Copy of bottom vertices for cap
    int topCenter = 4 * segments; // Center top vertex (for the cap)
    int bottomCenter = 4 * segments + 1; // Center bottom vertex (for the cap)

    // Create vertices for top and bottom ellipses
    for (int i = 0; i < segments; i++)
    {
        float angle = i * angleStep;
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);

        // Top ellipse (forward-facing side)
        vertices[topStart + i] = new Vector3(x * width, length / 2, z * depth);

        // Bottom ellipse (backward-facing side)
        vertices[bottomStart + i] = new Vector3(x * width, -length / 2, z * depth);

        // Top cap vertices (copy)
        vertices[topCapStart + i] = new Vector3(x * width, length / 2, z * depth);

        // Bottom cap vertices (copy)
        vertices[bottomCapStart + i] = new Vector3(x * width, -length / 2, z * depth);
    }

    // Center vertices for the caps
    vertices[topCenter] = new Vector3(0, length / 2, 0); // Top center
    vertices[bottomCenter] = new Vector3(0, -length / 2, 0); // Bottom center

    // Create triangles
    int[] triangles = new int[6 * segments + 6 * segments]; // 6 * segments for side faces, 6 * segments for caps
    int triangleIndex = 0;

    for (int i = 0; i < segments; i++)
    {
        int nextIndex = (i + 1) % segments;

        // Side triangles
        triangles[triangleIndex++] = topStart + i;
        triangles[triangleIndex++] = topStart + nextIndex;
        triangles[triangleIndex++] = bottomStart + nextIndex;

        triangles[triangleIndex++] = topStart + i;
        triangles[triangleIndex++] = bottomStart + nextIndex;
        triangles[triangleIndex++] = bottomStart + i;

        // Cap triangles (top)
        triangles[triangleIndex++] = topCenter;
        triangles[triangleIndex++] = topCapStart + nextIndex;
        triangles[triangleIndex++] = topCapStart + i;

        // Cap triangles (bottom)
        triangles[triangleIndex++] = bottomCapStart + i;
        triangles[triangleIndex++] = bottomCapStart + nextIndex;
        triangles[triangleIndex++] = bottomCenter;
    }

    // Assign vertices and triangles to the mesh
    mesh.vertices = vertices;
    mesh.triangles = triangles;

    // Recalculate normals for proper shading
    mesh.RecalculateNormals();

    return mesh;
}




    public static Mesh Gear(
        float innerRadius,
        float outerRadius,
        float hubRadius,
        int numTeeth,
        float toothWidth,
        float depth)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // Angle step for each tooth (in degrees)
        float angleStep = 360f / numTeeth; // Angle per tooth (degrees)

        // Ensure toothAngle is calculated properly as a proportion of the angle step.
        float toothAngle = toothWidth / (Mathf.PI * outerRadius) * 180f; // Converted into degrees
        float flatAngle = angleStep - toothAngle;

        // Loop over each tooth and generate the geometry for the gear
        for (int i = 0; i < numTeeth; i++)
        {
            // Current angle (in degrees) for each tooth
            float angle = i * angleStep*2;

            // Calculate the angles for each tooth and flat section (converted to radians)
            float radFlat1 = Mathf.Deg2Rad * (angle + flatAngle / 2);
            float radFlat2 = Mathf.Deg2Rad * (angle - flatAngle / 2);
            float radTooth1 = Mathf.Deg2Rad * (angle + toothAngle / 2);
            float radTooth2 = Mathf.Deg2Rad * (angle - toothAngle / 2);

            // Generate outer vertices for both tooth and flat sections
            Vector3 outerFlat1 = new Vector3(Mathf.Cos(radFlat1) * outerRadius, Mathf.Sin(radFlat1) * outerRadius, depth / 2);
            Vector3 outerFlat2 = new Vector3(Mathf.Cos(radFlat2) * outerRadius, Mathf.Sin(radFlat2) * outerRadius, depth / 2);
            Vector3 outerTooth1 = new Vector3(Mathf.Cos(radTooth1) * outerRadius, Mathf.Sin(radTooth1) * outerRadius, depth / 2);
            Vector3 outerTooth2 = new Vector3(Mathf.Cos(radTooth2) * outerRadius, Mathf.Sin(radTooth2) * outerRadius, depth / 2);

            // Generate inner circle vertices for both tooth and flat sections
            Vector3 innerFlat1 = new Vector3(Mathf.Cos(radFlat1) * innerRadius, Mathf.Sin(radFlat1) * innerRadius, depth / 2);
            Vector3 innerFlat2 = new Vector3(Mathf.Cos(radFlat2) * innerRadius, Mathf.Sin(radFlat2) * innerRadius, depth / 2);

            // Add front side vertices (Z = depth / 2)
            vertices.Add(innerFlat1); // Inner flat vertex 1
            vertices.Add(innerFlat2); // Inner flat vertex 2
            vertices.Add(outerFlat1); // Outer flat vertex 1
            vertices.Add(outerFlat2); // Outer flat vertex 2
            vertices.Add(outerTooth1); // Outer tooth vertex 1
            vertices.Add(outerTooth2); // Outer tooth vertex 2

            // Add back side vertices (Z = -depth / 2)
            vertices.Add(new Vector3(innerFlat1.x, innerFlat1.y, -depth / 2)); // Inner flat back 1
            vertices.Add(new Vector3(innerFlat2.x, innerFlat2.y, -depth / 2)); // Inner flat back 2
            vertices.Add(new Vector3(outerFlat1.x, outerFlat1.y, -depth / 2)); // Outer flat back 1
            vertices.Add(new Vector3(outerFlat2.x, outerFlat2.y, -depth / 2)); // Outer flat back 2
            vertices.Add(new Vector3(outerTooth1.x, outerTooth1.y, -depth / 2)); // Outer tooth back 1
            vertices.Add(new Vector3(outerTooth2.x, outerTooth2.y, -depth / 2)); // Outer tooth back 2
        }

        // Generate triangles for the front, back, and side faces of the gear
        for (int i = 0; i < numTeeth * 6; i += 6)
        {
            // Front face (clockwise)
            triangles.Add(i + 0); // Inner flat vertex 1
            triangles.Add(i + 2); // Outer flat vertex 1
            triangles.Add(i + 4); // Outer tooth vertex 1

            triangles.Add(i + 4); // Outer tooth vertex 1
            triangles.Add(i + 3); // Outer flat vertex 2
            triangles.Add(i + 5); // Outer tooth vertex 2

            // Back face (counterclockwise)
            triangles.Add(i + 6 + 0); // Inner flat back 1
            triangles.Add(i + 6 + 4); // Outer tooth back 1
            triangles.Add(i + 6 + 2); // Outer flat back 1

            triangles.Add(i + 6 + 4); // Outer tooth back 1
            triangles.Add(i + 6 + 3); // Outer flat back 2
            triangles.Add(i + 6 + 5); // Outer tooth back 2

            // Side faces (connecting front and back)
            for (int j = 0; j < 6; j++)
            {
                int next = (j + 1) % 6;

                triangles.Add(i + j);
                triangles.Add(i + j + 6);
                triangles.Add(i + next);

                triangles.Add(i + next);
                triangles.Add(i + j + 6);
                triangles.Add(i + next + 6);
            }
        }

        // Assign the vertices and triangles to the mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }
}









