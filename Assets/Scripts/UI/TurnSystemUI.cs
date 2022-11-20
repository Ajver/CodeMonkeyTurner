using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private Button endTurnBtn;
    [SerializeField] private TextMeshProUGUI turnNumberText;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        UpdateTurnNumber();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnNumber();
    }

    private void UpdateTurnNumber()
    {
        int turnNumber = TurnSystem.Instance.GetTurnNumber();
        turnNumberText.text = $"TURN {turnNumber}";
    }
    
}
