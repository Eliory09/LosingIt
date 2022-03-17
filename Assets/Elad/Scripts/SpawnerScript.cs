using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject camera;
    private Vector2 originPosition;
    public GameObject[] TetrisBlocks;

     
    // Start is called before the first frame update
    void Start()
    {
        NewTetrisBlock();
    }
    

    // Update is called once per frame
    void Update()
    {
        var roundX = Mathf.RoundToInt(camera.transform.position.x);
        var roundY = Mathf.RoundToInt(camera.transform.position.y);
        var newPos =  new Vector3(roundX, roundY + 6, 0);
        transform.position = newPos;
    }

    public void NewTetrisBlock()
    { 
        Instantiate(TetrisBlocks[Random.Range(0, TetrisBlocks.Length)], transform.position, Quaternion.identity);
    }
}
