using UnityEngine;

public class DetailedHouse : MonoBehaviour
{
    // Materials for different elements (Textures will be applied to these in the Unity Editor)
    public Material wallMaterial;
    public Material floorMaterial;
    public Material windowMaterial;
    public Material sofaMaterial;
    public Material chairMaterial;
    public Material tableMaterial;
    public Material tvMaterial;
    public Material bookMaterial;
    public Material paintingMaterial;
    public Material keyMaterial;  
   

    // Texture variables for assigning textures via the editor
    public Texture keyTexture;     
    public Texture wallTexture;
    public Texture floorTexture;
    public Texture windowTexture;
    public Texture sofaTexture;
    public Texture chairTexture;
    public Texture tableTexture;
    public Texture tvTexture;
    public Texture bookTexture;
    public Texture paintingTexture;

    // Fan reference
    private GameObject fan;

    void Start()
    {
        // Create a parent GameObject for the house
        GameObject house = new GameObject("House");

        // Initialize the house and its elements
        CreateHouse(house);
        CreateSofa(house);
        CreateFan(house);  // Setup fan
        CreateWindows(house);
        CreateFurniture(house);
        CreateAnotherRoom(house);
	CreateChestOfDrawers(house);
	CreateKey();
    }

    void Update()
    {
        // Rotate the fan blades continuously
        if (fan != null)
        {
            fan.transform.Rotate(0, 100f * Time.deltaTime, 0);  // Rotate around the Y-axis
        }
    }

    // Method to assign texture and material to a GameObject
    void AssignMaterialAndTexture(GameObject obj, Material material, Texture texture)
    {
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        if (material != null)
        {
            if (texture != null)
            {
                material.mainTexture = texture;
            }
            meshRenderer.material = material;
        }
        else
        {
            Debug.LogWarning($"No material assigned to {obj.name}. Please assign a material in the Unity Editor.");
        }
    }

    // Helper method to create walls
    void CreateWall(GameObject parent, string name, Vector3 position, Vector3 rotation, float width, float height, float depth)
    {
        GameObject wall = new GameObject(name);
        wall.transform.parent = parent.transform;  // Assigning wall as a child of the parent
        MeshFilter wallFilter = wall.AddComponent<MeshFilter>();
        wallFilter.mesh = MeshUtilities.CreateWall(width, height, depth);
        AssignMaterialAndTexture(wall, wallMaterial, wallTexture);
        wall.transform.position = position;
        wall.transform.eulerAngles = rotation;
    }

// Create the house structure
void CreateHouse(GameObject house)
{
    // Create walls using the helper method
    CreateWall(house, "BackWall", new Vector3(0, 2.5f, -5), Vector3.zero, 10f, 5f, 0.1f);
    CreateWall(house, "LeftWall", new Vector3(-5, 2.5f, 0), new Vector3(0, 90, 0), 10f, 5f, 0.1f);
    
    // Right wall with entrance (split into two walls) and rotated by 180 degrees around the Y-axis
    float expandedZDepth = 4.2f; // Slightly increased to expand the walls in the z-axis
    CreateWall(house, "RightWallLeft", new Vector3(4f, 2.5f, 3.10f), new Vector3(0, 180, 0), 0.1f, 5f, expandedZDepth);  // Left side of entrance
    CreateWall(house, "RightWallRight", new Vector3(4f, 2.5f, -3.10f), new Vector3(0, 180, 0), 0.1f, 5f, expandedZDepth); // Right side of entrance
    
    // Front walls
    CreateWall(house, "FrontWallLeft", new Vector3(-3f, 2.5f, 5), Vector3.zero, 4f, 5f, 0.1f);
    CreateWall(house, "FrontWallRight", new Vector3(3f, 2.5f, 5), Vector3.zero, 4f, 5f, 0.1f);

    // Create an even larger floor
    GameObject floor = new GameObject("Floor");
    floor.transform.parent = house.transform;  // Assigning floor as a child of house
    MeshFilter floorFilter = floor.AddComponent<MeshFilter>();
    floorFilter.mesh = MeshUtilities.CreateWall(40f, 30f, 0.1f);  // Further expanded floor mesh dimensions
    AssignMaterialAndTexture(floor, floorMaterial, floorTexture);
    floor.transform.position = new Vector3(0, 0, 0);  // Positioning the floor
    floor.transform.eulerAngles = new Vector3(90, 0, 0);  // Rotate to make it horizontal
}


    // Create the sofa and position it
    void CreateSofa(GameObject house)
    {
        GameObject sofa = new GameObject("Sofa");
        sofa.transform.parent = house.transform;  // Assigning sofa as a child of house

        // Sofa base
        GameObject baseSofa = new GameObject("SofaBase");
        baseSofa.transform.parent = sofa.transform;
        MeshFilter baseSofaFilter = baseSofa.AddComponent<MeshFilter>();
        baseSofaFilter.mesh = MeshUtilities.CreateWall(2f, 0.5f, 1f);  // Create sofa base mesh
        AssignMaterialAndTexture(baseSofa, sofaMaterial, sofaTexture);
        baseSofa.transform.position = new Vector3(0, 0.25f, 0);  // Positioning the base

        // Sofa back
        GameObject backSofa = new GameObject("SofaBack");
        backSofa.transform.parent = sofa.transform;
        MeshFilter backSofaFilter = backSofa.AddComponent<MeshFilter>();
        backSofaFilter.mesh = MeshUtilities.CreateWall(2f, 0.5f, 0.1f);  // Create sofa back mesh
        AssignMaterialAndTexture(backSofa, sofaMaterial, sofaTexture);
        backSofa.transform.position = new Vector3(0, 0.75f, -0.45f);  // Positioning the back

        // Rotate the sofa by 180 degrees along the Y-axis and position it slightly behind to make space for the table
        sofa.transform.Rotate(0, 180, 0);  // Rotation of sofa
        sofa.transform.position = new Vector3(0, 0, 1.5f);  // Positioning the sofa further back in the room
    }

    // Create a fan on the ceiling
    void CreateFan(GameObject house)
{
    fan = new GameObject("Fan");
    fan.transform.parent = house.transform;

    for (int i = 0; i < 2; i++)
    {
        GameObject blade = new GameObject("FanBlade" + (i + 1));
        blade.transform.parent = fan.transform;
        MeshFilter bladeFilter = blade.AddComponent<MeshFilter>();
        bladeFilter.mesh = MeshUtilities.CreateWall(1f, 0.1f, 0.2f);
        AssignMaterialAndTexture(blade, wallMaterial, wallTexture);
        blade.transform.localPosition = Vector3.zero;
        blade.transform.localRotation = Quaternion.Euler(0, 0, i * 90);  // Rotate blades
    }

    fan.transform.position = new Vector3(0, 5, 0);
}

    // Create windows for the house
    void CreateWindows(GameObject house)
    {
        // Front window
        GameObject window2 = new GameObject("Window2");
        window2.transform.parent = house.transform;  // Assigning window as a child of house
        MeshFilter window2Filter = window2.AddComponent<MeshFilter>();
        window2Filter.mesh = MeshUtilities.CreateWall(1.5f, 1.5f, 0.1f);  // Create window mesh
        AssignMaterialAndTexture(window2, windowMaterial, windowTexture);
        window2.transform.position = new Vector3(-3f, 2.5f, 4.95f);  // Positioning the front window

        // Add first window to the right wall
        GameObject rightWindow1 = new GameObject("RightWallWindow1");
        rightWindow1.transform.parent = house.transform;
        MeshFilter rightWindow1Filter = rightWindow1.AddComponent<MeshFilter>();
        rightWindow1Filter.mesh = MeshUtilities.CreateWall(1.5f, 1.5f, 0.1f);  // Create window mesh for the right wall
        AssignMaterialAndTexture(rightWindow1, windowMaterial, windowTexture);
        rightWindow1.transform.position = new Vector3(3.98f, 2.5f, 2f);  // Positioning the first window on the right wall
        rightWindow1.transform.eulerAngles = new Vector3(0, 90, 0);  // Align the window with the right wall

        // Add second window to the right wall
        GameObject rightWindow2 = new GameObject("RightWallWindow2");
        rightWindow2.transform.parent = house.transform;
        MeshFilter rightWindow2Filter = rightWindow2.AddComponent<MeshFilter>();
        rightWindow2Filter.mesh = MeshUtilities.CreateWall(1.5f, 1.5f, 0.1f);  // Create second window mesh for the right wall
        AssignMaterialAndTexture(rightWindow2, windowMaterial, windowTexture);
        rightWindow2.transform.position = new Vector3(3.98f, 2.5f, -2f);  // Positioning the second window on the right wall
        rightWindow2.transform.eulerAngles = new Vector3(0, 90, 0);  // Align the second window with the right wall
    }

    // Create furniture items like chairs, table, TV, and books
    void CreateFurniture(GameObject house)
    {
        // Chair
        GameObject chair = new GameObject("Chair");
        chair.transform.parent = house.transform;  // Assigning chair as a child of house
        MeshFilter chairFilter = chair.AddComponent<MeshFilter>();
        chairFilter.mesh = MeshUtilities.CreateWall(0.5f, 1f, 0.5f);  // Create chair mesh
        AssignMaterialAndTexture(chair, chairMaterial, chairTexture);
        chair.transform.position = new Vector3(-2f, 0.5f, 2f);  // Positioning the chair

        // Table with legs
        CreateTable(house);

        // TV
        GameObject tv = new GameObject("TV");
        tv.transform.parent = house.transform;  // Assigning TV as a child of house
        MeshFilter tvFilter = tv.AddComponent<MeshFilter>();
        tvFilter.mesh = MeshUtilities.CreateWall(1.5f, 1f, 0.1f);  // TV mesh dimensions
        AssignMaterialAndTexture(tv, tvMaterial, tvTexture);
        tv.transform.localScale = new Vector3(2f, 2f, 1f);  // Scale the TV (increase width and height)
        tv.transform.position = new Vector3(0f, 1.5f, -4.9f);  // Positioning the TV

        // Books placed on the rotated table
        for (int i = 0; i < 3; i++)
        {
            GameObject book = new GameObject($"Book{i + 1}");
            book.transform.parent = house.transform;  // Assigning books as children of house
            MeshFilter bookFilter = book.AddComponent<MeshFilter>();
            bookFilter.mesh = MeshUtilities.CreateWall(0.2f, 0.5f, 0.1f);  // Create book mesh
            AssignMaterialAndTexture(book, bookMaterial, bookTexture);

            // Rotate the books along the X-axis by 90 degrees and place them on the table at Y = 0.62
            book.transform.eulerAngles = new Vector3(90, 0, 0);  // Rotate the book along the X-axis
            book.transform.position = new Vector3(-0.15f + i * 0.3f, 0.66f, -1f);  // Positioning the books on the rotated table
        }

        // Painting
        GameObject painting = new GameObject("Painting");
        painting.transform.parent = house.transform;  // Assigning painting as a child of house
        MeshFilter paintingFilter = painting.AddComponent<MeshFilter>();
        paintingFilter.mesh = MeshUtilities.CreateWall(2f, 1f, 0.05f);  // Create painting mesh
        AssignMaterialAndTexture(painting, paintingMaterial, paintingTexture);
        painting.transform.position = new Vector3(-4.94f, 3f, 0);  // Positioning the painting
        painting.transform.rotation = Quaternion.Euler(0, -90, 0);  // Rotate painting to face inwards
    }

    // Helper method to create table and legs
    void CreateTable(GameObject house)
    {
        GameObject table = new GameObject("Table");
        table.transform.parent = house.transform;  // Assigning table as a child of house

        // Table top
        GameObject tableTop = new GameObject("TableTop");
        tableTop.transform.parent = table.transform;
        MeshFilter tableTopFilter = tableTop.AddComponent<MeshFilter>();
        tableTopFilter.mesh = MeshUtilities.CreateWall(1f, 0.1f, 1.5f);  // Create table top mesh
        AssignMaterialAndTexture(tableTop, tableMaterial, tableTexture);
        tableTop.transform.localPosition = new Vector3(0, 0.55f, 0);  // Adjust the height of the table top

        // Table legs
        float legHeight = 0.5f;  // Height of the table leg
        float legRadius = 0.05f;  // Radius of the table leg
        CreateTableLeg(table, new Vector3(-0.45f, 0.25f, -0.7f), legRadius, legHeight);  // Back-left leg
        CreateTableLeg(table, new Vector3(0.45f, 0.25f, -0.7f), legRadius, legHeight);  // Back-right leg
        CreateTableLeg(table, new Vector3(-0.45f, 0.25f, 0.7f), legRadius, legHeight);  // Front-left leg
        CreateTableLeg(table, new Vector3(0.45f, 0.25f, 0.7f), legRadius, legHeight);  // Front-right leg

        // Rotate the table around the Y-axis by 90 degrees
        table.transform.eulerAngles = new Vector3(0, 90, 0);

        // Position the table in front of the sofa
        table.transform.position = new Vector3(0, 0, -1);  // Positioning the table closer to the sofa
    }

    // Create table legs using MeshUtilities Cylinder method
    void CreateTableLeg(GameObject parent, Vector3 position, float radius, float height)
    {
        GameObject leg = new GameObject("TableLeg");
        leg.transform.parent = parent.transform;
        MeshFilter legFilter = leg.AddComponent<MeshFilter>();

        // Create a cylinder for the leg using MeshUtilities.Cylinder
        legFilter.mesh = MeshUtilities.Cylinder(16, radius, height);
        AssignMaterialAndTexture(leg, tableMaterial, tableTexture);  // Assign material and texture
        leg.transform.localPosition = position;  // Positioning the leg
    }

    // Create another connected room
    void CreateAnotherRoom(GameObject house)
    {
        GameObject otherRoomWall = new GameObject("OtherRoomWall");
        otherRoomWall.transform.parent = house.transform;  // Assigning the other room as a child of house
        MeshFilter otherRoomWallFilter = otherRoomWall.AddComponent<MeshFilter>();
        otherRoomWallFilter.mesh = MeshUtilities.CreateWall(10f, 5f, 0.1f);  // Create wall mesh
        AssignMaterialAndTexture(otherRoomWall, wallMaterial, wallTexture);
        otherRoomWall.transform.position = new Vector3(10, 2.5f, 5);  // Positioning the wall

        GameObject otherRoomFloor = new GameObject("OtherRoomFloor");
        otherRoomFloor.transform.parent = house.transform;  // Assigning the floor as a child of house
        MeshFilter otherRoomFloorFilter = otherRoomFloor.AddComponent<MeshFilter>();
        otherRoomFloorFilter.mesh = MeshUtilities.CreateWall(20f, 10f, 0.1f);  // Create floor mesh
        AssignMaterialAndTexture(otherRoomFloor, floorMaterial, floorTexture);
        otherRoomFloor.transform.position = new Vector3(10, 0, 0);  // Positioning the floor
        otherRoomFloor.transform.eulerAngles = new Vector3(90, 0, 0);  // Rotate to make it horizontal
    }





    public Material woodenMaterial; // Assign this in the inspector
    public Texture2D chestTexture; // Assign this in the inspector

    void CreateChestOfDrawers(GameObject room)
    {
        GameObject chest = new GameObject("ChestOfDrawers");
        chest.transform.parent = room.transform;
        chest.transform.position = new Vector3(-3.68f, 0.03f, -1.96f); // Position the chest in the room
        chest.transform.rotation = Quaternion.Euler(0, -132.526f, 0); // Rotate the cabinet around Y-axis

        MeshFilter meshFilter = chest.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = chest.AddComponent<MeshRenderer>();

        // Use the material assigned from the inspector
        if (woodenMaterial != null)
        {
            meshRenderer.material = woodenMaterial;
        }
        else
        {
            Debug.LogWarning("Wooden Material not assigned!");
            meshRenderer.material = new Material(Shader.Find("Standard")); // Fallback material
        }

        // Use the texture assigned from the inspector
        if (chestTexture != null)
        {
            meshRenderer.material.mainTexture = chestTexture;
        }
        else
        {
            Debug.LogWarning("Chest Texture not assigned!");
        }

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            // Front face (3 drawers)
            new Vector3(-0.5f, 0, 0.25f), new Vector3(0.5f, 0, 0.25f), new Vector3(0.5f, 0.5f, 0.25f), new Vector3(-0.5f, 0.5f, 0.25f),
            new Vector3(-0.5f, 0.5f, 0.25f), new Vector3(0.5f, 0.5f, 0.25f), new Vector3(0.5f, 1.0f, 0.25f), new Vector3(-0.5f, 1.0f, 0.25f),
            new Vector3(-0.5f, 1.0f, 0.25f), new Vector3(0.5f, 1.0f, 0.25f), new Vector3(0.5f, 1.5f, 0.25f), new Vector3(-0.5f, 1.5f, 0.25f),

            // Back face
            new Vector3(-0.5f, 0, -0.25f), new Vector3(0.5f, 0, -0.25f), new Vector3(0.5f, 1.5f, -0.25f), new Vector3(-0.5f, 1.5f, -0.25f),

            // Left side
            new Vector3(-0.5f, 0, -0.25f), new Vector3(-0.5f, 0, 0.25f), new Vector3(-0.5f, 1.5f, 0.25f), new Vector3(-0.5f, 1.5f, -0.25f),

            // Right side
            new Vector3(0.5f, 0, -0.25f), new Vector3(0.5f, 0, 0.25f), new Vector3(0.5f, 1.5f, 0.25f), new Vector3(0.5f, 1.5f, -0.25f),

            // Top face
            new Vector3(-0.5f, 1.5f, -0.25f), new Vector3(0.5f, 1.5f, -0.25f), new Vector3(0.5f, 1.5f, 0.25f), new Vector3(-0.5f, 1.5f, 0.25f),

            // Bottom face (optional, if needed)
            new Vector3(-0.5f, 0, -0.25f), new Vector3(0.5f, 0, -0.25f), new Vector3(0.5f, 0, 0.25f), new Vector3(-0.5f, 0, 0.25f),
        };

        int[] triangles = new int[]
        {
            // Front face (3 drawers)
            0, 2, 1, 0, 3, 2,       // Bottom drawer
            4, 6, 5, 4, 7, 6,       // Middle drawer
            8, 10, 9, 8, 11, 10,    // Top drawer

            // Back face
            12, 13, 14, 12, 14, 15,

            // Left side
            16, 18, 17, 16, 19, 18,

            // Right side
            20, 21, 22, 20, 23, 22,

            // Top face
            24, 26, 25, 24, 27, 26,

            // Bottom face (optional)
            28, 30, 29, 28, 31, 30,
        };

        Vector2[] uvs = new Vector2[]
        {
            // Front face (drawers UVs)
            new Vector2(0.1f, 0.0f), new Vector2(0.4f, 0.0f), new Vector2(0.4f, 0.3f), new Vector2(0.1f, 0.3f), // Bottom
            new Vector2(0.1f, 0.3f), new Vector2(0.4f, 0.3f), new Vector2(0.4f, 0.6f), new Vector2(0.1f, 0.6f), // Middle
            new Vector2(0.1f, 0.6f), new Vector2(0.4f, 0.6f), new Vector2(0.4f, 0.9f), new Vector2(0.1f, 0.9f), // Top

            // Back face UVs (only apply if visible)
            new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector2(0.0f, 1.0f),

            // Left side UVs (only apply if visible)
            new Vector2(0.0f, 0.0f), new Vector2(0.1f, 0.0f), new Vector2(0.1f, 1.0f), new Vector2(0.0f, 1.0f),

            // Right side UVs (only apply if visible)
            new Vector2(0.9f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector2(0.9f, 1.0f),

            // Top face UVs (only apply if visible)
            new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector2(0.0f, 1.0f),

            // Bottom face UVs (optional, can be left at (0,0) to avoid texture)
            new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector2(0.0f, 1.0f),
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals(); // Recalculate normals for lighting
        meshFilter.mesh = mesh; // Assign the mesh to the MeshFilter

        // Optionally, add a BoxCollider to interact with the chest
        chest.AddComponent<BoxCollider>();
    }










void CreateKey()
{
    // Create a new GameObject for the key
    GameObject key = new GameObject("Key");
    
    // Create the mesh filter and renderer
    MeshFilter meshFilter = key.AddComponent<MeshFilter>();
    MeshRenderer meshRenderer = key.AddComponent<MeshRenderer>();

    // Assign the material and texture
    if (keyMaterial != null)
    {
        if (keyTexture != null)
        {
            keyMaterial.mainTexture = keyTexture;
        }
        meshRenderer.material = keyMaterial;
    }
    else
    {
        Debug.LogWarning("No material assigned to the key. Please assign a material in the Unity Editor.");
    }

    // Create the key mesh
    meshFilter.mesh = CreateKeyMesh();
}

Mesh CreateKeyMesh()
{
    Mesh mesh = new Mesh();

    // Create components for the key using basic shapes
    Mesh headMesh = CreateKeyHead();
    Mesh shaftMesh = CreateKeyShaft();
    Mesh teethMesh = CreateKeyTeeth();

    // Combine the meshes
    CombineInstance[] combine = new CombineInstance[3];
    combine[0].mesh = headMesh;
    combine[0].transform = Matrix4x4.TRS(Vector3.up * 0.1f, Quaternion.identity, Vector3.one); // Adjusted position for head

    combine[1].mesh = shaftMesh;
    combine[1].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one); // Positioning the shaft

    combine[2].mesh = teethMesh;
    combine[2].transform = Matrix4x4.TRS(Vector3.down * 0.05f, Quaternion.identity, Vector3.one); // Positioned closer to the shaft's tip

    mesh.CombineMeshes(combine);
    mesh.RecalculateNormals();

    return mesh;
}

Mesh CreateKeyHead()
{
    float radius = 0.05f;
    GameObject head = new GameObject("KeyHead");

    // Generate the sphere
    MeshUtilities.Sphere(head, radius);
    Mesh headMesh = head.GetComponent<MeshFilter>().mesh;
    Vector3[] vertices = headMesh.vertices;
    Vector2[] uvs = new Vector2[vertices.Length];

    // Spherical mapping based on vertex positions
    for (int i = 0; i < vertices.Length; i++)
    {
        Vector3 v = vertices[i].normalized;
        uvs[i] = new Vector2(
            0.5f + Mathf.Atan2(v.z, v.x) / (2f * Mathf.PI), 
            0.5f - Mathf.Asin(v.y) / Mathf.PI
        );
    }

    headMesh.uv = uvs;
    GameObject.Destroy(head);
    return headMesh;
}

Mesh CreateKeyShaft()
{
    int segments = 12;
    float radius = 0.02f;
    float height = 0.1f;
    Mesh shaftMesh = MeshUtilities.Cylinder(segments, radius, height);
    Vector3[] vertices = shaftMesh.vertices;
    Vector2[] uvs = new Vector2[vertices.Length];

    // Cylindrical UV mapping along the shaft
    for (int i = 0; i < vertices.Length; i++)
    {
        float u = Mathf.Atan2(vertices[i].z, vertices[i].x) / (2 * Mathf.PI) + 0.5f;
        float v = vertices[i].y / height + 0.5f;
        uvs[i] = new Vector2(u, v);
    }

    shaftMesh.uv = uvs;
    return shaftMesh;
}


Mesh CreateKeyTeeth()
{
    float toothWidth = 0.03f;
    float toothHeight = 0.05f;
    float toothDepth = 0.02f;

    Mesh teeth = new Mesh();

    Vector3[] vertices = new Vector3[]
    {
        // Front face
        new Vector3(-toothWidth, 0, -toothDepth),
        new Vector3(toothWidth, 0, -toothDepth),
        new Vector3(toothWidth, toothHeight, -toothDepth),
        new Vector3(-toothWidth, toothHeight, -toothDepth),
        // Back face
        new Vector3(-toothWidth, 0, toothDepth),
        new Vector3(toothWidth, 0, toothDepth),
        new Vector3(toothWidth, toothHeight, toothDepth),
        new Vector3(-toothWidth, toothHeight, toothDepth)
    };

    int[] tris = new int[]
    {
        // Front face
        0, 2, 1,
        0, 3, 2,
        // Back face
        4, 5, 6,
        4, 6, 7,
        // Left face
        0, 3, 7,
        0, 7, 4,
        // Right face
        1, 2, 6,
        1, 6, 5,
        // Top face
        2, 3, 7,
        2, 7, 6,
        // Bottom face
        0, 4, 5,
        0, 5, 1
    };

    teeth.vertices = vertices;
    teeth.triangles = tris;

    // Ensure the UV array matches the vertices array
    Vector2[] uvs = new Vector2[vertices.Length];

    // Map each face individually
    uvs[0] = new Vector2(0, 0); uvs[1] = new Vector2(1, 0); uvs[2] = new Vector2(1, 1); uvs[3] = new Vector2(0, 1); // Front face
    uvs[4] = new Vector2(0, 0); uvs[5] = new Vector2(1, 0); uvs[6] = new Vector2(1, 1); uvs[7] = new Vector2(0, 1); // Back face

    teeth.uv = uvs;
    teeth.RecalculateNormals();

    return teeth;
}


}
