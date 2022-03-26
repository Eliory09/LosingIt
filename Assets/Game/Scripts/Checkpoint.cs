using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioClip magicAudioClip;

    #endregion

    #region MonoBehaviour

    public void AddCheckpointToGrid()
    {
        TetrisBlock.AddToGrid(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || !gameObject.CompareTag("Block")) return;
        var pos = gameObject.transform.position;
        pos.z = -10;
        CinemaMachineCamerasController.AddZoomCamera(pos, 0.3f);
        LevelManager.LoadNextLevel();
        CheckpointsGenerator.GenerateNewPoint();
        MusicManager.PlayEffect(magicAudioClip);
    }

    #endregion
}