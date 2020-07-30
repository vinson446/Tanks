using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float targetRadius;
    public float launchAngle;

    public bool targetReady;
    public bool touchingGround;

    // cache
    Rigidbody rb;
    Vector3 initialPos;
    Quaternion initialRot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetReady = true;
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (targetReady)
                Launch();
            else
            {
                ResetToIntialState();
                //SetNewTarget();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ResetToIntialState();
        }

        // projectile in trajectory
        if (!touchingGround && !targetReady)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity) * initialRot;
        }
    }

    // launch obj towards target with a given launchAngle
    void Launch()
    {
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetXZPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        // rotate the object to face the target
        transform.LookAt(targetXZPos);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(launchAngle * Mathf.Deg2Rad);
        float H = target.position.y - transform.position.y;
        Debug.Log(tanAlpha);
        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        rb.velocity = globalVelocity;
        targetReady = false;
    }

    /*
    // set a target randomly around this obj based on radius
    void SetNewTarget()
    {
        Transform targetTransform = target.GetComponent<Transform>();

        // get new random point around this obj
        Vector3 rotationAxis = Vector3.up;
        float randomAngle = Random.Range(0f, 360f);
        Vector3 randomVectorOnGroundPlane = Quaternion.AngleAxis(randomAngle, rotationAxis) * Vector3.right;

        // scale randomVector with targetRadius
        // offset so target is placed around obj based on radius
        Vector3 randomPoint = randomVectorOnGroundPlane * targetRadius + new Vector3(transform.position.x, transform.position.y, transform.position.z);

        target.SetPositionAndRotation(randomPoint, targetTransform.rotation);
        targetReady = true;
    }
    */

    void ResetToIntialState()
    {
        rb.velocity = Vector3.zero;
        this.transform.SetPositionAndRotation(initialPos, initialRot);
        targetReady = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        touchingGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        touchingGround = false;
    }
}
