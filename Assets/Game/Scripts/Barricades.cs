using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricades : MonoBehaviour
{
    private bool _created = false;
    private bool enterGrid = false;
    [SerializeField] private float dToGrid = 10;

    [SerializeField] private GameObject spawner;


    private float timer = 0; 

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
            // enterGrid = SpawnerScript.DistanceFromSpawner(gameObject, dToGrid);
            // if (enterGrid)
            // {
            //     
            //     gameObject.layer = 6;
                TetrisBlock.AddToGrid(gameObject);
                _created = true;
                // }
        }
     
    }

    
}