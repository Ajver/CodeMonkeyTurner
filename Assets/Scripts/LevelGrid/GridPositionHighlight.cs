using System;
using UnityEngine;

public class GridPositionHighlight : MonoBehaviour
{
    [SerializeField] private GameObject visualsGameObject;
    [SerializeField] private Transform spinnerTransform;
    [SerializeField] private Transform explosionRange;

    private bool isSpinningFast;
    
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnHighlightedGridPositionChanged += UnitActionSystem_OnHighlightedGridPositionChanged;
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

        SetSpinnerActive(false);
        ShowOrHide();
    }

    private void Update()
    {
        float rotationSpeed = 60f;
        
        if (isSpinningFast)
        {
            rotationSpeed = 400f;
        }
        
        float rot = rotationSpeed * Time.deltaTime;
        spinnerTransform.Rotate(new Vector3(0f, rot, 0f));
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        ShowOrHide();
    }

    private void UnitActionSystem_OnHighlightedGridPositionChanged(object sender, EventArgs e)
    {
        UpdateGridPosition();
        ShowOrHide();
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        SetSpinnerActive(isBusy);
    }

    private void SetSpinnerActive(bool active)
    {
        isSpinningFast = active;
    }
    
    private void UpdateGridPosition()
    {
        GridPosition highlightedGridPosition = UnitActionSystem.Instance.GetHighlightedGridPosition();
        Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(highlightedGridPosition);
        transform.position = worldPosition;
    }

    private void ShowOrHide()
    {
        GridPosition highlightedGridPosition = UnitActionSystem.Instance.GetHighlightedGridPosition();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        
        bool shouldBeActive = selectedAction != null;
        shouldBeActive = shouldBeActive && selectedAction.IsValidActionGridPosition(highlightedGridPosition);
        
        visualsGameObject.SetActive(shouldBeActive);

        if (shouldBeActive)
        {
            UpdateExplosionRange();
        }
    }

    private void UpdateExplosionRange()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        
        bool shouldBeActive = false;
        float range = 0f;
        
        if (selectedAction is GrenadeAction)
        {
            shouldBeActive = true;
            range = GrenadeProjectile.EXPLOSION_RANGE;
        }
        else
        {
            GridPosition highlightedGridPosition = UnitActionSystem.Instance.GetHighlightedGridPosition();
            if (LevelGrid.Instance.GetOccupantAtGridPosition(highlightedGridPosition) is ExplodingBarrel)
            {
                shouldBeActive = true;
                range = ExplodingBarrel.EXPLOSION_RANGE;
            }
        }
        
        explosionRange.gameObject.SetActive(shouldBeActive);

        if (shouldBeActive)
        {
            // Range is radius, so must be doubled to show the right size
            range *= 2f;
            
            explosionRange.transform.localScale = new Vector3(range, range, range);
        }
    }

}
