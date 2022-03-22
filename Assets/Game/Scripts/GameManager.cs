using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEditor.SearchService.Scene;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BallMovement ball;
    [SerializeField] private AudioClip startMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private GameObject orb;
    [SerializeField] private Vector3 orbInitialLocation;
    [SerializeField] private SpawnerScript spawner;
    [SerializeField] private GameObject initialPlatform;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Vector3 initialPlatformLocation;
    [SerializeField] private Canvas transitions;
    private static GameManager _shared;

    private void Awake()
    {
        _shared = this;
    }

    private void Start()
    {
        InitializeGame();
    }

    public static void InitializeGame()
    {
        MusicManager.SetLoop(true);
        MusicManager.ChangeMusic(_shared.startMusic);
        _shared.spawner.DisableSpawn();
        TetrisBlock.ResetGrid();
        _shared.initialPlatform.GetComponent<Platform>().AddToGrid();
        _shared.ball.ResetBall();
        Instantiate(_shared.orb, _shared.orbInitialLocation, Quaternion.identity);
    }

    public static void ActivateRoundLoss()
    {
        _shared.transitions.GetComponent<Animator>().SetTrigger("toFadeOut");
    }
    
    public static void ResetGame()
    {
        var objs = GameObject.FindGameObjectsWithTag("Block");
        foreach (var o in objs) Destroy(o);
        
        objs = GameObject.FindGameObjectsWithTag("Barricades");
        foreach (var o in objs) Destroy(o);
        

        MusicManager.ChangeMusic(_shared.startMusic);
        _shared.spawner.DisableSpawn();
        TetrisBlock.ResetGrid();
        
        var platform = Instantiate(_shared.platformPrefab, _shared.initialPlatformLocation, Quaternion.identity);
        platform.GetComponent<Platform>().AddToGrid();
        CheckpointsGenerator.RemoveCheckpoint();
        CinemachineCamerasController.ResetCameras();
        
        _shared.ball.ResetBall();
        Instantiate(_shared.orb, _shared.orbInitialLocation, Quaternion.identity);
    }

    public static void ActivateTetrisSequence()
    {
        _shared.spawner.AllowSpawn();
        CheckpointsGenerator.GenerateNewPoint();
        MusicManager.ChangeMusic(_shared.gameMusic);
    }
}
