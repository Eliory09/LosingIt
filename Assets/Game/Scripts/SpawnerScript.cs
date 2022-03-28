using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerScript : MonoBehaviour
{
    #region Fields

    [SerializeField] public List<GameObject> tetrisBlocks;
    [SerializeField] private GameObject tutorialBlock;
    [SerializeField] private int cameraDistance;
    [SerializeField] private float gridMaxCapacity = 0.75f;
    [SerializeField] private int gridExpand = 2;
    [SerializeField] private float minSpaceBlockToSpawner = 0.1f;
    [SerializeField] private float maxSpaceBlockToSpawnerY = 1.2f;
    [SerializeField] private float maxSpaceBlockToSpawnerX = 1.2f;
    [SerializeField] private int deleteLength = 10;
    [SerializeField] private int xChangeMax = 5;
    [SerializeField] private float maxTimeToSpawn = 8f;


    public bool firstSpawn = true;
    public bool isSpawnAllowed;
    public bool stopSpawn;

    private GameObject _lastBlock;
    private bool _isTutorialOn;
    private float _originalX;
    private int _tutorialBlocksSpawned;
    private float _timer;

    
    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _originalX = transform.position.x;
    }

    private void Update()
    {
        if (Camera.main is { })
        {
            var pos = Camera.main.transform.position;
            var roundX = Mathf.RoundToInt(pos.x);
            var roundY = Mathf.RoundToInt(pos.y) + cameraDistance;
            var newPos = new Vector3(roundX, roundY, 10);
            transform.position = newPos;
        }


        if (isSpawnAllowed && !stopSpawn)
        {

            if (EnoughSpaceToSpawn() || _timer >= maxTimeToSpawn)
            {
                _timer = 0;
                isSpawnAllowed = false;
                NewTetrisBlock(); 
            }

            else
            {
                _timer += Time.deltaTime;
            }
        }

        if (_lastBlock && BrickNotColliding())
        {
            Destroy(_lastBlock);
            AllowSpawn();
        }

        RemoveFromGrid();
        MakeGridBigger();
    }

    #endregion

    #region Methods

    public void ActivateTutorialState()
    {
        _isTutorialOn = true;
        _tutorialBlocksSpawned = 0;
    }

    /**
     * Responsible for making the grid bigger as we go up the game.
     */
    private void MakeGridBigger()
    {
        if (!_lastBlock) return;
        if (!(_lastBlock.transform.position.y >= gridMaxCapacity * TetrisBlock.height)) return;
        var newG = new Transform[TetrisBlock.width, TetrisBlock.height * gridExpand];
        for (var i = 0; i < TetrisBlock.width; i++)
        {
            for (var j = 0; j < TetrisBlock.height; j++)
            {
                newG[i, j] = TetrisBlock.grid[i, j];
            }
        }

        BackGroundManager.ChangeBackGround();
        TetrisBlock.grid = newG;
        TetrisBlock.height *= gridExpand;
    }


    /**
     * Instantiating a new Tetris block.
     */
    private void NewTetrisBlock()
    {
        var newPos = transform.position;
        newPos.x = Mathf.RoundToInt(Random.Range(newPos.x - xChangeMax, newPos.x + xChangeMax));
        if (firstSpawn)
        {
            newPos.x = _originalX;
            firstSpawn = false;
        }

        while (!(IsValidToSpawn(newPos.x, newPos.y)))
        {
            newPos.x = Mathf.RoundToInt(Random.Range(newPos.x - xChangeMax, newPos.x + xChangeMax));
        }

        GameObject tetrisBlock;

        if (_isTutorialOn)
        {
            tetrisBlock = tutorialBlock;
            _tutorialBlocksSpawned++;
            if (_tutorialBlocksSpawned == 1)
            {
                _isTutorialOn = false;
                _tutorialBlocksSpawned = 0;
            }
        }
        else
        {
            tetrisBlock = tetrisBlocks[Random.Range(0, tetrisBlocks.Count)];
        }

        _lastBlock = Instantiate(tetrisBlock, newPos, Quaternion.identity);


        _lastBlock.transform.SetParent(GameManager.shared.cloneFather.transform);
        // Destroy(_lastBlock.gameObject, 30);
    }

    /**
     *  See if the current brick is not going to collide.
     *  Moved out of the screen.
     */
    private bool BrickNotColliding()
    {
        var position = transform.position;
        var positionY = position.y;
        var positionX = position.x;

        var lastPositionBrick = _lastBlock.transform.position;
        var lastPositionBrickY = lastPositionBrick.y;
        var lastPositionBrickX = lastPositionBrick.x;

        var dLastBrickSpawnerY = positionY - lastPositionBrickY;
        var dLastBrickSpawnerX = math.abs(positionX - lastPositionBrickX);
        var retVal = maxSpaceBlockToSpawnerY <= dLastBrickSpawnerY ||
                     maxSpaceBlockToSpawnerX <= dLastBrickSpawnerX;
        return retVal;
    }

    /**
     * Checking if the space between the last brick to the Spawner is big enough.
     */
    private bool EnoughSpaceToSpawn()
    {
        if (_lastBlock != null)
        {
            return transform.position.y - _lastBlock.transform.position.y >= minSpaceBlockToSpawner;
        }

        return true;
    }


    public void AllowSpawn()
    {
        isSpawnAllowed = true;
        stopSpawn = false;
    }

    public void DisableSpawn()
    {
        isSpawnAllowed = false;
        stopSpawn = true;
    }

    /**
     * Decides if its ok to spawn.
     * Depends if the upper screen grid is clear enough. 
     */
    private bool IsValidToSpawn(float xPos, float yPos)
    {
        var x = Mathf.RoundToInt(xPos);
        var y = Mathf.RoundToInt(yPos);
        for (var i = x - 4; i <= x + 4; i++)
        {
            for (var j = y - 1; j <= y + 1; j++)
            {
                if (i < 0 || j < 0) continue;
                if (TetrisBlock.grid[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void RemoveFromGrid()
    {
        var position = transform.position;
        var xPos = Mathf.RoundToInt(position.x);
        var yPos = Mathf.RoundToInt(position.y);

        if (yPos - deleteLength < 0) return;
        for (var j = 0; j < 16; j++)
        {
            if (TetrisBlock.grid[j + xPos - 8, yPos - deleteLength] == null) continue;
            if (TetrisBlock.grid[j + xPos - 8, yPos - deleteLength].CompareTag("InitialPlatform")) continue;
            Destroy(TetrisBlock.grid[j + xPos - 8, yPos - deleteLength].gameObject);
            TetrisBlock.grid[j + xPos - 8, yPos - deleteLength] = null;
        }
    }

    public void ChangeCameraDistance(int distance)
    {
        cameraDistance = distance;
    }

    #endregion
}