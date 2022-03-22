using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public void OnFadeComplete()
    {
        GameManager.ResetGame();
        GetComponent<Animator>().SetTrigger("toFadeIn");
    }
}
