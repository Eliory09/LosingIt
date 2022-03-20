using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnerScript : MonoBehaviour
{
    // private Vector2 originPosition;
    public GameObject[] tetrisBlocks;
    private bool isSpawnAllowed;


    private GameObject lastBlock;
    public float minSpaceBlockToSpawner = 0.1f;
    public float maxSpaceBlockToSpawner = 1.2f;
    public GameObject ball;

    [SerializeField] private int deleteLength = 10;


    public bool stopSpawn = false;

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
            var newPos = new Vector3(roundX, roundY, 10);
            transform.position = newPos;
        }


        if (isSpawnAllowed & EnoughSpace() & (!stopSpawn))
        {
            NewTetrisBlock();
        }

        if (lastBlock && BrickNotColliding())
        {
            Destroy(lastBlock);
            AllowSpawn();
        }

        RemoveFromGrid();
    }

    public void NewTetrisBlock()
    {
        if (IsValidToSpawn())
        {
            lastBlock = Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Length)], transform.position,
                Quaternion.identity);
            isSpawnAllowed = false;
        }
    }

    private bool BrickNotColliding()
    {
        var positionY = transform.position.y;
        var dLastBrickSpawner = positionY - lastBlock.transform.position.y;
        // var dBallSpawner = positionY - ball.transform.position.y;
        // return (dBallSpawner * maxSpaceBlockToSpawner <= dLastBrickSpawner);
        return ( maxSpaceBlockToSpawner <= dLastBrickSpawner);

    }

    /**
     * Checking if the space between the last brick to the Spawner is big enough.
     */
    private bool EnoughSpace()
    {
        if (lastBlock != null)
        {
            return transform.position.y - lastBlock.transform.position.y >= minSpaceBlockToSpawner;
        }

        return true;
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

    public void RemoveFromGrid()
    {
        var position = transform.position;
        var xPos = Mathf.RoundToInt(position.x);
        var yPos = Mathf.RoundToInt(position.y);

        if (yPos - deleteLength >= 0)
        {
            for (int j = 0; j < 30; j++)
            {
                if (TetrisBlock.grid[j + xPos - 15, yPos - deleteLength] != null)
                {
                    Destroy(TetrisBlock.grid[j + xPos - 15, yPos - deleteLength].gameObject);
                    TetrisBlock.grid[j + xPos - 15, yPos - deleteLength] = null;
                }
            } 
        }
       
    }
}