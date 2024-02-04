using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class TrailManager : MonoBehaviour
{
    [Header("Arc Settings")]
    public GameObject Arc;
    public VisualEffect[] ArcVFX;
    public GameObject Pos1;
    public GameObject Pos2;
    public GameObject Pos3;
    public GameObject Pos4;

    [Header("LightPower")]
    public Animator HUD;
    public float LightPowerMax = 100;
    public float LightPowerChargeSpeed = 2f;
    public float LightPowerDischargeSpeed = 1f;
    public TMPro.TextMeshProUGUI Text;
    public GameObject ChargingText;

    [Header("Other Settings")]
    public float ConnenctionDistance = 5;

    [Header("DEBUG INFO (DO NOT CHANGE)")]
    public GameObject[] AllObjects;
    public GameObject NearestObject;
    public Color VFXColor;

    public static float LightPower;
    public static bool whenActiveted = false;

    float Distance;
    float NearestDistance = 10000;
    private float time = 0f;
    private Light2D NearestLight;

    void Update()
    {
        if (!whenActiveted)
        {
            time += Time.deltaTime;
            if (time >= .1f)
            {
                updateAllObjects();
                time = 0f;
                whenActiveted = true;
            }
        } else
        {
            for (int i = 0; i < AllObjects.Length; i++)
            {
                Distance = Vector3.Distance(this.transform.position, AllObjects[i].transform.position);

                if (Distance < NearestDistance)
                {
                    NearestDistance = Distance;
                    NearestObject = AllObjects[i];
                    NearestLight = NearestObject.GetComponent<Light2D>();
                }
            }

            if (NearestDistance <= ConnenctionDistance)
            {
                Pos1.transform.position = this.transform.position;
                Pos1.GetComponent<Light2D>().color = NearestLight.color;
                Pos4.transform.position = NearestObject.transform.position;

                Vector3 pos2 = Vector3.Lerp(Pos1.transform.position, Pos4.transform.position, 0.35f);
                Pos2.transform.position = pos2;
                Pos2.transform.position = new Vector3(Pos2.transform.position.x + .5f, Pos2.transform.position.y + -.5f, 0);
                Pos2.GetComponent<Light2D>().color = NearestLight.color;

                Vector3 pos3 = Vector3.Lerp(Pos1.transform.position, Pos4.transform.position, 0.65f);
                Pos3.transform.position = pos3;
                Pos3.transform.position = new Vector3(Pos3.transform.position.x + -.5f, Pos3.transform.position.y + .5f, 0);
                Pos3.GetComponent<Light2D>().color = NearestLight.color;

                VFXColor = NearestLight.color;
                VFXColor.a = (NearestDistance / ConnenctionDistance - 1) * -1;

                if (LightPowerMax >= LightPower) LightPower += LightPowerChargeSpeed * Time.deltaTime;

                for (int i = 0; i < ArcVFX.Length; i++)
                {
                    ArcVFX[i].SetVector4("Color", VFXColor);
                    ArcVFX[i].SetFloat("Alpha", VFXColor.a);
                }

                Arc.SetActive(true);
                ChargingText.SetActive(true);
            }
            else if (NearestDistance > ConnenctionDistance)
            {
                Arc.SetActive(false);
                ChargingText.SetActive(false);

                if (0 <= LightPower) LightPower -= LightPowerDischargeSpeed * Time.deltaTime;
            }

            NearestDistance = 10000;
        }

        Text.text = "Light - " + LightPower.ToString("F0") + "%";
        HUD.SetFloat("LightPower", LightPower);
    }

    public void updateAllObjects()
    {
        AllObjects = GameObject.FindGameObjectsWithTag("Light");
    }
}
