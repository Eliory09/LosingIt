using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _shared;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _shared = this;
    }

    private void InitializeGame()
    {
        
    }
}
