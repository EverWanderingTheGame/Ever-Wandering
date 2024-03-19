using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class TriggerEvent : MonoBehaviour
{
    public bool triggered = false;
    public bool TriggerOnce = false;
    public TriggerType triggerType;
    public TriggerObjectType triggerObjectType = TriggerObjectType.Player;  
    [Header("Custom Events")]
    [Space]
    public UnityEvent OnEnterTrigger;
    [Space]
    public UnityEvent OnExitTrigger;

    public Gizmo gizmo;

    [HideInInspector] public Collider2D TriggeredBy = null;

    private bool movePlayerToTheEnd;
    private Collider2D _collider;
    private SceneSettings sceneSettings;
    private CinemachineVirtualCamera _virtualCamera;
    

    void Awake()
    {
        sceneSettings = FindObjectOfType<SceneSettings>();
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    void FixedUpdate()
    {
        if (movePlayerToTheEnd)
        {
            FindAnyObjectByType<CharacterController2D>().Move(40 * Time.fixedDeltaTime, false, false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (
            (
                (triggerObjectType == TriggerObjectType.Any) ||
                (collider.gameObject.tag == "Player" && triggerObjectType == TriggerObjectType.Player && (collider is BoxCollider2D || collider is CircleCollider2D)) ||
                (collider.gameObject.tag == "Box" && triggerObjectType == TriggerObjectType.Box)
            ) &&
            triggered == false
        )
        {
            if (triggerType == TriggerType.Debug) // Debug
            {
                Debug.Log(triggerObjectType.ToString() + " Entered Debug Trigger");
            }
            else if (triggerType == TriggerType.EndLevel && triggerObjectType == TriggerObjectType.Player) // End Level 
            {
                sceneSettings.disablePlayerMovement = true;
                sceneSettings.disableHUD = true;
                sceneSettings.muteAudio = true;
                sceneSettings.applySceneSettings();

                movePlayerToTheEnd = true;
            }
            else if (triggerType == TriggerType.Death && triggerObjectType == TriggerObjectType.Player) // Dead
            {
                AudioManager.instance.Play("Death");
                GameManager.instance.playerDead();
            }
            else if (triggerType == TriggerType.DetachCamera && triggerObjectType == TriggerObjectType.Player) // Detach Camera
            {
                _virtualCamera.LookAt = null;
                _virtualCamera.Follow = null;
            }
            else if (triggerType == TriggerType.BoxVFX && triggerObjectType == TriggerObjectType.Box)
            {
                collider.gameObject.GetComponent<Box>().EnableVFX();
            }

            TriggeredBy = collider;
            OnEnterTrigger.Invoke();
            triggered = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (
            (triggerObjectType == TriggerObjectType.Any) ||
                (collider.gameObject.tag == "Player" && triggerObjectType == TriggerObjectType.Player && (collider is BoxCollider2D || collider is CircleCollider2D)) ||
            (collider.gameObject.tag == "Box" && triggerObjectType == TriggerObjectType.Box)
        )
        {
            if (triggerType == TriggerType.Debug)
            {
                Debug.Log("Player Exited Debug Trigger");
            }
            else if (triggerType == TriggerType.EndLevel)
            {
                LevelManager.instance.LoadScene(Utils.getSceneNameFromSceneReference(sceneSettings.nextScene));

                movePlayerToTheEnd = false;
            }
            else if (triggerType == TriggerType.BoxVFX && triggerObjectType == TriggerObjectType.Box)
            {
                collider.gameObject.GetComponent<Box>().DisableVFX();
            }

            OnExitTrigger.Invoke();
            triggered = TriggerOnce ? true : false;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (
            (triggerObjectType == TriggerObjectType.Any) ||
            (collider.gameObject.tag == "Player" && triggerObjectType == TriggerObjectType.Player && (collider is BoxCollider2D || collider is CircleCollider2D)) ||
            (collider.gameObject.tag == "Box" && triggerObjectType == TriggerObjectType.Box)
        )
        {
            if (triggerType == TriggerType.EndLevel)
            {
                _virtualCamera.LookAt = null;
                _virtualCamera.Follow = null;
            }
            else if (triggerType == TriggerType.ItemKillZone && collider.gameObject.tag != "Player")
            {
                Destroy(collider.gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!gizmo.displayGizmo) return;
        if (gizmo.showOnlyWhenSelected) return;

        if (_collider == null) _collider = GetComponent<Collider2D>();
        gizmo.gizmoColor.a = 0.05f;
        Gizmos.color = gizmo.gizmoColor;
        Gizmos.DrawCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);

        gizmo.gizmoColor.a = 1f;
        Gizmos.color = gizmo.gizmoColor;
        Gizmos.DrawWireCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);
    }

    void OnDrawGizmosSelected()
    {
        if (!gizmo.displayGizmo) return;
        if (!gizmo.showOnlyWhenSelected) return;

        if (_collider == null) _collider = GetComponent<Collider2D>();
        gizmo.gizmoColor.a = 0.1f;
        Gizmos.color = gizmo.gizmoColor;
        Gizmos.DrawCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);

        gizmo.gizmoColor.a = 1f;
        Gizmos.color = gizmo.gizmoColor;
        Gizmos.DrawWireCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);
    }
}

public enum TriggerType
{
    None,
    Debug,
    EndLevel,
    Death,
    DetachCamera,
    BoxVFX,
    ItemKillZone
}

public enum TriggerObjectType
{
    Any,
    Player,
    Box
}
