using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace GamePlay
{
    public class PlayerMaskBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private MeshRenderer maskSurface;

        [SerializeField] private MaskModel maskData;

        [SerializeField] private Outline outline;

        [FormerlySerializedAs("feedback")] [SerializeField]
        private AFeedback selectionFeedback;

        public static Action<PlayerMaskBehaviour> onFinishedSelectionFeedback;
        private bool _currentSelection = false;

        public bool IsImposter => _bIsImposter;
        private bool _bIsImposter = false;

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

            selectionFeedback.onReachedEnd.AddListener(OnFeedbackFinished);
        }

        private void OnFeedbackFinished()
        {
            onFinishedSelectionFeedback?.Invoke(this);
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
            if (maskData && maskSurface)
            {
                maskSurface.material.mainTexture = _bIsImposter ? maskData.ImposterMask : maskData.MaskTexture;
            }
        }
        
        

        public void SetupMask(bool bIsImposter)
        {
            _bIsImposter = bIsImposter;
        }

        public void OnSelect(GameObject originator, bool bIsSelected)
        {
            if (bIsSelected == _currentSelection)
                return;

            if (bIsSelected)
            {
                Debug.Log($"Starting Feedback on {gameObject.name}");
                selectionFeedback.Play();
            }
            else
            {
                Debug.Log($"Reverting Feedback on {gameObject.name}");
                selectionFeedback.Revert();
            }

            _currentSelection = bIsSelected;
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