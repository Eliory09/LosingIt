using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.WSA;
using Vector3 = UnityEngine.Vector3;

public class CinemachineCamerasController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain main;
    [SerializeField] private CinemachineVirtualCamera vmCamera;
    [SerializeField] private CinemachineVirtualCamera zoomoutCamera;
    [SerializeField] private CinemachineVirtualCamera initial2DCamera;
    [SerializeField] private float tiltEffectSpeed;
    [SerializeField] private CinemachineBlenderSettings cameraBlends; 
    [SerializeField] private float initialCamerasTransitionDuration = 30f;


    private static CinemachineCamerasController _shared;
    private List<CinemachineVirtualCamera> vCams = new List<CinemachineVirtualCamera>();
    private CinemachineVirtualCamera _currentCam;
    private CinemachineVirtualCamera _currentZoomCam;
    private bool _tiltActivated;
    private float _currentTiltValue;

    private void Awake()
    {
        _shared = this;
        _shared.vCams.Add(initial2DCamera);
        _currentCam = initial2DCamera;
        SetCameraTransitionsDuration(_shared.initialCamerasTransitionDuration);
    }

    private void Update()
    {
        if (!_tiltActivated) return;
        _currentTiltValue = 
            Mathf.Cos(Time.time) * 0.5f * _shared.tiltEffectSpeed * (Vector3.Distance(
                _shared.main.gameObject.transform.position,
                _shared._currentCam.gameObject.transform.position) + 1);
        _shared._currentCam.gameObject.transform.eulerAngles = new Vector3(0, 0, _currentTiltValue);
    }

    public static void AddCamera(Vector3 location, int orthographicSize = 7)
    {
        location.z = -10;
        var cam = Instantiate(_shared.vmCamera, location, quaternion.identity);
        cam.gameObject.name = "2D Camera";
        cam.m_Lens.OrthographicSize = orthographicSize;
        _shared._currentCam = cam;
        _shared.vCams.Add(cam);
    }

    public static void ResetCameras()
    {
        for (int i = 1; i < _shared.vCams.Count; i++)
        {
            var cinemachineVirtualCamera = _shared.vCams[i];
            if (cinemachineVirtualCamera != null) 
                Destroy(cinemachineVirtualCamera.gameObject);
        }
        _shared._currentCam = _shared.vCams[0];
        SetCameraTransitionsDuration(_shared.initialCamerasTransitionDuration);
    }
    
    public static void AddZoomCamera(Vector3 position, float duration)
    {
        _shared.StartCoroutine(_shared.SetupZoom(position, duration));
    }

    private IEnumerator SetupZoom(Vector3 position, float duration)
    {
        var zoCam = Instantiate(_shared.zoomoutCamera, position, quaternion.identity);
        zoCam.gameObject.name = "ZoomoutCamera";
        GameManager.UpdateSpawnerPosition((int) zoCam.m_Lens.OrthographicSize);
        _shared._currentZoomCam = zoCam;
        _shared.vCams.Add(zoCam);
        yield return new WaitForSeconds(duration);
        zoCam.Priority = 0;
    }

    public static void ActivateCameraTilt()
    {
        _shared._tiltActivated = true;
    }

    public static void DisableCameraTilt()
    {
        _shared._tiltActivated = false;
    }

    public static void SetCameraTransitionsDuration(float transitionDuration)
    {
        for (int i = 1; i < _shared.cameraBlends.m_CustomBlends.Length; i++)
        {
            _shared.cameraBlends.m_CustomBlends[i].m_Blend.m_Time = transitionDuration;
        }

        _shared.main.m_DefaultBlend.m_Time = transitionDuration;
    }

    public static float GetCameraTransitionsDuration()
    {
        return _shared.cameraBlends.m_CustomBlends[1].m_Blend.m_Time;
    }
}
