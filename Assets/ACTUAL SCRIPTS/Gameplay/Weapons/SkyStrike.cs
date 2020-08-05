using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStrike : MonoBehaviour
{
    public GameObject skyShit;
    public bool forever = true;

    // Start is called before the first frame update
    void Start()
    {
        skyShit = Resources.Load<GameObject>("SkyStrike Missile");
        StartCoroutine(DropEm());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DropEm()
    {
        while (forever)
        {
            GameObject missile = Instantiate(skyShit, transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));

            yield return new WaitForSeconds(0.25f);
        }
    }
}
