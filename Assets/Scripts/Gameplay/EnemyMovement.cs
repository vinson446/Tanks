using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : TacticsMovement
{
    GameObject target;
    public bool myTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myTurn)
        {
            return;
        }

        if (!isMoving)
        {
            FindNearestTarget();
            CalculatePath();

            FindSelectableTiles();
            actualTargetTile.target = true;
        }
        else
        {
            Move();
        }
    }

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Ally Tank");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject t in targets)
        {
            float d = Vector3.Distance(transform.position, t.transform.position);
            if (d < distance)
            {
                distance = d;
                nearest = t;
            }
        }

        target = nearest;
    }

    void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }
}
