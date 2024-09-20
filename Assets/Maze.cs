using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public int width = 300; // Increased width
    public int height = 300; // Increased height
    public float wallWidth = 1.0f;
    public float wallHeight = 3.0f;
    public float wallThickness = 0.2f;
    public float wallSpacing = 1.0f; // Reduced spacing to increase wall density

    private void Start()
    {
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        int[,] mazeLayout = GenerateMazeLayout(width, height);

        for (int y = 0; y < mazeLayout.GetLength(0); y++)
        {
            for (int x = 0; x < mazeLayout.GetLength(1); x++)
            {
                if (mazeLayout[y, x] == 1)
                {
                    Vector3 position = new Vector3(x * (wallWidth + wallSpacing), wallHeight / 2, y * (wallWidth + wallSpacing));
                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
                    wall.transform.localScale = new Vector3(wallWidth, wallHeight, wallThickness);
                    wall.transform.parent = this.transform; // Set maze as parent
                }
            }
        }
    }

    private int[,] GenerateMazeLayout(int width, int height)
    {
        int[,] mazeLayout = new int[height, width];

        // Initialize maze with walls
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mazeLayout[y, x] = 1; // Set all cells as walls initially
            }
        }

        // Implement a maze generation algorithm (Depth-First Search)
        MazeAlgorithm(mazeLayout, 1, 1);

        mazeLayout[0, 0] = 0; // Start point
        mazeLayout[height - 1, width - 1] = 0; // End point

        return mazeLayout;
    }

    private void MazeAlgorithm(int[,] maze, int startX, int startY)
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(new Vector2Int(startX, startY));

        int[,] directions = new int[,]
        {
            { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }
        };

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Pop();
            int x = current.x;
            int y = current.y;

            if (x < 1 || x >= width - 1 || y < 1 || y >= height - 1)
                continue;

            if (maze[y, x] == 0)
                continue;

            maze[y, x] = 0; // Mark as part of the maze

            List<Vector2Int> neighbors = new List<Vector2Int>();

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int dx = directions[i, 0];
                int dy = directions[i, 1];
                int nx = x + dx * 2;
                int ny = y + dy * 2;

                if (nx >= 1 && nx < width - 1 && ny >= 1 && ny < height - 1 && maze[ny, nx] == 1)
                {
                    neighbors.Add(new Vector2Int(nx, ny));
                }
            }

            if (neighbors.Count > 0)
            {
                stack.Push(new Vector2Int(x, y));

                Vector2Int next = neighbors[Random.Range(0, neighbors.Count)];
                int nx = next.x;
                int ny = next.y;

                maze[(y + ny) / 2, (x + nx) / 2] = 0;
                stack.Push(next);
            }
        }
    }
}
