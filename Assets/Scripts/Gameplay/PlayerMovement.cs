using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : TacticsMovement
{
    Touch touch;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        isPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!myTurn)
        {
            return;
        }

        if (!moving)
        {
            FindSelectableTiles();
            CheckTouch();
        }
        else
        {
            Move();
        }
    }

    void CheckTouch()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Tile")
                    {
                        Tile t = hit.collider.GetComponent<Tile>();

                        if (t.selectable)
                        {
                            // move target
                            MoveToTile(t);
                        }
                    }
                    else if (hit.collider.tag == "Ally Tank")
                    {

                    }
                    else if (hit.collider.tag == "Enemy Tank")
                    {

                    }
                }
            }
        }
    }
}
