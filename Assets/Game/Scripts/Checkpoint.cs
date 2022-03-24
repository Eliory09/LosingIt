using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void AddCheckpointToGrid()
    {
        TetrisBlock.AddToGrid(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Block"))
        {
            LevelManager.LoadNextLevel();
            CheckpointsGenerator.GenerateNewPoint();
            // Destroy(gameObject);
        }
    }
}