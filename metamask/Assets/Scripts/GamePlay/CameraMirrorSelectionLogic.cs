using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CameraMirrorSelectionLogic : MonoBehaviour
{
    private CameraMirror _activeMirror;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene loadedScene, LoadSceneMode arg1)
    {
        CameraMirror[] allCameraMirrors =
            FindObjectsByType<CameraMirror>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int numCameras = allCameraMirrors.Length;
        if (numCameras > 0)
        {
            int randomIndex = Random.Range(0, numCameras);
            for (int i = 0; i < numCameras; i++)
            {
                allCameraMirrors[i].SetMirrorUsed(randomIndex == i);
            }

            _activeMirror = allCameraMirrors[randomIndex];
        }
    }

    private void SwitchToMirror(CameraMirror target)
    {
        CameraMirror[] allCameraMirrors =
            FindObjectsByType<CameraMirror>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CameraMirror mirror in allCameraMirrors)
        {
            mirror.SetMirrorUsed(target == mirror);
        }

        _activeMirror = target;
    }

    private void Update()
    {
        if (_activeMirror)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Ray ray = _activeMirror.HandledCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out var hit))
                {
                    CameraMirror clickedMirror = hit.collider.GetComponent<CameraMirror>();
                    if (clickedMirror)
                    {
                        SwitchToMirror(clickedMirror);
                    }
                }
            }
        }
    }
}