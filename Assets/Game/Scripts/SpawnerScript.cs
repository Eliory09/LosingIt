using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnerScript : MonoBehaviour
{
    // private Vector2 originPosition;
    public GameObject[] tetrisBlocks;
    private bool isSpawnAllowed;

    void Start()
    {
        NewTetrisBlock();
    }
    
    void Update()
    {
        if (Camera.main is { })
        {
            var pos = Camera.main.transform.position;
            var roundX = Mathf.RoundToInt(pos.x);
            var roundY = Mathf.RoundToInt(pos.y) + 6f;
            var newPos =  new Vector3(roundX, roundY, 10);
            transform.position = newPos;
        }

        if (isSpawnAllowed)
        {
            NewTetrisBlock();
        }
    }

    public void NewTetrisBlock()
    {
        if (IsValidToSpawn())
        {
            Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Length)], transform.position, Quaternion.identity);
            isSpawnAllowed = false;
        }
    }

    public void AllowSpawn()
    {
        isSpawnAllowed = true;
    }

    private bool IsValidToSpawn()
    {
        var position = transform.position;
        var xPos = Mathf.RoundToInt(position.x);
        var yPos = Mathf.RoundToInt(position.y - 3);
        for (int i = xPos - 9; i <= xPos + 9; i++)
        {
            if (TetrisBlock.grid[xPos, yPos])
            {
                return false;
            }
        }

        return true;
    }
}
