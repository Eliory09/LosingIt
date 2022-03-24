using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject father;

    // private Vector2 originPosition;
    public List<GameObject> tetrisBlocks;
    [SerializeField] private GameObject[] backGrounds;
    private GameObject _currentBackground;
    [SerializeField] private float backGroundHeight = 120f;
    public bool isSpawnAllowed;

    private GameObject lastBlock;
    public float minSpaceBlockToSpawner = 0.1f;
    public float maxSpaceBlockToSpawnerY = 1.2f;
    public float maxSpaceBlockToSpawnerX = 1.2f;
    public GameObject ball;

    [SerializeField] private int cameraDistance;
    [SerializeField] private float gridMaxCapacity = 0.75f;
    [SerializeField] private int gridExpand = 2;


    [SerializeField] private int xChangeMax = 5;
    private bool _firstSpawn = true;
    private float _originalX;

    [SerializeField] private int deleteLength = 10;
    public bool stopSpawn;

    private static float currentYPose;

    private void Awake()
    {
        _originalX = transform.position.x;
        _currentBackground = backGrounds[0];
    }

    void Update()
    {
        if (Camera.main is { })
        {
            var pos = Camera.main.transform.position;
            var roundX = Mathf.RoundToInt(pos.x);
            var roundY = Mathf.RoundToInt(pos.y) + cameraDistance;
            var newPos = new Vector3(roundX, roundY, 10);
            transform.position = newPos;
        }


        if (isSpawnAllowed && EnoughSpaceToSpawn() && !stopSpawn)
        {
            isSpawnAllowed = false;
            NewTetrisBlock();
        }

        if (lastBlock && BrickNotColliding())
        {
            Destroy(lastBlock);
            AllowSpawn();
        }

        RemoveFromGrid();
        MakeGridBigger();
        currentYPose = transform.position.y;
    }

    private void MakeGridBigger()
    {
        if (lastBlock)
        {
            if (lastBlock.transform.position.y >= gridMaxCapacity * TetrisBlock.height)
            {
                var newG = new Transform[TetrisBlock.width, TetrisBlock.height * gridExpand];
                for (int i = 0; i < TetrisBlock.width; i++)
                {
                    for (int j = 0; j < TetrisBlock.height; j++)
                    {
                        newG[i, j] = TetrisBlock.grid[i, j];
                    }
                }

                HigherBackGround();
                TetrisBlock.grid = newG;
                TetrisBlock.height *= gridExpand;
            }
        }
    }

    private void HigherBackGround()
    {
        var oldPos = _currentBackground.transform.position;
        var newPos = new Vector3(oldPos.x, oldPos.y + backGroundHeight, oldPos.z);
        var obj = Instantiate(backGrounds[1], newPos, quaternion.identity);
        obj.transform.SetParent(father.transform);
        _currentBackground = obj;
    }

    public void NewTetrisBlock()
    {
        // if (IsValidToSpawn())
        // {

        var newPos = transform.position;
            newPos.x = Mathf.RoundToInt(Random.Range(newPos.x - xChangeMax, newPos.x + xChangeMax));  
        if (_firstSpawn)
        {
            newPos.x = _originalX;
            _firstSpawn = false;
        }

        while (!(IsValidToSpawn(newPos.x, newPos.y)))
        {
            newPos.x = Mathf.RoundToInt(Random.Range(newPos.x - xChangeMax, newPos.x + xChangeMax));  
        }

        lastBlock = Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Count)], newPos,
            Quaternion.identity) as GameObject;
        lastBlock.transform.SetParent(father.transform);
        Destroy(lastBlock.gameObject, 30);

        // }
    }

    private bool BrickNotColliding()
    {
        var position = transform.position;
        var positionY = position.y;
        var positionX = position.x;

        var positionlastBrick = lastBlock.transform.position;
        var positionlastBrickY = positionlastBrick.y;
        var positionlastBrickX = positionlastBrick.x;

        var dLastBrickSpawnerY = positionY - positionlastBrickY;
        var dLastBrickSpawnerX = math.abs(positionX - positionlastBrickX);
        // var dBallSpawner = positionY - ball.transform.position.y;
        // return (dBallSpawner * maxSpaceBlockToSpawner <= dLastBrickSpawner);
        var retVal = maxSpaceBlockToSpawnerY <= dLastBrickSpawnerY ||
                     maxSpaceBlockToSpawnerX <= dLastBrickSpawnerX;
        return retVal;
    }

    /**
     * Checking if the space between the last brick to the Spawner is big enough.
     */
    private bool EnoughSpaceToSpawn()
    {
        if (lastBlock != null)
        {
            return transform.position.y - lastBlock.transform.position.y >= minSpaceBlockToSpawner;
        }

        return true;
    }


    public void AllowSpawn()
    {
        // BarricadeGenerator.generate = true;
        isSpawnAllowed = true;
        stopSpawn = false;
        // NewTetrisBlock();
    }

    public void DisableSpawn()
    {
        isSpawnAllowed = false;
        stopSpawn = true;
    }

    private bool IsValidToSpawn(float xPos, float yPos)
    {

        var x = Mathf.RoundToInt(xPos);
        var y = Mathf.RoundToInt(yPos);
        for (var i = x - 4; i <= x + 4; i++)
        {
            for (var j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && j >= 0)
                {
                    // print(i);
                    // print(j);
                    if (TetrisBlock.grid[i, j])
                    {
                        return false;
                    }
                }
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
            for (int j = 0; j < 16; j++)
            {
                if (TetrisBlock.grid[j + xPos - 8, yPos - deleteLength] != null)
                {
                    Destroy(TetrisBlock.grid[j + xPos - 8, yPos - deleteLength].gameObject);
                    TetrisBlock.grid[j + xPos - 8, yPos - deleteLength] = null;
                }
            }
        }
    }

    public void ChangeCameraDistance(int distance)
    {
        cameraDistance = distance;
    }

    public static bool DistanceFromSpawner(GameObject obj, float requiredD)
    {
        if (obj.CompareTag("Barricades"))
        {
            if ((obj.transform.position.y <= currentYPose - requiredD))
            {
                return true;
            }
        }

        return false;
    }
}