using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatteryDiplay : MonoBehaviour
{
    public Image cooldown;
    public bool batteryCoolingDown;
    public float batteryWaitTime = 60.0f;

    void Update()
    {  
        if (batteryCoolingDown == true)
        {
            cooldown.fillAmount -= 1.0f / batteryWaitTime * Time.deltaTime;
        }
    }
}
