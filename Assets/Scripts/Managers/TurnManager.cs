using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<PlayerMovement> playerTeam;
    public List<EnemyMovement> enemyTeam;

    public bool allyTeamTurn = true;
    public bool alliesCanMoveNow = false;
    public bool allyUnitIsMoving = false;
    public int allyUnitsFinished = 0;
    public int enemyUnitsFinished = 0;

    TurnDisplay turnDisplay;

    private void Start()
    {
        turnDisplay = FindObjectOfType<TurnDisplay>();
    }

    public void AllyUnitEndTurn(int currentTank)
    {
        allyUnitsFinished += 1;
        playerTeam[currentTank].finishedTurn = true;

        if (allyUnitsFinished == playerTeam.Count)
        {
            SwitchTeams();
        }
    }

    public void EnemyUnitEndTurn()
    {
        enemyUnitsFinished += 1;

        if (enemyUnitsFinished == enemyTeam.Count)
        {
            SwitchTeams();
        }
        else
        {
            DoNextEnemyTurn();
        }
    }

    public void DoNextEnemyTurn()
    {
        enemyTeam[enemyUnitsFinished].BeginTurn(enemyTeam[enemyUnitsFinished]);
    }

    public void SwitchTeams()
    {
        turnDisplay.UpdateTurn();
    }
}
