using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : TacticsMovement
{
    Touch touch;
    public bool isSelected;

    TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnManager.allyTeamTurn && !TurnManager.allyUnitIsMoving)
            SelectUnitWithTouch();

        if (isSelected)
        {
            if (!isMoving)
            {
                FindSelectableTiles();
                CheckTileTouchToMove();
            }
            else
            {
                TurnManager.allyUnitIsMoving = true;
                Move();
            }
        }
    }

    void SelectUnitWithTouch()
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
                    if (hit.collider.tag == "Ally Tank")
                    {
                        PlayerMovement selectedUnit = hit.collider.GetComponentInParent<PlayerMovement>();
                        if (selectedUnit != null)
                        {
                            isSelected = false;
                            selectedUnit.isSelected = true;
                        }
                    }
                }
            }
        }
    }

    void CheckTileTouchToMove()
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
                }
            }
        }
    }
}
