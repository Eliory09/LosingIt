using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TetrisBlock : MonoBehaviour
{
    #region Fields

    public static int height = 200;
    public static int width = 100;
    public static Transform[,] grid;

    [SerializeField] private Vector3 rotationPoint;
    [SerializeField] private float fallTime = 0.8f;
    private readonly int[] _degrees = new[] {0, 90, 180, 270};

    private float _previousTime;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        var index = Mathf.RoundToInt(Random.Range(0, _degrees.Length));

        if (!gameObject.name.Contains("TutorialBlock"))
        {
            ChangeRotation(_degrees[index]);
        }
    }

    private void Update()
    {
        //Horizontal Movement
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
                BlockCollision();
            }
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
                BlockCollision();
            }
        }

        else if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeRotation(90);
            if (!ValidMove())
            {
                ChangeRotation(-90);
            }
        }

        //Vertical Movement

        if (!(Time.time - _previousTime > (Input.GetKey(KeyCode.S) ? fallTime / 10 : fallTime))) return;
        transform.position += new Vector3(0, -1, 0);
        _previousTime = Time.time;
        if (ValidMove()) return;
        transform.position -= new Vector3(0, -1, 0);
        BlockCollision();
    }

    #endregion
    
    #region Methods

    /**
     * Rotating the brick randomly at the start.
     */
    private void ChangeRotation(int degrees)
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), degrees);
    }


    /**
     * See if block collision happened on the grid.
     */
    private void BlockCollision()
    {
        AddToGrid(gameObject);
        enabled = false;
        FindObjectOfType<SpawnerScript>().AllowSpawn();
    }


    /**
     * Adding the block to the grid.
     */
    public static void AddToGrid(GameObject obj)
    {
        foreach (Transform children in obj.transform)
        {
            if (children.CompareTag("Shape")) continue;
            var childPos = children.transform.position;
            var roundX = Mathf.RoundToInt(childPos.x);
            var roundY = Mathf.RoundToInt(childPos.y);
            grid[roundX, roundY] = children;
        }
    }

    /**
     * Seeing the Restrictions Of movement the block has on the grid.
     */
    private bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            var childPos = children.transform.position;
            var roundX = Mathf.RoundToInt(childPos.x);
            var roundY = Mathf.RoundToInt(childPos.y);

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

    public static void ResetGrid()
    {
        grid = new Transform[width, height];
    }

    #endregion
}