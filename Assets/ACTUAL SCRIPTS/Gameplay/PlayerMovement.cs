﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : TacticsMovement
{
    Touch touch;

    [Header("Game Manager Flags")]
    public int tankNum;
    public bool isSelected = false;
    public bool hasMovedAlready = false;
    public bool finishedTurn = false;

    // references

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSelected)
        {
            if (!isMoving && !hasMovedAlready)
            {
                FindSelectableTiles();
                CheckTileTouchToMove();
            }
            else if (!hasMovedAlready)
            {
                turnManager.allyUnitIsMoving = true;
                Move();
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

                        if (t.selectable && !t.current)
                        {
                            // move target
                            CreatePathToTargetTile(t);
                        }
                    }
                }
            }
        }
    }
}
