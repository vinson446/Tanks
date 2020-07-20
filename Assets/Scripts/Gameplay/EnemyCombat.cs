using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void Attack(EnemyMovement enemy)
    {
        enemy.EndTurn(enemy);
        turnManager.EnemyUnitEndTurn();
    }
}
