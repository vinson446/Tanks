using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public PlayerMovement[] playerTeam = new PlayerMovement[3];
    public EnemyMovement[] enemyTeam;

    public static bool allyTeamTurn = true;
    public static bool allyUnitIsMoving = false;
    public int allyUnitsFinished = 0;
    public int enemyUnitsFinished = 0;

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
        if (allyUnitsFinished == 3)
        {
            allyTeamTurn = false;
            foreach (PlayerMovement p in playerTeam)
            {
                p.isSelected = false;
            }

            enemyTeam[0].BeginTurn(enemyTeam[0]);
        }
        else
        {
            foreach (EnemyMovement e in enemyTeam)
            {
                e.EndTurn(e);
            }
            allyTeamTurn = true;
        }

        allyUnitsFinished = 0;
        enemyUnitsFinished = 0;
    }
}
