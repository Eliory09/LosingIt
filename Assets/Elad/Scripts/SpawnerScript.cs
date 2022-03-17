using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] TetrisBlocks;
    // Start is called before the first frame update
    void Start()
    {
        NewTetrisBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewTetrisBlock()
    { 
        Instantiate(TetrisBlocks[Random.Range(0, TetrisBlocks.Length)], transform.position, Quaternion.identity);
    }
}
