using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;

                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // No more enemies can take an action, so we end the turn
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = .5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 1f;
        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (!enemyUnit.gameObject.activeSelf)
            {
                // Skip inactive units
                continue;
            }
            
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        List<EnemyAIAction> bestEnemyAIActions = new List<EnemyAIAction>();

        foreach (BaseAction baseAction in enemyUnit.GetBaseActionsArray())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                continue;
            }

            if (!baseAction.CanBeTaken())
            {
                continue;
            }

            EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();

            if (testEnemyAIAction == null)
            {
                continue;
            }
            
            testEnemyAIAction.action = baseAction;

            if (bestEnemyAIActions.Count == 0)
            {
                // It's the first action we're checking, so just set it as the best
                bestEnemyAIActions.Add(testEnemyAIAction);
            }
            else
            {
                EnemyAIAction bestEnemyAIAction = bestEnemyAIActions[0];
                if (testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIActions.Clear();
                    bestEnemyAIActions.Add(testEnemyAIAction);
                }
                else if (testEnemyAIAction.actionValue == bestEnemyAIAction.actionValue)
                {
                    // It's as good as the current best action. Let's add it to the list,
                    // and later we'll choose random from these
                    bestEnemyAIActions.Add(testEnemyAIAction);
                }
            }
        }

        if (bestEnemyAIActions.Count == 0)
        {
            return false;
        }

        Random rnd = new Random();
        int randomIdx = rnd.Next(bestEnemyAIActions.Count);
        
        EnemyAIAction randomBestAction = bestEnemyAIActions[randomIdx];
        BaseAction action = randomBestAction.action;
        
        if (enemyUnit.TrySpendActionPointsToTakeAction(action))
        {
            action.TakeAction(randomBestAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }

        return false;
    }
    
}
