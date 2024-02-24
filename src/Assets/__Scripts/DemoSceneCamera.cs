using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSceneCamera : MonoBehaviour
{
    [SerializeField] public GameObject cameraCutscene;

    CinemachineVirtualCamera _virtualCamera;
    Animator _animator;
    SceneSettings sceneSettings;
    bool _showOnce = false;

    public void Start()
    {
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _animator = cameraCutscene.GetComponent<Animator>();
        sceneSettings = FindObjectOfType<SceneSettings>();
    }

    public void ShowHole()
    {
        if(_showOnce) return;

        sceneSettings.cameraDeadZone = new Vector2(0, 0);
        sceneSettings.disablePlayerMovement = true;
        sceneSettings.applySceneSettings();
        FindObjectOfType<CharacterController2D>().Move(0, false, false);
        _virtualCamera.LookAt = cameraCutscene.transform;
        _virtualCamera.Follow = cameraCutscene.transform;
        cameraCutscene.transform.position = GameManager.instance.player.transform.position;
        _animator.SetTrigger("Start");
        Invoke("GetControl", 7.5f);

        _showOnce = true;
    }

    public void GetControl()
    {
        sceneSettings.cameraDeadZone = new Vector2(.2f, .1f);
        sceneSettings.disablePlayerMovement = false;
        sceneSettings.applySceneSettings();
    }
}
