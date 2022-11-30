using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActionTargetPicker : MonoBehaviour
{
    
    [SerializeField] private LayerMask menuDamagableLayerMask;
    [SerializeField] private LayerMask menuInteractableLayerMask;
    [SerializeField] private MenuShootAction unitShootAction;

    private bool isBusy;
    
    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (!InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            return;
        }

        IInteractable interactable = TryGetInteractableTarget();
        if (interactable != null)
        {
            isBusy = true;
            interactable.Interact(OnActionComplete);
            return;
        }
        
        IDamageable damageable = TryGetDamageableTarget();
        if (damageable != null)
        {
            isBusy = true;
            unitShootAction.TakeAction(damageable, OnActionComplete);
        }
    }

    private IDamageable TryGetDamageableTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, menuDamagableLayerMask);

        if (hit)
        {
            return hitInfo.collider.GetComponent<IDamageable>();
        }

        return null;
    }

    private IInteractable TryGetInteractableTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, menuInteractableLayerMask);

        if (hit)
        {
            return hitInfo.collider.GetComponent<IInteractable>();
        }

        return null;
    }

    private void OnActionComplete()
    {
        isBusy = false;
    }
}
