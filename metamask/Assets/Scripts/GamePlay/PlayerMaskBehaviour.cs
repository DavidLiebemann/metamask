using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace GamePlay
{
    public class PlayerMaskBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private MeshRenderer maskSurface;

        [SerializeField] private MaskModel maskData;

        [SerializeField] private Outline outline;
        

        private void Awake()
        {
            Assert.IsNotNull(maskSurface);
            Assert.IsNotNull(outline);
            Assert.IsNotNull(maskData);

            maskData.OnMaskChanged += OnMaskChanged;
        }

        private void OnEnable()
        {
            OnMaskChanged();
        }

        private void OnDisable()
        {
            if (outline)
            {
                outline.enabled = false;
            }
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

        public void OnSelect(GameObject originator, bool bIsSelected)
        {
            
        }

        public void OnHover(GameObject originator, bool bIsHovered)
        {
            if (outline)
            {
                outline.enabled = bIsHovered;
            }
        }
    }
}