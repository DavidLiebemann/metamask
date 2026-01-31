using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace GamePlay
{
    public class PlayerMaskBehaviour : MonoBehaviour
    {
        [SerializeField] private MeshRenderer maskSurface;

        [SerializeField] private MaskModel maskData;

        private void Awake()
        {
            Assert.IsNotNull(maskSurface);
            Assert.IsNotNull(maskData);

            maskData.OnMaskChanged += OnMaskChanged;
        }

        private void OnEnable()
        {
            OnMaskChanged();
        }

        private void OnDestroy()
        {
            maskData.OnMaskChanged -= OnMaskChanged;
        }

        private void OnMaskChanged()
        {
            if (maskData && maskData.MaskTexture && maskSurface)
            {
                maskSurface.material.mainTexture = maskData.MaskTexture;
            }
        }
    }
}