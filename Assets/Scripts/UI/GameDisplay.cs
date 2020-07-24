using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameDisplay : MonoBehaviour
{
    bool isInActionsPanel = false;

    [Header("UI References")]
    public GameObject actionsPanel;

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
        TurnOffUnitActions();

        playerCombatManager = FindObjectOfType<PlayerCombatManager>();
    }

    // Update is called once per frame
    void Update()
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

    public void SetWeaponConfiguration(int tankNum)
    {
        powerSlider.maxValue = playerCombatManager.listOfMaxPowers[tankNum];
        powerSlider.value = 0;
        powerText.text = "0";

        horizontalAngleSlider.value = playerCombatManager.listOfHorizontalAngles[tankNum];
        horizontalAngleText.text = playerCombatManager.listOfHorizontalAngles[tankNum].ToString();

        verticalAngleSlider.value = playerCombatManager.listOfVerticalAngles[tankNum];
        verticalAngleText.text = playerCombatManager.listOfVerticalAngles[tankNum].ToString();
    }

    public void UpdateWeaponConfiguration()
    {
        playerCombatManager.power = (int)powerSlider.value;
        playerCombatManager.horizontalAngle = (int)horizontalAngleSlider.value;
        playerCombatManager.verticalAngle = (int)verticalAngleSlider.value;

        powerText.text = powerSlider.value.ToString();
        horizontalAngleText.text = horizontalAngleSlider.value.ToString();
        verticalAngleText.text = verticalAngleSlider.value.ToString();
    }
}
