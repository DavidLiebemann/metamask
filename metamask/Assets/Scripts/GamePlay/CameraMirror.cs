using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class CameraMirror : MonoBehaviour
{
    [SerializeField] private LayerMask selectionLayer;
    
    [SerializeField] private Camera handledCamera;
    [SerializeField] private InputActionReference selectPressed;
    [SerializeField] private InputActionReference selectionPosition;
    

    public Camera HandledCamera => handledCamera;
    
    private void Awake()
    {
        Assert.IsNotNull(handledCamera);
        Assert.IsNotNull(selectionPosition);
        Assert.IsNotNull(selectPressed);
        handledCamera.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        selectPressed.action.Enable();
        selectionPosition.action.Enable();
    }


    public void SetMirrorUsed(bool bShouldBeUsed)
    {
        handledCamera.gameObject.SetActive(bShouldBeUsed);
    }

    private void Update()
    {
        if (selectPressed.action.IsPressed())
        {
            Ray selectionRay = HandledCamera.ScreenPointToRay(selectionPosition.action.ReadValue<Vector2>());
            if (Physics.Raycast(selectionRay, out var hit,  1000.0f, selectionLayer))
            {
                Debug.Log($"Selected: {hit.collider.transform.parent.gameObject.name}");
            }
        }
       
    }

    private void OnMouseEnter()
    {
        Outline outline = GetComponent<Outline>();
        if (outline)
        {
            outline.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        Outline outline = GetComponent<Outline>();
        if (outline)
        {
            outline.enabled = false;
        }
    }
}
