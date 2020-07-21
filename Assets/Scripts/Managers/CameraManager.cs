using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameObject gameCamera;
    public GameObject focusCamera;

    // Start is called before the first frame update
    void Start()
    {
        gameCamera.SetActive(true);
        focusCamera.SetActive(false);
    }

    public void FocusOnTarget(Transform target)
    {

    }
}
