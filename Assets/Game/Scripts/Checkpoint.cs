using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class Checkpoint : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioClip magicAudioClip;
    private bool isActivated;


    #endregion

    #region MonoBehaviour

    
    public void AddCheckpointToGrid()
    {
        TetrisBlock.AddToGrid(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || !gameObject.CompareTag("Block")) return;
        if (isActivated) return;
        var pos = gameObject.transform.position;
        pos.z = -10;
        pos.x += 5 * Random.Range(-1, 1);
        CinemaMachineCamerasController.AddZoomCamera(pos, 0.3f);
        LevelManager.LoadNextLevel();
        CheckpointsGenerator.GenerateNewPoint();
        MusicManager.PlayEffect(magicAudioClip);
        isActivated = true;
    }

    #endregion
}