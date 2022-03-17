using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class M_CinemachineCamerasController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vmCamera;
    [SerializeField] private GameObject _camera;
    private static M_CinemachineCamerasController _shared;

    private void Awake()
    {
        _shared = this;
    }

    public static void AddCamera(Vector3 location)
    {
        location.z = -10;
        Instantiate(_shared._camera, location, quaternion.identity);
    }
}
