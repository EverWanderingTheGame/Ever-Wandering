using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
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
}
