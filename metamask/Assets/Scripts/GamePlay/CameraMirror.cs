using System;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraMirror : MonoBehaviour
{
    [SerializeField] private Camera handledCamera;

    public Camera HandledCamera => handledCamera;
    
    private void Awake()
    {
        Assert.IsNotNull(handledCamera);
        handledCamera.gameObject.SetActive(false);
    }


    public void SetMirrorUsed(bool bShouldBeUsed)
    {
        handledCamera.gameObject.SetActive(bShouldBeUsed);
    }
}
