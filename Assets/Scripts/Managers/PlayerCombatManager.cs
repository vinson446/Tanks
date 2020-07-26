using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    [Header("Current Tank Details")]
    public int currentTank;
    public string weapon;
    public int power;
    public int maxPower;
    public int horizontalAngle;
    public int verticalAngle;

    [Header("Tank Details")]
    public string[] listOfWeapons;
    public int[] listOfPowers;
    public int[] listOfMaxPowers;
    public int[] listOfHorizontalAngles;
    public int[] listOfVerticalAngles;

    // references
    TurnManager turnManager;
    GameDisplay gameDisplay;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        gameDisplay = FindObjectOfType<GameDisplay>();
    }

    public void UpdateTankDetails(int index)
    {
        // get new tank details
        currentTank = index;
        weapon = listOfWeapons[index];
        power = listOfPowers[index];
        maxPower = listOfMaxPowers[index];
        horizontalAngle = listOfHorizontalAngles[index];
        verticalAngle = listOfVerticalAngles[index];
    }

    public void Attack()
    {
        // backend
        gameDisplay.TurnOffUnitActions();

        maxPower -= (int)gameDisplay.powerSlider.value;
        listOfMaxPowers[currentTank] = maxPower;

        listOfHorizontalAngles[currentTank] = (int)gameDisplay.horizontalAngleSlider.value;
        listOfVerticalAngles[currentTank] = (int)gameDisplay.verticalAngleSlider.value;

        // frontend
        turnManager.AllyUnitEndTurn(currentTank);
    }

    public void Standby()
    {
        // backend
        gameDisplay.TurnOffUnitActions();

        if (maxPower + 25 <= 100)
        {
            maxPower += 25;
        }
        else
        {
            maxPower = 100;
        }

        listOfMaxPowers[currentTank] = maxPower;
        listOfHorizontalAngles[currentTank] = (int)gameDisplay.horizontalAngleSlider.value;
        listOfVerticalAngles[currentTank] = (int)gameDisplay.verticalAngleSlider.value;

        // frontend


        turnManager.AllyUnitEndTurn(currentTank);
    }
}
