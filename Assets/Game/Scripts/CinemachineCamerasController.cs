using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CinemachineCamerasController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain main;
    [SerializeField] private CinemachineVirtualCamera vmCamera;
    private static CinemachineCamerasController _shared;
    private List<CinemachineVirtualCamera> vCams = new List<CinemachineVirtualCamera>();

    private void Awake()
    {
        _shared = this;
    }

    public static void AddCamera(Vector3 location)
    {
        location.z = -10;
        var cam = Instantiate(_shared.vmCamera, location, quaternion.identity);
        _shared.vCams.Add(cam);
    }

    public static void ResetCameras()
    {
        foreach (var cinemachineVirtualCamera in _shared.vCams)
        {
            if (cinemachineVirtualCamera != null)
                Destroy(cinemachineVirtualCamera.gameObject);
        }
    }
}
