using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private BallMovement ball;
    [SerializeField] private AudioClip startMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private GameObject orb;
    [SerializeField] private Vector3 orbInitialLocation;
    [SerializeField] private SpawnerScript spawner;
    [SerializeField] private GameObject initialPlatform;
    [SerializeField] private Canvas transitions;
    [SerializeField] private Vector3 zoomoutCameraInitialLocation;
    [SerializeField] private GameObject LoseAnimationBG;

    private bool _isSpawnAllowed = true;
    private GameObject _loseBg;
    public GameObject cloneFather;
    public static GameManager shared;
    private static readonly int ToFadeOut = Animator.StringToHash("toFadeOut");

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        shared = this;
    }

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    #endregion

    #region Methods

    private static void InitializeGame()
    {
        shared.spawner.firstSpawn = true;
        MusicManager.SetLoop(true);
        MusicManager.ChangeMusic(shared.startMusic);
        shared.spawner.DisableSpawn();
        TetrisBlock.ResetGrid();
        shared.initialPlatform.GetComponent<Checkpoint>().AddCheckpointToGrid();
        shared.ball.ResetBall();
        Instantiate(shared.orb, shared.orbInitialLocation, Quaternion.identity);
        LevelManager.ResetLevel();
    }

    public static void ActivateRoundLoss()
    {
        shared.StartCoroutine(ResetCooldown(4f));
        shared.StartCoroutine(LoseAnimation());
    }

    public static bool IsResetAllowed()
    {
        return shared._isSpawnAllowed;
    }

    public static void ResetGame()
    {
        var objs = GameObject.FindGameObjectsWithTag("Block");
        foreach (var o in objs) Destroy(o);

        objs = GameObject.FindGameObjectsWithTag("Barricades");
        foreach (var o in objs) Destroy(o);

        objs = GameObject.FindGameObjectsWithTag("Orb");
        foreach (var o in objs) Destroy(o);


        MusicManager.ChangeMusic(shared.startMusic);
        shared.spawner.DisableSpawn();
        TetrisBlock.ResetGrid();
        LevelManager.ResetLevel();

        shared.initialPlatform.GetComponent<Checkpoint>().AddCheckpointToGrid();
        CinemaMachineCamerasController.ResetCameras();

        shared.ball.ResetBall();
        Destroy(shared._loseBg);
        Instantiate(shared.orb, shared.orbInitialLocation, Quaternion.identity);
    }

    public static void ActivateTetrisSequence()
    {
        shared.spawner.AllowSpawn();
        CinemaMachineCamerasController.AddZoomCamera(shared.zoomoutCameraInitialLocation, 0.3f);
        CheckpointsGenerator.GenerateNewPoint();
        MusicManager.ChangeMusic(shared.gameMusic);
        LevelManager.LoadNextLevel();
    }

    public static void UpdateSpawnerPosition(int distanceOfCamera)
    {
        shared.spawner.ChangeCameraDistance(distanceOfCamera);
    }
    
    private static IEnumerator ResetCooldown(float time)
    {
        shared._isSpawnAllowed = false;
        yield return new WaitForSeconds(time);
        shared._isSpawnAllowed = true;
    }

    private static IEnumerator LoseAnimation()
    {
        shared.ball.DeactivateBallMovement();
        var position = shared.ball.transform.position;
        shared._loseBg = Instantiate(shared.LoseAnimationBG, position, quaternion.identity);
        CinemaMachineCamerasController.AddZoomCamera(position, 0.3f, 3);
        yield return new WaitForSeconds(0.3f);
        CinemaMachineCamerasController.AddZoomCamera(position, 3f, 0.5f);
        yield return new WaitForSeconds(3f);
        LoseAnimationContinue();
    }
    
    private static void LoseAnimationContinue()
    {
        shared.StartCoroutine(MusicManager.FadeOut(0.5f));
        shared.transitions.GetComponent<Animator>().SetTrigger(ToFadeOut);
        shared.StartCoroutine(ResetCooldown(2));
    }

    #endregion
}