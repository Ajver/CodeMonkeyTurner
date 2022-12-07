using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    public static Testing Instance;

    public event EventHandler OnTestWinMission; 
    public event EventHandler OnTestLoseMission;
    
    
    private void Awake()
    {
        Instance = this;
    }

    public void TestWinMission()
    {
        OnTestWinMission?.Invoke(this, EventArgs.Empty);
    }
    
    public void TestLoseMission()
    {
        OnTestLoseMission?.Invoke(this, EventArgs.Empty);
    }
}
