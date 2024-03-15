using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerEvent))]
public class AddForceTrigger : MonoBehaviour
{
    public TriggerEvent triggerEvent;
    public Vector2 force = default;

    private void Awake()
    {
        triggerEvent = GetComponent<TriggerEvent>();
    }

    public void AddForce()
    {
        triggerEvent.TriggeredBy.GetComponent<Rigidbody2D>().AddForce(force);
    }
}
