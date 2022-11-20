using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private Button endTurnBtn;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnVisualGameObject;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        UpdateAllVisuals();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateAllVisuals();
    }

    private void UpdateAllVisuals()
    {
        UpdateTurnNumber();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }
    
    private void UpdateTurnNumber()
    {
        int turnNumber = TurnSystem.Instance.GetTurnNumber();
        turnNumberText.text = $"TURN {turnNumber}";
    }

    private void UpdateEnemyTurnVisual()
    {
        bool isEnemyTurn = !TurnSystem.Instance.IsPlayerTurn();
        enemyTurnVisualGameObject.SetActive(isEnemyTurn);
    }

    private void UpdateEndTurnButtonVisibility()
    {
        bool isPlayerTurn = TurnSystem.Instance.IsPlayerTurn();
        endTurnBtn.gameObject.SetActive(isPlayerTurn);
    }
    
}
