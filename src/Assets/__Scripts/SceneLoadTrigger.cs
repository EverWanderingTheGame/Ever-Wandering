using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadTrigger : MonoBehaviour
{
    public SceneReference SceneReference;
    public CheckpointType StartFromCheckpoint = CheckpointType.End;

    public Gizmo _Gizmo;

    private Collider2D _collider;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider is BoxCollider2D && collider.CompareTag("Player"))
        {
            LevelManager.instance.LoadScene(Utils.getSceneNameFromSceneReference(SceneReference));
            GameManager.instance.checkpoint = StartFromCheckpoint;
        }
    }

    void OnDrawGizmos()
    {
        if (!_Gizmo.displayGizmo) return;
        if (_Gizmo.showOnlyWhenSelected) return;

        if (_collider == null) _collider = GetComponent<Collider2D>();
        _Gizmo.gizmoColor.a = 0.05f;
        Gizmos.color = _Gizmo.gizmoColor;
        Gizmos.DrawCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);

        _Gizmo.gizmoColor.a = 1f;
        Gizmos.color = _Gizmo.gizmoColor;
        Gizmos.DrawWireCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);
    }

    void OnDrawGizmosSelected()
    {
        if (!_Gizmo.displayGizmo) return;
        if (!_Gizmo.showOnlyWhenSelected) return;

        if (_collider == null) _collider = GetComponent<Collider2D>();
        _Gizmo.gizmoColor.a = 0.1f;
        Gizmos.color = _Gizmo.gizmoColor;
        Gizmos.DrawCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);

        _Gizmo.gizmoColor.a = 1f;
        Gizmos.color = _Gizmo.gizmoColor;
        Gizmos.DrawWireCube(_collider.transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0), _collider.bounds.size);
    }
}
