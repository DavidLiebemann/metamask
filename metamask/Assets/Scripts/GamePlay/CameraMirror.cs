using System;
using GamePlay;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class CameraMirror : MonoBehaviour, IInteractable
{
    [Header("References")] [SerializeField]
    private Outline outline;

    [SerializeField] private GameObject watch;
    

    [SerializeField] private Camera handledCamera;


    public Camera HandledCamera => handledCamera;


    public static CameraMirror EnabledMirror { get; private set; }
    
    private void Awake()
    {
        Assert.IsNotNull(handledCamera);

        handledCamera.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnSelect(gameObject, false);
    }

    private void OnDisable()
    {
        OnSelect(gameObject, false);
    }

    private void SetMirrorUsed(bool bShouldBeUsed)
    {
        handledCamera.gameObject.SetActive(bShouldBeUsed);

        Renderer[] renderers = watch.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = bShouldBeUsed;
        }

        if (bShouldBeUsed)
        {
            EnabledMirror = this;
        }
    }


    private bool _previousSelected = false;
    public void OnSelect(GameObject originator, bool bIsSelected)
    {
        if (bIsSelected && _previousSelected != true)
        {
            CameraMirror[] allCameraMirrors =
                FindObjectsByType<CameraMirror>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (CameraMirror mirror in allCameraMirrors)
            {
                if (mirror != this)
                {
                    mirror.SetMirrorUsed(false);
                }
            }
            SetMirrorUsed(true);
        }
        _previousSelected = bIsSelected;

    }


    public void OnHover(GameObject originator, bool bIsHovered)
    {
        if (outline)
        {
            outline.enabled = bIsHovered;
        }
    }
}