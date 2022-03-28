using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private BarricadeGenerator barricadeGenerator;
    [SerializeField] private SpawnerScript spawner;
    [SerializeField] private List<GameObject> blocks1;
    [SerializeField] private List<GameObject> blocks2;
    [SerializeField] private List<GameObject> blocks3;
    [SerializeField] private List<GameObject> blocks4;
    [SerializeField] private List<GameObject> blocks5;
    [SerializeField] private List<GameObject> blocks6;
    [SerializeField] private GameObject inverter;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private GameObject arrows;
    [SerializeField] private GameObject logo;


    private static LevelManager _shared;
    private int _currentLevel;
    private bool _toDeactivateArrows;
    private static readonly int Disable = Animator.StringToHash("Disable");

    #endregion

    #region MonoBehaviour
    
    private void Awake()
    {
        _shared = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.RightArrow))
            _shared._toDeactivateArrows = true;

        if (!_toDeactivateArrows) return;
        _shared.StartCoroutine(_shared.DeactivateArrows());
        _shared._toDeactivateArrows = false;
    }
    
    #endregion

    #region Methods
    
    public static void ResetLevel()
    {
        _shared._currentLevel = 0;
        _shared.inverter.SetActive(false);
        CinemaMachineCamerasController.DisableCameraTilt();
        CheckpointsGenerator.ChangeRadius(28f);
        CinemaMachineCamerasController.SetCameraTransitionsStyle(CinemachineBlendDefinition.Style.EaseInOut);
        _shared.logo.transform.GetChild(0).gameObject.SetActive(false);
        var logoAnimator = _shared.logo.GetComponent<Animator>();
        logoAnimator.ResetTrigger("Activate");
        logoAnimator.Play("Empty", -1 ,0);
        StartTutorial();
    }

    public static void LoadNextLevel()
    {
        _shared._currentLevel += 1;
        switch (_shared._currentLevel)
        {
            case 1:
                Level1();
                break;
            case 2:
                Level2();
                break;
            case 3:
                Level3();
                break;
            case 4:
                Level4();
                break;
            case 5:
                Level5();
                break;
            case 6:
                Level6();
                break;
            default:
                Level6();
                break;
        }
    }

    private static void Level1()
    {
        _shared.spawner.tetrisBlocks = _shared.blocks1;
        CinemaMachineCamerasController.SetCameraTransitionsDuration(30f);
    }

    private static void Level2()
    {
        BarricadeGenerator.CallToInitiate();
        MusicManager.PlayEffect(_shared.sfx);
        _shared.spawner.tetrisBlocks = _shared.blocks2;
        CinemaMachineCamerasController.SetCameraTransitionsStyle(CinemachineBlendDefinition.Style.EaseIn);
        CinemaMachineCamerasController.SetCameraTransitionsDuration(42f);
        CheckpointsGenerator.ChangeRadius(40f);
    }

    private static void Level3()
    {
        BarricadeGenerator.CallToInitiate();
        _shared.spawner.tetrisBlocks = _shared.blocks3;
        CinemaMachineCamerasController.ActivateCameraTilt();
        CinemaMachineCamerasController.SetCameraTransitionsDuration(55f);
        CheckpointsGenerator.ChangeRadius(45f);
    }

    private static void Level4()
    {
        BarricadeGenerator.CallToInitiate();
        CinemaMachineCamerasController.SetCameraTransitionsDuration(47f);
        _shared.spawner.tetrisBlocks = _shared.blocks4;
        _shared.inverter.SetActive(true);
    }

    private static void Level5()
    {
        CinemaMachineCamerasController.SetCameraTransitionsDuration(47f);
        BarricadeGenerator.CallToInitiate();
        _shared.spawner.tetrisBlocks = _shared.blocks5;
    }
    
    private static void Level6()
    {
        CinemaMachineCamerasController.SetCameraTransitionsDuration(42f);
        BarricadeGenerator.CallToInitiate();
        _shared.spawner.tetrisBlocks = _shared.blocks6;
    }


    private static void StartTutorial()
    {
        _shared.arrows.SetActive(true);
        foreach (Transform child in _shared.arrows.transform)
        {
            var color = child.gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 1;
            child.gameObject.GetComponent<SpriteRenderer>().color = color;
            child.gameObject.GetComponent<Animator>().Play(0, -1, 0f);
        }

        _shared.spawner.ActivateTutorialState();
    }

    private IEnumerator DeactivateArrows()
    {
        foreach (Transform arrow in _shared.arrows.transform)
        {
            arrow.gameObject.GetComponent<Animator>().SetTrigger(Disable);
        }
        
        _shared.logo.GetComponent<Animator>().SetTrigger("Activate");

        yield return new WaitForSeconds(0.4f);
        foreach (Transform child in _shared.arrows.transform)
        {
            var color = child.gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 1;
            child.gameObject.GetComponent<SpriteRenderer>().color = color;
        }

        _shared.arrows.SetActive(false);
    }
    
    #endregion
}