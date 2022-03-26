using UnityEngine;

public class Fader : MonoBehaviour
{
    #region Fields

    private static readonly int ToFadeIn = Animator.StringToHash("toFadeIn");

    #endregion

    #region Methods

    public void OnFadeComplete()
    {
        GameManager.ResetGame();
        GetComponent<Animator>().SetTrigger(ToFadeIn);
    }

    #endregion
}