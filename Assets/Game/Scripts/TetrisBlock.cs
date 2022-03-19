using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TetrisBlock : MonoBehaviour
{
    //Vertical Movement
    private float _previousTime;
    public float fallTime = 0.8f;

    //Restrictions Of movement if needed 
    public static int height = 100;
    public static int width = 100;
    public static Transform[,] grid = new Transform[width, height];




    //Rotation 
    public Vector3 rotationPoint;

    // Update is called once per frame
    void Update()
    {
        //Horizontal Movement
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }

        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }

        //Vertical Movement

        if (Time.time - _previousTime > (Input.GetKey(KeyCode.S) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            _previousTime = Time.time;
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                enabled = false;
                FindObjectOfType<SpawnerScript>().AllowSpawn();
            }
        }
        
        
    }

    

    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            var childPos = children.transform.position;
            var roundX = Mathf.RoundToInt(childPos.x);
            var roundY = Mathf.RoundToInt(childPos.y);
            grid[roundX, roundY] = children;
        }
    }

    //Restrictions Of movement if needed 
    private bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            var childPos = children.transform.position;
            var roundX = Mathf.RoundToInt(childPos.x);
            var roundY = Mathf.RoundToInt(childPos.y);

            // if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height) 
            if (roundY < 0)
            {
                return false;
            }

            if (grid[roundX, roundY])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Add blocks to grid.
    /// Used to enter new platforms to the grid.
    /// </summary>
    /// <param name="minX"></param>
    /// <param name="maxX"></param>
    /// <param name="minY"></param>
    /// <param name="maxY"></param>
    public static void AddBlocksToGrid(Transform obj, int minX, int maxX, int minY, int maxY)
    {
        for (int i = minY; i < maxY; i++)
        {
            for (int j = minX; j < maxX; j++)
            {
                grid[j, i] = obj;
            }
        }
    }

    /// <summary>
    /// Remove blocks from the grid.
    /// Used to remove platforms and previous blocks.
    /// </summary>
    /// <param name="minX"></param>
    /// <param name="maxX"></param>
    /// <param name="minY"></param>
    /// <param name="maxY"></param>
    public static void RemoveBlocksFromGrid(Transform obj, int minX, int maxX, int minY, int maxY)
    {
        for (int i = minY; i < maxY; i++)
        {
            for (int j = minX; j < maxX; j++)
            {
                grid[j, i] = obj;
            }
        }
    }
}