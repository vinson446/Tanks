﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameDisplay : MonoBehaviour
{
    // todo (set to false)
    public bool isInActionsPanel = false;

    [Header("UI References")]
    public GameObject actionsPanel;

    public TextMeshProUGUI weaponNameText;

    public Slider powerSlider;
    public TextMeshProUGUI powerText;

    public Slider horizontalAngleSlider;
    public TextMeshProUGUI horizontalAngleText;

    public Slider verticalAngleSlider;
    public TextMeshProUGUI verticalAngleText;

    // references
    PlayerCombatManager playerCombatManager;

    // Start is called before the first frame update
    void Start()
    {
        // todo
        TurnOffUnitActions();

        playerCombatManager = FindObjectOfType<PlayerCombatManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInActionsPanel)
            UpdateWeaponConfiguration();
    }

    public void DisplayUnitActions(int tankNum)
    {
        SetWeaponConfiguration(tankNum);
        isInActionsPanel = true;

        actionsPanel.SetActive(true);
    }

    public void TurnOffUnitActions()
    {
        isInActionsPanel = false;

        actionsPanel.SetActive(false);
    }

    // right when player finishes moving
    public void SetWeaponConfiguration(int tankNum)
    {
        weaponNameText.text = playerCombatManager.listOfWeaponNames[tankNum];

        powerSlider.maxValue = playerCombatManager.listOfMaxPowers[tankNum];
        powerSlider.value = 0;
        powerText.text = "0";

        horizontalAngleSlider.value = 0;
        horizontalAngleText.text = "0";

        verticalAngleSlider.value = 0;
        verticalAngleText.text = "0";

        playerCombatManager.aimingScripts[playerCombatManager.currentTank].enabled = true;
    }

    // while player is in weapon configuration (update)
    public void UpdateWeaponConfiguration()
    {
        /* todo */
        playerCombatManager.power = (int)powerSlider.value;
        playerCombatManager.horizontalAngle = (int)horizontalAngleSlider.value;
        playerCombatManager.verticalAngle = (int)verticalAngleSlider.value;

        powerText.text = powerSlider.value.ToString();
        horizontalAngleText.text = horizontalAngleSlider.value.ToString();
        verticalAngleText.text = verticalAngleSlider.value.ToString();
    }
}
