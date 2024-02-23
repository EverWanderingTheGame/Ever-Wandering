using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    public static void Unflip(GameObject Object)
    {
        Vector3 theScale = Object.transform.localScale;
        theScale.x *= -1;
        Object.transform.localScale = theScale;
    }

    public static GameObject GetNearestObject(GameObject[] AllObjects, GameObject ThisObject)
    {
        GameObject NearestObject = null;
        float NearestDistance = float.PositiveInfinity;

        for (int i = 0; i < AllObjects.Length; i++)
        {
            float Distance = Vector3.Distance(ThisObject.transform.position, AllObjects[i].transform.position);

            if (Distance < NearestDistance)
            {
                NearestDistance = Distance;
                NearestObject = AllObjects[i];
            }
        }

        return NearestObject;
    }

    public static void TeleportPlayerToSceneSettings()
    {
        GameObject SpawnPoint = GameObject.Find("SceneSettings");
        GameObject Player = GameManager.instance.player;
        if (SpawnPoint == null)
        {
            Debug.LogError("SceneSettings NOT FOUND");
            return;
        }
        Player.transform.position = SpawnPoint.transform.position;
    }

    public static IEnumerator CoroutineScreenshot()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshot.ReadPixels(rect, 0, 0);
        screenshot.Apply();

        byte[] byteArray = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshot.png", byteArray);
    }

    public static string FixSceneName(string scene)
    {
        return scene.Split('/').Last().ReplaceLast(".unity", "");
    }

    public static string ReplaceLast(this string Source, string Find, string Replace)
    {
        int place = Source.LastIndexOf(Find);

        if (place == -1)
            return Source;

        string result = Source.Remove(place, Find.Length).Insert(place, Replace);
        return result;
    }

    public static string RemoveLast(this string Source, string Find)
    {
        return ReplaceLast(Source, Find, "");
    }
}
