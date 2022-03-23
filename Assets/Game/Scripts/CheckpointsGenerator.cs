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
    
    private Vector3 _currentPoint = Vector3.zero;
    private static CheckpointsGenerator _shared;

    private void Awake()
    {
        
        _shared = this;
    }

    public static void GenerateNewPoint()
    {
        
        var delta = 90;
        var theta = Mathf.Deg2Rad * (Random.Range(-_shared.angleDelta, _shared.angleDelta) + delta);
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
        CinemachineCamerasController.AddCamera(_shared.currentCheckpoint.transform.position);
    }

    public static void RemoveCheckpoint()
    {
        
        if(_shared.currentCheckpoint != null)
            Destroy(_shared.currentCheckpoint.gameObject);
        
    }
}
