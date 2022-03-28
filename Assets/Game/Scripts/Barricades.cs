using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricades : MonoBehaviour
{
    #region Fields

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || !GameManager.IsResetAllowed()) return;
        GameManager.ActivateRoundLoss();
    }

    #endregion
}