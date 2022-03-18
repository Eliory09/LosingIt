using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CheckpointsGenerator.LevelUp();
            CheckpointsGenerator.GenerateNewPoint();
            Destroy(gameObject);
        }
    }
}
