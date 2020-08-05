using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isAllyBullet = false;
    public bool hitSomething;
    public int damage;
    public float timer;

    Rigidbody rb;
    Quaternion initialRot;

    PlayerCombatManager playerCombatManager;
    EnemyCombat enemyCombatManager;
    CameraManager cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        initialRot = transform.rotation;

        if (isAllyBullet)
            playerCombatManager = FindObjectOfType<PlayerCombatManager>();
        cameraManager = FindObjectOfType<CameraManager>();

        Invoke("Rip", timer);
    }

    // Update is called once per frame
    void Update()
    {
        // initial rot = transform.rotation in Start()
        if (!hitSomething)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity) * initialRot;
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            Collider coll = GetComponent<Collider>();
            coll.enabled = false;
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.enabled = false;
            cameraManager.attackCam.GetComponent<FollowCam>().target = null;

            hitSomething = true;
            if (isAllyBullet)
                playerCombatManager.bulletLanded = true;
            else
                enemyCombatManager.bulletLanded = true;


            Destroy(gameObject, 3);

            if (collision.gameObject.tag == "Enemy Tank")
            {
                EnemyCombat enemy = collision.gameObject.GetComponent<EnemyCombat>();
                enemy.TakeDamage(damage);
            }
            else if (collision.gameObject.tag == "Ally Tank")
            {
                PlayerCombat ally = collision.gameObject.GetComponent<PlayerCombat>();
                ally.TakeDamage(damage);
            }
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Bullet")
        {
            Collider coll = GetComponent<Collider>();
            coll.enabled = false;
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.enabled = false;
            cameraManager.attackCam.GetComponent<FollowCam>().target = null;

            hitSomething = true;
            if (isAllyBullet)
                playerCombatManager.bulletLanded = true;
            else
                enemyCombatManager.bulletLanded = true;


            Destroy(gameObject, 3);

            if (other.gameObject.tag == "Enemy Tank")
            {
                EnemyCombat enemy = other.gameObject.GetComponent<EnemyCombat>();
                enemy.TakeDamage(damage);
            }
            else if (other.gameObject.tag == "Ally Tank")
            {
                PlayerCombat ally = other.gameObject.GetComponent<PlayerCombat>();
                ally.TakeDamage(damage);
            }
        }
    }

    public void SetEnemyFlag(EnemyCombat enemy)
    {
        enemyCombatManager = enemy;
    }

    void Rip()
    {
        Collider coll = GetComponent<Collider>();
        coll.enabled = false;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = false;
        cameraManager.attackCam.GetComponent<FollowCam>().target = null;

        hitSomething = true;
        if (isAllyBullet)
            playerCombatManager.bulletLanded = true;
        else
            enemyCombatManager.bulletLanded = true;

        SkyStrike skyStrike = GetComponent<SkyStrike>();
        if (skyStrike != null)
            skyStrike.forever = false;

        Destroy(gameObject, 3);
    }
}
