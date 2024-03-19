using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gizmo
{
    [Header("Gizmo Settings")]
    public bool displayGizmo = true;
    public bool showOnlyWhenSelected = true;
    public Color gizmoColor = Color.green;
}
