using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CinemachineCamerasController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vmCamera;
    private static CinemachineCamerasController _shared;

    private void Awake()
    {
        _shared = this;
    }

    public static void AddCamera(Vector3 location)
    {
        location.z = -10;
        Instantiate(_shared.vmCamera, location, quaternion.identity);
    }
}
