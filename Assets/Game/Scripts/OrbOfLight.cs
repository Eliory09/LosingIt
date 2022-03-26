using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbOfLight : MonoBehaviour
{
    #region Fields

    private static readonly int Collided = Animator.StringToHash("Collided");

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D other)
    {
        var animator = GetComponent<Animator>();
        animator.SetTrigger(Collided);
        GameManager.ActivateTetrisSequence();
        Destroy(gameObject, 0.25f);
    }

    #endregion
}