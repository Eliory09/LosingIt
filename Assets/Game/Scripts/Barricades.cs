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
        if (!other.gameObject.CompareTag("Player")) return;
        StartCoroutine(ResetCooldown(1));
        GameManager.ActivateRoundLoss();
    }
    
    private IEnumerator ResetCooldown(float time)
    {
        yield return new WaitForSeconds(time);
    }

    #endregion
}