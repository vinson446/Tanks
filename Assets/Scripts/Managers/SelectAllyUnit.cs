using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAllyUnit : MonoBehaviour
{
    Touch touch;

    public PlayerMovement selectedUnit;

    CameraManager cameraManager;
    GameDisplay gameDisplay;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        gameDisplay = FindObjectOfType<GameDisplay>();
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
                        // first time to select unit
                        if (selectedUnit == null)
                        {
                            selectedUnit = hit.collider.GetComponentInParent<PlayerMovement>();
                            if (selectedUnit != null)
                            {
                                selectedUnit.isSelected = true;
                            }
                        }
                        else
                        {
                            // can only select ally units before movement finishes
                            if (selectedUnit.hasMovedAlready)
                            {

                            }
                            else
                            {
                                selectedUnit.isSelected = false;

                                selectedUnit = hit.collider.GetComponentInParent<PlayerMovement>();
                                if (selectedUnit != null)
                                {
                                    selectedUnit.isSelected = true;
                                }
                            }
                        }
                        cameraManager.FocusOnTarget(selectedUnit.gameObject.transform);
                    }
                    else if (hit.collider.tag == "Tile")
                    {
                        if (!hit.collider.GetComponent<Tile>().selectable && selectedUnit != null)
                        {
                            if (!selectedUnit.hasMovedAlready)
                            {
                                selectedUnit.isSelected = false;
                                selectedUnit.RemoveSelectableTiles();

                                selectedUnit = null;

                                cameraManager.LookAtGameWorld();
                            }
                        }
                    }
                }
            }
        }
    }
}
