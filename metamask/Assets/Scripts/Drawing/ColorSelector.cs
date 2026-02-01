using System;
using GamePlay;
using UnityEngine;
using UnityEngine.Assertions;
using Visuals;

namespace Drawing
{
    [RequireComponent(typeof(Collider), typeof(Outline))]
    public class ColorSelector : MonoBehaviour, IInteractable
    {
        [SerializeField] private Color color;
        [SerializeField] private ColorSelectionData selectionData;
        
        public Color DrawColor => color;

        private Outline _outline;
        private void Awake()
        {
            _outline = GetComponent<Outline>();
            Assert.IsNotNull(_outline);
            Assert.IsNotNull(selectionData);
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
                selectionData.CurrentColor = color;

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