using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadTrigger : MonoBehaviour
{
    public SceneReference SceneReference;

    [Header("Gizmo Settings")]
    [SerializeField] private bool _displayGizmo = true;
    [SerializeField] private bool _showOnlyWhenSelected = true;
    [SerializeField] private Color _gizmoColor = Color.green;

    private Collider2D _collider;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider is BoxCollider2D && collider.CompareTag("Player"))
        {
            LevelManager.instance.LoadScene(Utils.getSceneNameFromSceneReference(SceneReference));
        }
    }

    void OnDrawGizmos()
    {
        if (!_displayGizmo) return;
        if (_showOnlyWhenSelected) return;

        if (_collider == null) _collider = GetComponent<Collider2D>();
        _gizmoColor.a = 0.05f;
        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);

        _gizmoColor.a = 1f;
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);
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
