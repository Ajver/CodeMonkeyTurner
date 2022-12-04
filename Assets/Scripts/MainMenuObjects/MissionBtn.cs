using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionBtn : MonoBehaviour
{

    [SerializeField] private string missionSceneName;
    [SerializeField] private string missionDisplayName;
    
    [SerializeField] private Image outlineImage;
    [SerializeField] private GameObject lockGameObject;
    private Button button;

    [SerializeField] private Color activeOutlineColor;
    [SerializeField] private Color inactiveOutlineColor;
    
    [SerializeField] private TextMeshProUGUI missionDisplayNameText;

    [SerializeField] private bool isActive;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        missionDisplayNameText.text = missionDisplayName;
        
        SetIsActive(isActive);
    }

    private void OnClick()
    {
        SceneManager.LoadScene(missionSceneName);
    }
    
    private void OnActivate()
    {
        outlineImage.color = activeOutlineColor;
        button.interactable = true;
        lockGameObject.SetActive(false);
    }
    
    private void OnDeactivate()
    {
        outlineImage.color = inactiveOutlineColor;
        button.interactable = false;
        lockGameObject.SetActive(true);
    }

    public void SetIsActive(bool flag)
    {
        isActive = flag;
        
        if (isActive)
        {
            OnActivate();
        }
        else
        {
            OnDeactivate();
        }
    }
}
