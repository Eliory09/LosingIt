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
    [SerializeField] private float widthRange;
    [Range(0.0f, 50.0f)]
    [SerializeField] private float heightRange;
    [SerializeField] private GameObject checkpointObj;
    
    private float _level;
    private Vector3 _currentPoint = Vector3.zero;
    private static CheckpointsGenerator _shared;

    private void Awake()
    {
        _shared = this;
    }

    private void Start()
    {
        GenerateNewPoint();
    }

    public static void LevelUp()
    {
        _shared._level++;
    }

    public static void GenerateNewPoint()
    {
        float x = Random.Range(-_shared.widthRange, _shared.widthRange);
        float y = Random.Range(_shared.heightRange * _shared._level, _shared.heightRange * (_shared._level + 1));
        _shared._currentPoint = new Vector3(x, y, 0);
        GameObject checkpointGameObject = Instantiate(_shared.checkpointObj);
        checkpointGameObject.transform.position = _shared._currentPoint;
        CinemachineCamerasController.AddCamera(checkpointGameObject.transform.position);
    }
}
