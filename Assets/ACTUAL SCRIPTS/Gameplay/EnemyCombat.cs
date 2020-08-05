using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour
{
    [Header("Stats")]
    public int indexInTurnManager;
    public float currentHP;
    public float maxHP;
    public int damage;
    public int attackRange;

    [Header("Inventory")]
    public GameObject weapon;
    public Transform weaponSpawnpoint;
    public int minProjectileSpeed;
    public int maxProjectileSpeed;

    [Header("UI")]
    public HealthBar hpBar;

    [Header("Aiming/Attacking Stuff")]
    public bool canAttack = false;
    public bool hasAttacked = false;
    public PlayerMovement closestTarget;
    float closestDistance = Mathf.Infinity;

    [Header("Combat Shit")]
    public bool bulletLanded = false;

    // references
    TurnManager turnManager;
    CameraManager cameraManager;

    private void Start()
    {
        currentHP = maxHP;

        turnManager = FindObjectOfType<TurnManager>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void FixedUpdate()
    {
        if (DoIAttack())
        {
            FaceTarget();
        }
    }

    bool DoIAttack()
    {
        for (int i = 0; i < turnManager.playerTeam.Count; i++)
        {
            // check if target is in attackRange
            float distance = Vector3.Distance(transform.position, turnManager.playerTeam[i].transform.position);

            if (distance <= attackRange)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = turnManager.playerTeam[i];
                }

                canAttack = true;
            }
        }

        if (canAttack)
            return true;
        else
            return false;
    }

    void FaceTarget()
    {
        if (closestTarget != null)
        {
            Vector3 direction = transform.position - closestTarget.gameObject.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 10);
        }
    }

    public void Attack(EnemyMovement enemy)
    {
        // target in range -> attack
        if (DoIAttack())
        {
            StartCoroutine(EnemyAttackCoroutine(enemy));
        }
        // otherwise, end turn
        else
        {
            StartCoroutine(EnemyStandbyCoroutine(enemy));
        }
    }

    IEnumerator EnemyAttackCoroutine(EnemyMovement enemy)
    {
        hasAttacked = true;

        yield return new WaitForSeconds(2);

        // could do a raycast, if distance from this enemy to closest ally target is blocked by an obstacle, shoot weapon at an upwards angle (random)

        GameObject bullet = Instantiate(weapon, weaponSpawnpoint.position, Quaternion.Euler(new Vector3(90, 0, 0)));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(-weaponSpawnpoint.transform.forward * Random.Range(minProjectileSpeed, maxProjectileSpeed + 1));

        Bullet b = bullet.GetComponent<Bullet>();
        b.damage = damage;
        b.timer = 5;
        b.SetEnemyFlag(this);

        // camera shift to bullet
        cameraManager.FocusOnAttack(bullet.transform);

        while (!bulletLanded)
            yield return null;

        // when bullet collides with something, wait x seconds, then change bulletLanded = true
        yield return new WaitForSeconds(3);

        cameraManager.LookAtGameWorld();

        enemy.EndTurn(enemy);
        turnManager.EnemyUnitEndTurn();

        // reset movement/attack checks
        enemy.hasMovedAlready = false;
        enemy.gotCam = false;
        hasAttacked = false;
    }

    IEnumerator EnemyStandbyCoroutine(EnemyMovement enemy)
    {
        yield return new WaitForSeconds(2);

        enemy.EndTurn(enemy);
        turnManager.EnemyUnitEndTurn();

        // reset movement/attack checks
        enemy.hasMovedAlready = false;
        enemy.gotCam = false;
        hasAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        hpBar.ShowHealthChange(currentHP / maxHP);

        if (currentHP <= 0)
        {
            turnManager.enemyTeam.RemoveAt(indexInTurnManager);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}
