using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    [SerializeField] private Image selectedImg;

    private BaseAction action;

    public void SetBaseAction(BaseAction action)
    {
        this.action = action;
        
        text.text = action.GetActionName();
        
        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(action);
        });

        UpdateSelectedVisual();
    }

    public void UpdateSelectedVisual()
    {
        bool isSelected = action == UnitActionSystem.Instance.GetSelectedAction();
        selectedImg.enabled = isSelected;
    }

}
