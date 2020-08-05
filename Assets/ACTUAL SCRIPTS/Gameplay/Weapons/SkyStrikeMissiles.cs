using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStrikeMissiles : MonoBehaviour
{
    public bool hitSomething = false;

    Rigidbody rb;
    Quaternion initialRot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        initialRot = transform.rotation;

        Destroy(gameObject, 5);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Bullet")
        {
            Collider coll = GetComponent<Collider>();
            coll.enabled = false;
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.enabled = false;

            hitSomething = true;

            Destroy(gameObject, 3);

            // spawn Explosion vfx and do damage
        }
    }
}
