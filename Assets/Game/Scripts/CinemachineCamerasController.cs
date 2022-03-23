using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CinemachineCamerasController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain main;
    [SerializeField] private CinemachineVirtualCamera vmCamera;
    [SerializeField] private CinemachineVirtualCamera zoomoutCamera;
    private static CinemachineCamerasController _shared;
    private List<CinemachineVirtualCamera> vCams = new List<CinemachineVirtualCamera>();

    private void Awake()
    {
        _shared = this;
    }

    public static void AddCamera(Vector3 location, int orthographicSize = 7)
    {
        location.z = -10;
        var cam = Instantiate(_shared.vmCamera, location, quaternion.identity);
        cam.gameObject.name = "2D Camera";
        cam.m_Lens.OrthographicSize = orthographicSize;

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
    
    public static void AddZoomoutCamera(Vector3 position, float duration)
    {
        _shared.StartCoroutine(_shared.SetupZoomout(position, duration));
    }

    private IEnumerator SetupZoomout(Vector3 position, float duration)
    {
        var zoCam = Instantiate(_shared.zoomoutCamera, position, quaternion.identity);
        zoCam.gameObject.name = "ZoomoutCamera";
        GameManager.UpdateSpawnerPosition((int) zoCam.m_Lens.OrthographicSize);
        _shared.vCams.Add(zoCam);
        yield return new WaitForSeconds(duration);
        zoCam.Priority = 0;
    }
}
