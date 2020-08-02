using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public Transform rotatableBody;
    public Transform rotatableTurret;

    public bool madeAction = false;
    public bool reset = false;
    float timeCount;
    public Quaternion resetRotation = Quaternion.identity;

    GameDisplay gameDisplay;
    PlayerCombatManager playerCombatManager;

    // Start is called before the first frame update
    void Start()
    {
        gameDisplay = FindObjectOfType<GameDisplay>();
        playerCombatManager = FindObjectOfType<PlayerCombatManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!madeAction)
        {
            HorizontalAim();
            VerticalAim();
        }

        if (reset)
            ResetTankAim();
    }

    public void HorizontalAim()
    {
        rotatableBody.localRotation = Quaternion.Euler(new Vector3(rotatableBody.rotation.x,
            gameDisplay.horizontalAngleSlider.value,
            rotatableBody.rotation.z));
    }

    public void VerticalAim()
    {
        rotatableTurret.localRotation = Quaternion.Euler(new Vector3(gameDisplay.verticalAngleSlider.value,
            rotatableTurret.rotation.y,
            rotatableTurret.rotation.z));
    }

    void ResetTankAim()
    {
        rotatableBody.localRotation = Quaternion.Slerp(rotatableBody.localRotation, resetRotation, timeCount);
        rotatableTurret.localRotation = Quaternion.Slerp(rotatableTurret.localRotation, resetRotation, timeCount);
        timeCount += Time.fixedDeltaTime / 2;

        Invoke("ResetFlag", 1);
    }

    void ResetFlag()
    {
        rotatableBody.localRotation = rotatableTurret.localRotation = Quaternion.identity;
        timeCount = 0;

        madeAction = false;
        reset = false;
        this.enabled = false;
    }
}
