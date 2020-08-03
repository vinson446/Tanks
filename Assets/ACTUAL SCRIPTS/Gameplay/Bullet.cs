using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isAllyBullet = false;
    public bool hitSomething;
    public int damage;

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

        Invoke("Rip", 5);
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

    private void OnCollisionEnter(Collision collision)
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

        Destroy(gameObject, 3);
    }
}
