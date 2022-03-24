using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private static LevelManager _shared;
    private int currentLevel;


    private void Awake()
    {
        _shared = this;
    }

    public static void ResetLevel()
    {
        _shared.currentLevel = 0;
    }

    public static void LoadNextLevel()
    {
        _shared.currentLevel += 1;
        print(_shared.currentLevel);
        print(_shared._spawner);
        print(_shared._blocks1.Capacity);
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
        _shared._spawner.tetrisBlocks = _shared._blocks3;
        // CinemachineCamerasController.ActivateCameraTilt();
    }
    
    private static void Level2()
    {
        _shared._spawner.tetrisBlocks = _shared._blocks2;
    }
    
    private static void Level3()
    {
        _shared._spawner.tetrisBlocks = _shared._blocks1;
    }
    
    private static void Level4()
    { 
        _shared._spawner.tetrisBlocks = _shared._blocks1;
    }
    
    private static void Level5()
    {
        _shared._spawner.tetrisBlocks = _shared._blocks1;
    }
}