using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TetrisBlock : MonoBehaviour
{
    //Vertical Movement
    private float _previousTime;
    public float fallTime = 0.8f;

    public static float height = 20;
    public static float width = 10;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Horizontal Movement
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
        }


        //Vertical Movement
        if (Time.time - _previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);

            _previousTime = Time.time;
        }
    }


    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            var roundX = Mathf.RoundToInt(children.transform.position.x);
            var roundY = Mathf.RoundToInt(children.transform.position.y);

            print(roundX);
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }
        }

        return true;
    }
}