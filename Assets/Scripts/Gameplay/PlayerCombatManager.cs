using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public int power;
    public int maxPower;

    // references
    TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void Standby()
    {
        if (maxPower + 25 <= 100)
        {
            maxPower += 25;
        }
        else
        {
            maxPower = 100;
        }

        turnManager.AllyUnitEndTurn();
    }
}
