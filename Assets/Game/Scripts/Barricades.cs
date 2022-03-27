using UnityEngine;

public class Barricades : MonoBehaviour
{
    #region Fields

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Ball")
        {
            GameManager.ResetGame();
        }
    }

    #endregion
}