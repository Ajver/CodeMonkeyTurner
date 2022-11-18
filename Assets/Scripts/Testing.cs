using System;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform gameObjectPrefab; 

    private GridSystem gridSystem;
    
    void Start()
    {
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gameObjectPrefab);
    }
}
