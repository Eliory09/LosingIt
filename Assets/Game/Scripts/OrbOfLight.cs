using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbOfLight : MonoBehaviour
{
    [SerializeField] private AudioClip magicAudioClip;
    private static readonly int Collided = Animator.StringToHash("Collided");

    private void OnTriggerEnter2D(Collider2D other)
    {
        var animator = GetComponent<Animator>();
        animator.SetTrigger(Collided);
        GameManager.ActivateTetrisSequence();
        // MusicManager.PlayEffect(magicAudioClip);
        Destroy(gameObject, 0.25f);
    }
}
