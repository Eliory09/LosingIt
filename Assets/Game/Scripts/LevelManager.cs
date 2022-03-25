using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private BarricadeGenerator _barricadeGenerator;
    [SerializeField] private SpawnerScript _spawner;
    [SerializeField] private List<GameObject> _blocks1;
    [SerializeField] private List<GameObject> _blocks2;
    [SerializeField] private List<GameObject> _blocks3;
    [SerializeField] private List<GameObject> _blocks4;
    [SerializeField] private List<GameObject> _blocks5;
    [SerializeField] private List<GameObject> _blocks6;
    [SerializeField] private GameObject _inverter;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private GameObject arrows;


    private static LevelManager _shared;
    private int currentLevel;
    private bool toDeactivateArrows;


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
            _shared.toDeactivateArrows = true;
        
        if (toDeactivateArrows)
        {
            _shared.StartCoroutine(_shared.DeactivateArrows());
            _shared.toDeactivateArrows = false;
        }
    }

    public static void ResetLevel()
    {
        _shared.currentLevel = 0;
        _shared._inverter.SetActive(false);
        CinemachineCamerasController.DisableCameraTilt();
        CheckpointsGenerator.ChangeRadius(28f);
        CinemachineCamerasController.SetCameraTransitionsStyle(CinemachineBlendDefinition.Style.EaseInOut);
        StartTutorial();
    }

    public static void LoadNextLevel()
    {
        _shared.currentLevel += 1;
        switch (_shared.currentLevel)
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
            default:
                Level5();
                break;
        }
    }

    private static void Level1()
    {
        _shared._spawner.tetrisBlocks = _shared._blocks1;
    }
    
    private static void Level2()
    {
        MusicManager.PlayEffect(_shared.sfx);
        _shared._spawner.tetrisBlocks = _shared._blocks2;
        CinemachineCamerasController.SetCameraTransitionsStyle(CinemachineBlendDefinition.Style.Linear);
        CinemachineCamerasController.SetCameraTransitionsDuration(30f);
        CheckpointsGenerator.ChangeRadius(35f);
    }
    
    private static void Level3()
    {
        _shared._spawner.tetrisBlocks = _shared._blocks3;
        CinemachineCamerasController.ActivateCameraTilt();
        CinemachineCamerasController.SetCameraTransitionsDuration(36f);
        CheckpointsGenerator.ChangeRadius(40f);

    }
    
    private static void Level4()
    { 
        CinemachineCamerasController.SetCameraTransitionsDuration(33f);
        _shared._spawner.tetrisBlocks = _shared._blocks4;
        _shared._inverter.SetActive(true);
    }
    
    private static void Level5()
    {
        _shared._spawner.tetrisBlocks = _shared._blocks5;
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
    }
    
    private IEnumerator DeactivateArrows()
    {
        foreach (Transform arrow in _shared.arrows.transform)
        {
            arrow.gameObject.GetComponent<Animator>().SetTrigger("Disable");
        }
        yield return new WaitForSeconds(0.4f);
        foreach (Transform child in _shared.arrows.transform)
        {
            var color = child.gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 1;
            child.gameObject.GetComponent<SpriteRenderer>().color = color;
            
        }
        _shared.arrows.SetActive(false);
    }
}