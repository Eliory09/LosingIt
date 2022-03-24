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

    [SerializeField] public int maxIndex = 0;
    [SerializeField] public float timeToGenerate = 5;
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
        if (!spawner.stopSpawn && spawner.isSpawnAllowed)
        {
            if (_timer >= timeToGenerate)
            {
                _timer = 0;
                var index = Mathf.RoundToInt(Random.Range(0, maxIndex + 1));
                var barricade = barricades[index];
                SquareInstantiate(barricade);
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
            xPos = Mathf.RoundToInt(Random.Range(xPos - maxRandX, xPos + maxRandX));
        }

        // var xPos = Mathf.RoundToInt(Random.Range(spawnerPos.x - maxRandX, spawnerPos.x + maxRandX ));
        var ypos = Mathf.RoundToInt(spawnerPos.y - spawnerDistance);

        var newBaricadePos = new Vector3(xPos, ypos, 0);
        Instantiate(barricade, newBaricadePos, Quaternion.identity);
    }

    private void LineInstantiate(GameObject barricade)
    {
        var spawnerPos = spawner.transform.position;

        var index = Mathf.RoundToInt(Random.Range(0, ScreenXPoints.Length));
        var xPos = ScreenXPoints[index];
        // print(xPos);

        var yPos = Mathf.RoundToInt(spawnerPos.y - spawnerDistance*0.5f);

        var newBaricadePos = new Vector3(xPos, yPos, 0);
        Instantiate(barricade, newBaricadePos, Quaternion.identity);
    }
}