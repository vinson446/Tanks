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
        StartCoroutine(AllyAttackCoroutine(listOfWeaponNames[currentTank]));
    }

    IEnumerator AllyAttackCoroutine(string weap)
    {
        // stop setting tank aim to ui slider values
        aimingScripts[currentTank].madeAction = true;

        yield return new WaitForSeconds(1);

        switch (weap)
        {
            case "BIG SHOT":
                BigShot();
                break;
            case "RAPID FIRE":
                StartCoroutine(RapidFire());
                break;
            case "SHOTGUN":
                Shotgun();
                break;
            case "SKY STRIKE":
                SkyStrike();
                break;
            case "GOLDEN BULLET":
                GoldenBullet();
                break;
            case "EXPLOSION":
                Explosion();
                break;
            case "MINI BOMBS":
                MiniBombs();
                break;
            case "NAPALM":
                Napalm();
                break;
            case "LASER BEAM":
                LaserBeam();
                break;
            case "HEALER":
                Healer();
                break;
        }

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

    // 1- big shot!
    void BigShot()
    {
        GameObject bullet = Instantiate(weapons[currentTank], weaponSpawnpoints[currentTank].position, Quaternion.Euler(new Vector3(90, 0, 0)));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(-weaponSpawnpoints[currentTank].transform.forward * (gameDisplay.powerSlider.value * powerMultiplier + basePower));

        Bullet b = bullet.GetComponent<Bullet>();
        b.isAllyBullet = true;
        b.damage = weaponDamages[currentTank];
        b.timer = 5;

        // camera shift to bullet
        cameraManager.FocusOnAttack(bullet.transform);
    }

    // 2- shoots 5-10 bullets at slightly varying angles
    IEnumerator RapidFire()
    {
        GameObject firstBulletToLookAt;

        for (int i = 0; i < Random.Range(5, 11); i++)
        {
            GameObject bullet = Instantiate(weapons[currentTank], weaponSpawnpoints[currentTank].position, Quaternion.Euler(new Vector3(90, 0, 0)));

            // add force in slightly varying direction
            Quaternion rotation = Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), 0);
            Vector3 direction = rotation * -weaponSpawnpoints[currentTank].transform.forward;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(direction * (gameDisplay.powerSlider.value * powerMultiplier + basePower));

            Bullet b = bullet.GetComponent<Bullet>();
            b.isAllyBullet = true;
            b.damage = weaponDamages[currentTank];
            b.timer = 5;

            yield return new WaitForSeconds(0.1f);

            if (i == 0)
            {
                firstBulletToLookAt = bullet;

                // camera shift to bullet
                cameraManager.FocusOnAttack(firstBulletToLookAt.transform);
            }
        }
    }

    // 3- shoots 8 bullets 
    // bullets do lots of damage, but dont last long (close range)
    void Shotgun()
    {
        GameObject firstBulletToLookAt;

        for (int i = 0; i < 8; i++)
        {
            GameObject bullet = Instantiate(weapons[currentTank], weaponSpawnpoints[currentTank].position, Quaternion.Euler(new Vector3(90, 0, 0)));

            // add force in circle direction
            Quaternion rotation = Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), 0);
            Vector3 direction = rotation * -weaponSpawnpoints[currentTank].transform.forward;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(direction * (gameDisplay.powerSlider.value * powerMultiplier + basePower));

            Bullet b = bullet.GetComponent<Bullet>();
            b.isAllyBullet = true;
            b.damage = weaponDamages[currentTank];
            b.timer = 0.25f;

            if (i == 0)
            {
                firstBulletToLookAt = bullet;

                // camera shift to bullet
                cameraManager.FocusOnAttack(firstBulletToLookAt.transform);
            }
        }
    }

    // 4
    void SkyStrike()
    {
        GameObject bullet = Instantiate(weapons[currentTank], weaponSpawnpoints[currentTank].position, Quaternion.Euler(new Vector3(90, 0, 0)));

        bullet.AddComponent<SkyStrike>();

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(-weaponSpawnpoints[currentTank].transform.forward * (gameDisplay.powerSlider.value * powerMultiplier + basePower));

        Bullet b = bullet.GetComponent<Bullet>();
        b.isAllyBullet = true;
        b.damage = 0;
        b.timer = 5;

        // camera shift to bullet
        cameraManager.FocusOnAttack(bullet.transform);
    }

    // 5
    void GoldenBullet()
    {
        GameObject bullet = Instantiate(weapons[currentTank], weaponSpawnpoints[currentTank].position, Quaternion.Euler(new Vector3(90, 0, 0)));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(-weaponSpawnpoints[currentTank].transform.forward * (gameDisplay.powerSlider.value * powerMultiplier + basePower));

        Bullet b = bullet.GetComponent<Bullet>();
        b.isAllyBullet = true;
        b.damage = weaponDamages[currentTank];
        b.timer = 5;

        // camera shift to bullet
        cameraManager.FocusOnAttack(bullet.transform);
    }

    // 6
    void Explosion()
    {

    }

    // 7
    void MiniBombs()
    {

    }

    // 8
    void Napalm()
    {

    }

    // 9
    void LaserBeam()
    {

    }

    // 10
    void Healer()
    {

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
