using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BarricadeGenerator : MonoBehaviour
{
    public GameObject[] barricades;

    public SpawnerScript spawner;

    public GameObject ball;

    private int[] ScreenXPoints;
    [SerializeField] private int xRangeScreen = 10;

    [SerializeField] private float timeToGenerate = 2;
    [SerializeField] private float maxRandX = 3f;
    [SerializeField] private int spawnerDistance = 3;

    private float _timer;

    private int _lastXPose;
    // Start is called before the first frame update


    private void Start()
    {
        var ballPoseX = Mathf.RoundToInt(ball.transform.position.x);
        ScreenXPoints = new[] {ballPoseX, ballPoseX + xRangeScreen, ballPoseX - xRangeScreen};
    }

    // Update is called once per frame
    void Update()
    {
        print(!spawner.isSpawnAllowed);
        if (!spawner.isSpawnAllowed)
        {
            if (_timer >= timeToGenerate)
            {
                _timer = 0;
                var index = Mathf.RoundToInt(Random.Range(0, barricades.Length));
                var barricade = barricades[index];
                
                if (barricade.name == "square")
                {
                   SquareInstantiate(barricade); 
                }
                
                // if (barricade.name == "Line")
                // {
                //     SquareInstantiate(barricade); 
                // }

                else
                {
                    LineInstantiate(barricade);  
                }
                
                
                
            }

            _timer += Time.deltaTime;
        }
    }

    private void SquareInstantiate(GameObject barricade)
    {
        var spawnerPos = spawner.transform.position;
        var xPos = Mathf.RoundToInt(ball.transform.position.x);
        if (xPos == _lastXPose)
        {
            xPos = Mathf.RoundToInt(Random.Range(xPos - 1, xPos + 1));
        }

        // var xPos = Mathf.RoundToInt(Random.Range(spawnerPos.x - maxRandX, spawnerPos.x + maxRandX ));
        var ypos = Mathf.RoundToInt(Random.Range(spawnerPos.y - 1.7f * spawnerDistance,
            spawnerPos.y - spawnerDistance));

        var newBaricadePos = new Vector3(xPos, ypos, 0);
        Instantiate(barricade, newBaricadePos, Quaternion.identity);
    }

    private void LineInstantiate(GameObject barricade)
    {
        var spawnerPos = spawner.transform.position;

        var index = Mathf.RoundToInt(Random.Range(0, ScreenXPoints.Length));
        var xPos = ScreenXPoints[index];
        print(xPos);
        
        var yPos = Mathf.RoundToInt(Random.Range(spawnerPos.y - 1.7f * spawnerDistance,
            spawnerPos.y - spawnerDistance));

        var newBaricadePos = new Vector3(xPos, yPos, 0);
        Instantiate(barricade, newBaricadePos, Quaternion.identity);
    }
}