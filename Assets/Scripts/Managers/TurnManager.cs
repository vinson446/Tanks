using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public PlayerMovement[] playerTeam;
    public EnemyMovement[] enemyTeam;

    public static bool allyTeamTurn = true;
    public static bool alliesCanMoveNow = false;
    public static bool allyUnitIsMoving = false;
    public static int allyUnitsFinished = 0;
    public static int enemyUnitsFinished = 0;

    TurnDisplay turnDisplay;

    private void Start()
    {
        turnDisplay = FindObjectOfType<TurnDisplay>();
    }

    public void AllyUnitEndTurn()
    {
        allyUnitsFinished += 1;

        if (allyUnitsFinished == 3)
        {
            SwitchTeams();
        }
    }

    public void EnemyUnitEndTurn()
    {
        enemyUnitsFinished += 1;

        if (enemyUnitsFinished == enemyTeam.Length)
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
