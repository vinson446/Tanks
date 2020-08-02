using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public float basePower;
    public float powerMultiplier;

    [Header("Current Tank Details")]
    public int currentTank;
    public string weapon;
    public int power;
    public int maxPower;
    public int horizontalAngle;
    public int verticalAngle;

    [Header("All Tank Details")]
    public GameObject[] weapons;
    public Aiming[] aimingScripts;
    public Transform[] weaponSpawnpoints;
    public string[] listOfWeaponNames;
    public int[] listOfPowers;
    public int[] listOfMaxPowers;

    [Header("Weapon Details")]
    public int[] weaponDamages;
    public int[] weaponPowerCosts;

    [Header("Combat Shit")]
    public bool bulletLanded = false;

    // references
    TurnManager turnManager;
    GameDisplay gameDisplay;
    CameraManager cameraManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        gameDisplay = FindObjectOfType<GameDisplay>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    public void UpdateTankDetails(int index)
    {
        // get new tank details
        currentTank = index;

        weapon = listOfWeaponNames[index];
        power = listOfPowers[index];
        maxPower = listOfMaxPowers[index];
    }

    public void Attack()
    {
        // backend
        gameDisplay.TurnOffUnitActions();

        maxPower -= weaponPowerCosts[currentTank];
        listOfMaxPowers[currentTank] = maxPower;

        // frontend
        StartCoroutine(AllyAttackCoroutine());
    }

    IEnumerator AllyAttackCoroutine()
    {
        // stop setting tank aim to ui slider values
        aimingScripts[currentTank].madeAction = true;

        yield return new WaitForSeconds(1);

        GameObject bullet = Instantiate(weapons[currentTank], weaponSpawnpoints[currentTank].position, Quaternion.Euler(new Vector3(90, 0, 0)));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(-weaponSpawnpoints[currentTank].transform.forward * (gameDisplay.powerSlider.value * powerMultiplier + basePower));

        Bullet b = bullet.GetComponent<Bullet>();
        b.damage = weaponDamages[currentTank];

        // camera shift to bullet
        cameraManager.FocusOnTarget(bullet.transform);

        while (!bulletLanded)
            yield return null;

        // when bullet collides with something, wait x seconds, then change bulletLanded = true
        yield return new WaitForSeconds(3);

        // reset tank aiming
        aimingScripts[currentTank].reset = true;

        cameraManager.LookAtGameWorld();

        bulletLanded = false;
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

        // frontend

        // reset tank aiming
        aimingScripts[currentTank].madeAction = true;
        aimingScripts[currentTank].reset = true;

        turnManager.AllyUnitEndTurn(currentTank);
        cameraManager.LookAtGameWorld();
    }
}
