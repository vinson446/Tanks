using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameObject gameCamera;
    public GameObject focusCamera;

    // references

    // Start is called before the first frame update
    void Start()
    {
        LookAtGameWorld();
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
}
