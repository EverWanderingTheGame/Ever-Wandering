using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
using UnityEngine.Diagnostics;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
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
    public GameObject HUD;
    public float LightPowerMax = 100;
    public float LightPowerChargeSpeed = 2f;
    public float LightPowerDischargeSpeed = 1f;
    public TMPro.TextMeshProUGUI Text;
    public GameObject ChargingText;
    [Header("Other Settings")]
    [Range(1, 100)] public float ConnenctionDistance = 5;
    [Range(1, 10)] public float ColorIntensity = 6;

    public static float LightPower;
    public static GameObject[] AllObjects;
    public static bool flip = false;

    private GameObject NearestObject;
    private Light2D NearestLight;
    private Color VFXColor;
    float Distance;
    float NearestDistance = float.PositiveInfinity;

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (BuildPipeline.isBuildingPlayer) return;
#endif

        updateAllObjects();
    }

    void Update()
    {
        if (AllObjects != null)
        {
            for (int i = 0; i < AllObjects.Length; i++)
            {
                Distance = Vector3.Distance(this.transform.position, AllObjects[i].transform.position);

                if (Distance < NearestDistance)
                {
                    NearestDistance = Distance;
                    NearestObject = AllObjects[i];
                }
            }
        }
        else updateAllObjects();

        if (NearestDistance <= ConnenctionDistance)
        {
            NearestLight = NearestObject.GetComponent<Light2D>();

            VFXColor = NearestLight.color * ColorIntensity;
            VFXColor.a = 1 - NearestDistance / (ConnenctionDistance - .5f);
            // Pos1
            Pos1.transform.position = Vector3.Lerp(Pos1.transform.position, this.transform.position, 10 * Time.deltaTime);
            Pos1.GetComponent<Light2D>().color = NearestLight.color;
            Pos1.GetComponent<Light2D>().intensity = VFXColor.a;
            // Pos2
            Vector3 pos2 = Vector3.Lerp(Pos1.transform.position, Pos4.transform.position, 0.35f);
            Pos2.transform.position = pos2;
            Pos2.transform.position = new Vector3(Pos2.transform.position.x, Pos2.transform.position.y, 0);
            Pos2.GetComponent<Light2D>().color = NearestLight.color;
            Pos2.GetComponent<Light2D>().intensity = VFXColor.a * 1.2f;
            // Pos3
            Vector3 pos3 = Vector3.Lerp(Pos1.transform.position, Pos4.transform.position, 0.65f);
            Pos3.transform.position = pos3;
            Pos3.transform.position = new Vector3(Pos3.transform.position.x, Pos3.transform.position.y, 0);
            Pos3.GetComponent<Light2D>().color = NearestLight.color;
            Pos3.GetComponent<Light2D>().intensity = VFXColor.a * 1.2f;
            // Pos4
            Pos4.transform.position = Vector3.Lerp(Pos4.transform.position, NearestObject.transform.position, 20 * Time.deltaTime);

            for (int i = 0; i < ArcVFX.Length; i++)
            {
                ArcVFX[i].SetVector4("Color", VFXColor);
                ArcVFX[i].SetFloat("Alpha", VFXColor.a);
            }

            if (flip)
            {
                Utils.Unflip(Arc);
                flip = false;
            }

            if (LightPowerMax >= LightPower)
            {
                LightPower += LightPowerChargeSpeed * Time.deltaTime;
                ChargingText.SetActive(true);
            }

            Arc.SetActive(true);
        }
        else if (NearestDistance > ConnenctionDistance)
        {
            Arc.SetActive(false);

            if (0 <= LightPower)
            {
                LightPower -= LightPowerDischargeSpeed * Time.deltaTime;
                ChargingText.SetActive(false);
            }
        }

        if (HUD.activeInHierarchy == true)
        {
            Text.text = "Light " + (LightPower * 100 / LightPowerMax).ToString("F0") + "%";
            HUD.GetComponent<Animator>().SetFloat("LightPower", LightPower);
        }

        NearestDistance = float.PositiveInfinity;
    }

    public static void updateAllObjects()
    {
        AllObjects = GameObject.FindGameObjectsWithTag("Light");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TrailManager))]
public class TrailManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TrailManager trailManager = (TrailManager)target;

        GUI.backgroundColor = new Color(85f, 177f, 85f, 128f) / 255f * new Vector4(5, 5, 5, 1);
        if (GUILayout.Button("Update All Objects", GUILayout.Height(25)))
        {
            TrailManager.updateAllObjects();
            Debug.Log("Updated Objects(" + TrailManager.AllObjects.Length.ToString() + ")");
        }
    }
}
#endif
