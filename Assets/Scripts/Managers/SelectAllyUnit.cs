using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAllyUnit : MonoBehaviour
{
    Touch touch;

    public PlayerMovement selectedUnit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnManager.alliesCanMoveNow && !TurnManager.allyUnitIsMoving)
            SelectWithTouch();
    }

    void SelectWithTouch()
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
                        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally Tank");
                        foreach(GameObject a in allies)
                        {
                            a.GetComponent<PlayerMovement>().isSelected = false;
                        }

                        selectedUnit = hit.collider.GetComponentInParent<PlayerMovement>();
                        if (selectedUnit != null)
                        {
                            selectedUnit.isSelected = true;
                        }
                    }
                    else if (hit.collider.tag == "Tile")
                    {
                        if (!hit.collider.GetComponent<Tile>().selectable && selectedUnit != null)
                        {
                            selectedUnit.isSelected = false;
                            selectedUnit.RemoveSelectableTiles();
                        }
                    }
                }
            }
        }
    }
}
