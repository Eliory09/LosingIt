using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] private GameObject _father = null;

    [SerializeField] private Transform backGroundMeasurement;

    private static float backGroundHeight;
    [SerializeField] private  GameObject[] backGrounds;

    private static GameObject _currentBackground;

    private static bool _change;

    private bool _counter = true;

    private void Awake()
    {
        _currentBackground = backGrounds[0];
        backGroundHeight =backGroundMeasurement.position.y - _currentBackground.transform.position.y;
    }

    void Update()
    {
        if (_change)
        {
            _change = false;
            HigherBackGround();
        }
    }

    private void HigherBackGround()
    {
        var oldPos = _currentBackground.transform.position;
        var newPos = new Vector3(oldPos.x, oldPos.y + backGroundHeight, oldPos.z);
        
        var obj = Instantiate(backGrounds[1], newPos, quaternion.identity );

        if (_counter)
        {
            obj.transform.Rotate(180 , 0 , 0);
            print("kaka");
        }

        _counter = !_counter;
        obj.transform.SetParent(_father.transform);
        _currentBackground = obj;
    }

    public static void ChangeBackGround()
    {
        _change = true;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    
}