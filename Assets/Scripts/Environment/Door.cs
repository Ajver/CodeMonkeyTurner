using System;
using UnityEngine;

public class Door : GridOccupant, IInteractable
{
    [SerializeField] private Animator animator;

    [SerializeField] private bool isOpen;
 
    private Action onInteractComplete;
    private bool isActive;
    private float timer;
    
    protected override void OccupantStart()
    {
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    protected override void OccupantUpdate()
    {
        if (!isActive)
        {
            return;
        }
        
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isActive = false;
            onInteractComplete();
        }
    }

    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;

        isActive = true;
        timer = .5f;
        
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        
        Debug.Log("Door opened: " + isOpen);
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, isOpen);
    }
    
    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, isOpen);
    }
}
