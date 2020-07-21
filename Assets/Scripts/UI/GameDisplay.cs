using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDisplay : MonoBehaviour
{
    public GameObject actionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        TurnOffUnitActions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayUnitActions()
    {
        actionsPanel.SetActive(true);
    }

    public void TurnOffUnitActions()
    {
        actionsPanel.SetActive(false);
    }
}
