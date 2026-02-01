using System;
using GamePlay;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CameraMirrorSelectionLogic : MonoBehaviour
{
    [Header("Input")] [SerializeField] private LayerMask selectionLayer;

    [SerializeField] private InputActionReference selectPressed;
    [SerializeField] private InputActionReference selectionPosition;


    private IInteractable _currentlyHovered;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
        Assert.IsNotNull(selectionPosition);
        Assert.IsNotNull(selectPressed);
    }

    private void OnEnable()
    {
        selectPressed.action.Enable();
        selectionPosition.action.Enable();
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
                allCameraMirrors[i].OnSelect(gameObject, randomIndex == i);
            }
        }
    }

    private float _lastSelection = 0.0f;

    private void Update()
    {
        if (Camera.allCamerasCount > 0)
        {
            Camera currentCamera = Camera.allCameras[0];
            if (currentCamera)
            {
                Ray selectionRay =
                    currentCamera.ScreenPointToRay(selectionPosition.action.ReadValue<Vector2>());
                if (Physics.Raycast(selectionRay, out var hit, 1000.0f, selectionLayer))
                {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != _currentlyHovered)
                    {
                        SetCurrentHover(false);
                    }

                    _currentlyHovered = interactable;
                    if (null != _currentlyHovered)
                    {
                        SetCurrentHover(true);
                        if (selectPressed.action.IsPressed())
                        {
                            if (Time.time - _lastSelection > 1.0f)
                            {
                                SetCurrentSelect(true);
                                _lastSelection = Time.time;
                            }
                        }
                        else
                        {
                            SetCurrentSelect(false);
                        }
                    }
                }
                else
                {
                    SetCurrentHover(false);
                    SetCurrentSelect(false);
                }
            }
        }
    }

    private void SetCurrentSelect(bool newValue)
    {
        if (null != _currentlyHovered)
        {
            _currentlyHovered.OnSelect(gameObject, newValue);
        }
    }

    private void SetCurrentHover(bool bNewValue)
    {
        if (null != _currentlyHovered)
        {
            _currentlyHovered.OnHover(gameObject, bNewValue);
            if (!bNewValue)
            {
                SetCurrentSelect(false);
            }
        }
    }
}