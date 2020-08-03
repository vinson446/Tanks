using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpBar;
    public float updateSpeedSeconds = 0.2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void ShowHealthChange(float percent)
    {
        StartCoroutine(HealthChange(percent));
    }

    IEnumerator HealthChange(float afterChangePercent)
    {
        float preChangePercent = hpBar.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            hpBar.fillAmount = Mathf.Lerp(preChangePercent, afterChangePercent, elapsed / updateSpeedSeconds);
            yield return null;
        }

        hpBar.fillAmount = afterChangePercent;
    }
}
