using System;
using UnityEngine;

public class InteractSphere : GridOccupant, IInteractable
{

    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;

    [SerializeField] private MeshRenderer meshRenderer;

    private Action onInteractComplete;
    private bool isActive;
    private float timer;
    
    private bool isGreen;
    
    protected override void OccupantStart()
    {
        SetColorGreen();
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
        
        if (isGreen)
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }
    }
    
    private void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }
    
    private void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }
}
