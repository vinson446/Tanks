using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameObject gameCamera;
    public GameObject focusCamera;
    public float cameraMovementDivider;

    public float xMinBounds;
    public float xMaxBounds;
    public float zMinBounds;
    public float zMaxBounds;

    /*
    public bool isCamRotable;
    public float cameraRotateDivider;
    */

    Camera gameCam;
    Plane plane;

    // references

    // Start is called before the first frame update
    void Start()
    {
        gameCam = Camera.main;

        LookAtGameWorld();
    }

    public void FixedUpdate()
    {
        if (gameCamera.activeInHierarchy)
        {
            CameraControlsForGeneralGameplay();
        }
        else
        {

        }
    }

    public void LookAtGameWorld()
    {
        gameCamera.SetActive(true);
        focusCamera.SetActive(false);
    }

    public void FocusOnTarget(Transform target)
    {
        gameCamera.SetActive(false);
        focusCamera.SetActive(true);

        focusCamera.GetComponent<CinemachineVirtualCamera>().LookAt = target;
        focusCamera.GetComponent<CinemachineVirtualCamera>().Follow = target;
    }

    void CameraControlsForGeneralGameplay()
    {
        if (Input.touchCount >= 1)
        {
            plane.SetNormalAndPosition(transform.up, transform.position);
        }

        var delta1 = Vector3.zero;
        var delta2 = Vector3.zero;

        // scroll
        if (Input.touchCount >= 1)
        {
            delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                gameCamera.transform.Translate(delta1 / cameraMovementDivider, Space.World);

            gameCamera.transform.position = new Vector3(Mathf.Clamp(gameCamera.transform.position.x, xMinBounds, xMaxBounds),
                gameCamera.transform.position.y, 
                Mathf.Clamp(gameCamera.transform.position.z, zMinBounds, zMaxBounds));
        }

        // pinch
        /*
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            // calc zoom
            var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);

            // edge case
            if (zoom == 0 || zoom > 10)
                return;

            // move cam amount the mid ray
            gameCamera.transform.position = Vector3.LerpUnclamped(pos1, gameCam.transform.position, 1 / zoom);

            if (isCamRotable && pos2b != pos2)
                gameCamera.transform.RotateAround(pos1, plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, plane.normal) / cameraRotateDivider);
        }    
        */
    }

    // screenPos = finger on screen in pixels
    Vector3 PlanePosition(Vector2 screenPos)
    {
        var rayNow = gameCam.ScreenPointToRay(screenPos);
        if (plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    Vector3 PlanePositionDelta(Touch touch)
    {
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        var rayBefore = gameCam.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = gameCam.ScreenPointToRay(touch.position);
        if (plane.Raycast(rayBefore, out var enterBefore) && plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        // not on plane
        return Vector3.zero;
    }
}
