using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class CheckpointsGenerator : MonoBehaviour
{
    [Range(0.0f, 50.0f)]
    [SerializeField] private float radius;
    [SerializeField] private float angleDelta;
    [SerializeField] private GameObject checkpointObj;
    [SerializeField] private GameObject ball;
    private GameObject currentCheckpoint;
    private int direction;
    
    private Vector3 _currentPoint = Vector3.zero;
    private static CheckpointsGenerator _shared;

    private void Awake()
    {
        _shared = this;
        _shared.direction = 1;
    }

    public static void ChangeRadius(float radius)
    {
        _shared.radius = radius;
    }

    public static void GenerateNewPoint()
    {
        var delta = 90;
        float minDelta, maxDelta;
        
        if (_shared.direction == 1)
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
        _shared.currentCheckpoint = Instantiate(_shared.checkpointObj);
        _shared.currentCheckpoint.transform.position = _shared._currentPoint;
        _shared.currentCheckpoint.GetComponent<Checkpoint>().AddCheckpointToGrid();
        _shared.direction *= -1;
        CinemachineCamerasController.AddCamera(_shared.currentCheckpoint.transform.position);
    }
}
