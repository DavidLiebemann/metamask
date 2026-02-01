using System;
using GamePlay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Drawing
{
    public class HoverSelectionBehaviour : MonoBehaviour
    {

        [SerializeField] private InputActionReference selectPressed;
        [SerializeField] private InputActionReference selectionPosition;
        [SerializeField] private LayerMask selectionLayer;

        private IInteractable _currentlyHovered;

        private void OnEnable()
        {
            selectPressed.action.Enable();
            selectionPosition.action.Enable();
            
        }

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
                                SetCurrentSelect(true);
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
}