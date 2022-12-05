using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{

    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    [SerializeField] private AudioSource startPlayerTurnAudio;
    [SerializeField] private AudioSource startEnemyTurnAudio;
    
    private int turnNumber = 1;

    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one TurnSystem in the scene!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
        {
            startPlayerTurnAudio.Play();
        }
        else
        {
            startEnemyTurnAudio.Play();
        }
        
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

}
