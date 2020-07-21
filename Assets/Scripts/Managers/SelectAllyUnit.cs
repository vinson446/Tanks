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
            SelectAllyWithTouch();
    }

    void SelectAllyWithTouch()
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

                        // if ally moved already, camera doesnt look at it
                        if (!selectedUnit.hasMovedAlready)
                            cameraManager.FocusOnTarget(selectedUnit.gameObject.transform);
                    }
                    else if (!selectedUnit.hasMovedAlready && hit.collider.tag == "Tile")
                    {
                        if (!hit.collider.GetComponent<Tile>().selectable && selectedUnit != null)
                        {
                            selectedUnit.isSelected = false;
                            selectedUnit.RemoveSelectableTiles();

                            cameraManager.LookAtGameWorld();
                        }
                    }
                }
            }
        }
    }
}
