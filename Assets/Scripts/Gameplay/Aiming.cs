using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public Transform rotatableBody;
    public Transform rotatableTurret;

    GameDisplay gameDisplay;

    // Start is called before the first frame update
    void Start()
    {
        gameDisplay = FindObjectOfType<GameDisplay>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HorizontalAim();
        VerticalAim();
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
}
