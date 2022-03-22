using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricades : MonoBehaviour
{
    private bool _created = false;
    // Start is called before the first frame update
    private void Start()
    {
        // var position = transform.position;
        // var xPos = Mathf.RoundToInt(position.x);
        // var yPos = Mathf.RoundToInt(position.y);
        // TetrisBlock.grid[xPos, yPos ] = gameObject.transform;
    }

    private void Update()
    {
        if (!_created)
        {
            TetrisBlock.AddToGrid(gameObject);
        }
        // var position = transform.position;
        // var xPos = Mathf.RoundToInt(position.x);
        // var yPos = Mathf.RoundToInt(position.y);
        // TetrisBlock.grid[xPos, yPos ] = gameObject.transform;
    }
}
