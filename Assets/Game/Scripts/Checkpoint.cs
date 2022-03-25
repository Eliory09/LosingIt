using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private AudioClip magicAudioClip;
    public void AddCheckpointToGrid()
    {
        TetrisBlock.AddToGrid(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Block"))
        {
            Vector3 pos = gameObject.transform.position;
            pos.z = -10;
            CinemachineCamerasController.AddZoomCamera(pos, 0.3f);
            LevelManager.LoadNextLevel();
            CheckpointsGenerator.GenerateNewPoint();
            MusicManager.PlayEffect(magicAudioClip);
        }
    }
}