using System;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ScreenShake.Instance.Shake(5f);
        }
    }
}