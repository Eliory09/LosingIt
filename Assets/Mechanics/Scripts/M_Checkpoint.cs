using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            M_CheckpointsGenerator.LevelUp();
            M_CheckpointsGenerator.GenerateNewPoint();
            Destroy(gameObject);
        }
    }
}
