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
            LevelManager.LoadNextLevel();
            CheckpointsGenerator.GenerateNewPoint();
            Destroy(gameObject);
        }
    }
}