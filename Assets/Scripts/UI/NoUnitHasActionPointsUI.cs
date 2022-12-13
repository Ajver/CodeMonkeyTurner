using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoUnitHasActionPointsUI : MonoBehaviour
{

    public void OnEndTurnBtnClicked()
    {
        TurnSystem.Instance.NextTurn();
    }
    
}
