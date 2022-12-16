using System;
using UnityEngine;

public class Suitcase : MonoBehaviour
{

    [SerializeField] private Animator animator;

    public void Open()
    {
        animator.SetBool("IsOpen", true);
    }
    
}
