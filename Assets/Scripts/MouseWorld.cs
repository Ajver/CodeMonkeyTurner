using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{

    private static MouseWorld instance;
    [SerializeField] private LayerMask mousePLaneLayerMask;

    private void Awake()
    {
        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, instance.mousePLaneLayerMask);
        return hitInfo.point;
    }
}
