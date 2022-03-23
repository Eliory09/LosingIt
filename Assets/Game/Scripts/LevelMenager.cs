using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenager : MonoBehaviour
{
    [SerializeField] private BarricadeGenerator _barricadeGenerator;
    [SerializeField] private float timeLowerBarricade = 0.85f;

    public static bool addLevel = false;

    private int levelCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (addLevel)
        {
            addLevel = false;
            levelCounter += 1;
            _barricadeGenerator.maxIndex = levelCounter;
            _barricadeGenerator.timeToGenerate *= timeLowerBarricade;
        }
    }
}