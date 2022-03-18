using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    // private Vector2 originPosition;
    public GameObject[] TetrisBlocks;

    void Start()
    {
        NewTetrisBlock();
    }
    
    void Update()
    {
        // var camPosition = camera.transform.position;
        // var roundX = Mathf.RoundToInt(camPosition.x);
        // var roundY = Mathf.RoundToInt(camPosition.y);
        // var newPos =  new Vector3(roundX, roundY + 6, 0);
        // transform.position = newPos;
        if (Camera.main is { })
        {
            var pos = Camera.main.transform.position;
            var roundX = Mathf.RoundToInt(pos.x);
            var roundY = Mathf.RoundToInt(pos.y) + 6;
            var newPos =  new Vector3(roundX, roundY, 10);
            transform.position = newPos;
        }
    }

    public void NewTetrisBlock()
    { 
        Instantiate(TetrisBlocks[Random.Range(0, TetrisBlocks.Length)], transform.position, Quaternion.identity);
    }
}
