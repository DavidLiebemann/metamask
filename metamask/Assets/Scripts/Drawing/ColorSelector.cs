using System;
using GamePlay;
using UnityEngine;
using UnityEngine.Assertions;

namespace Drawing
{
    [RequireComponent(typeof(Collider), typeof(Outline))]
    public class ColorSelector : MonoBehaviour, IInteractable
    {
        [SerializeField] private Color color;

        public Color DrawColor => color;

        private Outline _outline;
        private void Awake()
        {
            _outline = GetComponent<Outline>();
            Assert.IsNotNull(_outline);
        }

        private void OnEnable()
        {
            _outline.enabled = false;
        }


        public void OnSelect(GameObject originator, bool bIsSelected)
        {
            if (bIsSelected)
            {
                _outline.OutlineColor = Color.lawnGreen;
                _outline.OutlineWidth = 10.0f;
            }
            else
            {
                _outline.OutlineColor = Color.yellow;
                _outline.OutlineWidth = 5.0f;
            }
        }

        public void OnHover(GameObject originator, bool bIsHovered)
        {
            _outline.enabled = bIsHovered;
        }
    }
}