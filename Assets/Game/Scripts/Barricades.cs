using UnityEngine;

public class Barricades : MonoBehaviour
{
    #region Fields

    private bool _created;

    #endregion

    #region MonoBehaviour

    private void Update()
    {
        if (_created) return;
        TetrisBlock.AddToGrid(gameObject);
        _created = true;
    }

    #endregion
}