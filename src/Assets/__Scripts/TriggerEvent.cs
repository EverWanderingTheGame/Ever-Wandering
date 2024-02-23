using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class TriggerEvent : MonoBehaviour
{
    public TriggerType triggerType;
    [Header("Custom Events")]
    [Space]
    public UnityEvent OnEnterTrigger;

    [Header("Gizmo Settings")]
    [SerializeField] private bool _displayGizmo = true;
    [SerializeField] private bool _showOnlyWhenSelected = true;
    [SerializeField] private Color _gizmoColor = Color.green;

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
        if(movePlayerToTheEnd)
        {
            FindAnyObjectByType<CharacterController2D>().Move(35 * Time.fixedDeltaTime, false, false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && collider.GetType() == typeof(BoxCollider2D))
        {
            if (triggerType == TriggerType.Debug)
            {
                Debug.Log("Player Entered Debug Trigger");
            } else if (triggerType == TriggerType.EndLevel) {
                sceneSettings.disablePlayerMovement = true;
                sceneSettings.disableHUD = true;
                sceneSettings.muteAudio = true;
                sceneSettings.applySceneSettings();

                movePlayerToTheEnd = true;
            }

            OnEnterTrigger.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.GetType() == typeof(BoxCollider2D))
        {
            if (triggerType == TriggerType.Debug)
            {
                Debug.Log("Player Exited Debug Trigger");
            } else if (triggerType == TriggerType.EndLevel)
            {
                LevelManager.instance.LoadScene(Utils.FixSceneName(sceneSettings.nextScene.ScenePath));

                movePlayerToTheEnd = false;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.GetType() == typeof(BoxCollider2D))
        {
            if (triggerType == TriggerType.EndLevel)
            {
                _virtualCamera.LookAt = null;
                _virtualCamera.Follow = null;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!_displayGizmo) return;
        if (_showOnlyWhenSelected) return;

        if (_collider == null) _collider = GetComponent<Collider2D>();
        _gizmoColor.a = 0.1f;
        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);

        _gizmoColor.a = 1f;
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireCube(_collider.transform.position +  new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);
    }

    void OnDrawGizmosSelected()
    {
        if (!_displayGizmo) return;
        if (!_showOnlyWhenSelected) return;

        if (_collider == null) _collider = GetComponent<Collider2D>();
        _gizmoColor.a = 0.1f;
        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);

        _gizmoColor.a = 1f;
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);
    }
}

public enum TriggerType
{
    None,
    Debug,
    EndLevel,
    Dead,
}
