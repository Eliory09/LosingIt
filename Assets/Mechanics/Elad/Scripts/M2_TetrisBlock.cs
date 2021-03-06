using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class M2_TetrisBlock : MonoBehaviour
{
    
    
    
    //Vertical Movement
    private float _previousTime;
    public float fallTime = 0.8f;

    //Restrictions Of movement if needed 
    public static int height = 100;
    public static int width = 100;
    public static Transform[,] grid = new Transform[width,height];
    

    //Rotation 
    public Vector3 rotationPoint;

    // Update is called once per frame
    void Update()
    {
        //Horizontal Movement
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);

            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

            }
        }
        
        //Vertical Movement
        
        if (Time.time - _previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            _previousTime = Time.time;
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                this.enabled = false;
                FindObjectOfType<M1_SpawnerScript>().NewTetrisBlock();
            }
        }
    }
    void AddToGrid()
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
    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            var childPos = children.transform.position;
            var roundX = Mathf.RoundToInt(childPos.x);
            var roundY = Mathf.RoundToInt(childPos.y);

            // if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height) 
            if ( roundY < 0 )
            {
                return false;
            }

            if (grid[roundX,roundY] != null)
            {
                return false;
            }
        }

        return true;
    }
}