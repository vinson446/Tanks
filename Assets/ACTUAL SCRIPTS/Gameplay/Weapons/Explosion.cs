using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Bullet")
        {
            Collider coll = GetComponent<Collider>();
            coll.enabled = false;
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.enabled = false;

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
}
