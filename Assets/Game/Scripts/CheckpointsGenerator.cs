using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class CheckpointsGenerator : MonoBehaviour
{
    #region Fields

    [Range(0.0f, 50.0f)] [SerializeField] private float radius;
    [SerializeField] private float angleDelta;
    [SerializeField] private GameObject checkpointObj;
    [SerializeField] private GameObject ball;

    private static CheckpointsGenerator _shared;
    private GameObject _currentCheckpoint;
    private Vector3 _currentPoint = Vector3.zero;
    private int _direction;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _shared = this;
        _shared._direction = 1;
    }

    #endregion

    #region Methods

    public static void ChangeRadius(float radius)
    {
        _shared.radius = radius;
    }

    /**
     * Generating a new check point.
     */
    public static void GenerateNewPoint()
    {
        const int delta = 90;
        float minDelta, maxDelta;

        if (_shared._direction == 1)
        {
            minDelta = 0;
            maxDelta = _shared.angleDelta;
        }
        else
        {
            minDelta = -_shared.angleDelta;
            maxDelta = 0;
        }

        var theta = Mathf.Deg2Rad * (Random.Range(minDelta, maxDelta) + delta);
        _shared._currentPoint =
            new Vector3(
                (float) (_shared.radius * Math.Cos(theta)),
                (float) (_shared.radius * Math.Sin(theta)),
                0);
        _shared._currentPoint += _shared.ball.transform.position;
        _shared._currentPoint.x = Mathf.RoundToInt(_shared._currentPoint.x);
        _shared._currentPoint.y = Mathf.RoundToInt(_shared._currentPoint.y);
        _shared._currentCheckpoint = Instantiate(_shared.checkpointObj);
        _shared._currentCheckpoint.transform.position = _shared._currentPoint;
        _shared._currentCheckpoint.GetComponent<Checkpoint>().AddCheckpointToGrid();
        _shared._direction *= -1;
        CinemaMachineCamerasController.AddCamera(_shared._currentCheckpoint.transform.position);
    }

    #endregion
}