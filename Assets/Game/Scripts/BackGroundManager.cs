using Unity.Mathematics;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform backGroundMeasurement;
    [SerializeField] private GameObject[] backGrounds;
    private static GameObject _currentBackground;

    private static float _backGroundHeight;
    private bool _counter = true;

    private static BackGroundManager _shared;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _shared = this;
        _currentBackground = backGrounds[0];
        _backGroundHeight = backGroundMeasurement.position.y - _currentBackground.transform.position.y;
    }

    #endregion

    #region Methods

    /**
    * Making a new higher background.
    */
    private void HigherBackGround()
    {
        //Calculating the new background position
        var oldPos = _currentBackground.transform.position;
        var newPos = new Vector3(oldPos.x, oldPos.y + _backGroundHeight, oldPos.z);

        var obj = Instantiate(backGrounds[1], newPos, quaternion.identity);

        //Every second time flipping the background to be continuously with the last background color.
        if (_counter)
        {
            obj.transform.Rotate(180, 0, 0);
        }

        _counter = !_counter;

        obj.transform.SetParent(GameManager.shared.cloneFather.transform);
        _currentBackground = obj;
    }

    /**
    * Called from outside the class.
    */
    public static void ChangeBackGround()
    {
        _shared.HigherBackGround();
    }

    #endregion
}