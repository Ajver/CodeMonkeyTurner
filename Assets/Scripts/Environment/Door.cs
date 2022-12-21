using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConnectedLevelAreasPair : IEnumerable<LevelArea>
{
    public LevelArea area1;
    public LevelArea area2;
    
    public IEnumerator<LevelArea> GetEnumerator()
    {
        yield return area1;
        yield return area2;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class Door : GridOccupant, IInteractable
{
    [SerializeField] private Animator animator;

    [SerializeField] private AudioSource doorOpenAudio;
    [SerializeField] private AudioSource doorCloseAudio;

    [SerializeField] private bool isOpen;
    
    [Header("Which areas are on both sides of the door?")]
    [SerializeField] private ConnectedLevelAreasPair levelAreasConnected;

    public event EventHandler OnOpened;
    
    private Action onInteractComplete;
    private bool isActive;
    private float timer;
    
    protected override void OccupantStart()
    {
        if (isOpen)
        {
            OpenDoor(true);
        }
        else
        {
            CloseDoor(true);
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
            if (isOpen)
            {
                ActivateAreas();
                OnOpened?.Invoke(this, EventArgs.Empty);
            }
            
            isActive = false;
            onInteractComplete();
        }
    }

    private void ActivateAreas()
    {
        foreach (LevelArea area in levelAreasConnected)
        {
            area.Activate();
        }
    }

    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;

        isActive = true;
        timer = 0.5f;
        
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void OpenDoor(bool quietly = false)
    {
        isOpen = true;
        
        animator.SetBool("IsOpen", isOpen);

        if (!quietly)
        {
            doorOpenAudio.Play();
        }

        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, isOpen);
    }
    
    private void CloseDoor(bool quietly = false)
    {
        isOpen = false;
        
        animator.SetBool("IsOpen", isOpen);
        
        if (!quietly)
        {
            doorCloseAudio.Play();
        }
        
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, isOpen);
    }

    public ConnectedLevelAreasPair GetLevelAreasConnected()
    {
        return levelAreasConnected;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
    
}
