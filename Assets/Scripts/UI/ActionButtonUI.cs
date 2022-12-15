using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI usagesLeftText;
    [SerializeField] private TextMeshProUGUI hotkeyText;
    [SerializeField] private Button button;
    [SerializeField] private Image selectedImg;

    [SerializeField] private Color textColorEnabled;
    [SerializeField] private Color textColorDisabled;

    private BaseAction action;

    public void Setup(BaseAction action, int actionNumber)
    {
        this.action = action;
        
        text.text = action.GetActionName();

        hotkeyText.text = $"[ {actionNumber} ]";

        UpdateUsagesLeft();
        
        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(action);
        });

        UpdateSelectedVisual();
    }

    public void UpdateUsagesLeft()
    {
        int usagesLeft = action.GetAvailableUsagesLeft();
        if (usagesLeft == -1)
        {
            usagesLeftText.enabled = false;
        }
        else
        {
            usagesLeftText.enabled = true;
            usagesLeftText.text = usagesLeft.ToString();
        }
        
        if (!action.CanBeTaken())
        {
            SetAsDisabled();
        }
        else
        {
            SetAsEnabled();
        }
    }
    
    private void SetAsDisabled()
    {
        button.interactable = false;
        
        text.color = textColorDisabled;
        usagesLeftText.color = textColorDisabled;
    }

    private void SetAsEnabled()
    {
        button.interactable = true;
        
        text.color = textColorEnabled;
        usagesLeftText.color = textColorEnabled;
    }
    
    public void UpdateSelectedVisual()
    {
        bool isSelected = action == UnitActionSystem.Instance.GetSelectedAction();
        selectedImg.enabled = isSelected;
    }

}
