using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class PlayerMaskBehaviour : MonoBehaviour, IInteractable
    {
        private static readonly int BaseMapId = Shader.PropertyToID("Base_Map");

        [SerializeField] private DecalProjector targetProjector;

        [SerializeField] private MaskModel maskData;

        [SerializeField] private Outline outline;

        [SerializeField] private Transform maskRoot;
        

        [FormerlySerializedAs("feedback")] [SerializeField]
        private AFeedback selectionFeedback;

        public static Action<PlayerMaskBehaviour> onFinishedSelectionFeedback;
        private bool _currentSelection = false;

        public bool IsImposter => _bIsImposter;
        private bool _bIsImposter = false;

        public virtual Texture MaskTexture
        {
            get => targetProjector.material.GetTexture(BaseMapId);
            set
            {
                targetProjector.material.SetTexture(BaseMapId, value);
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(targetProjector);
            Assert.IsNotNull(outline);
            Assert.IsNotNull(maskData);

            maskData.OnMaskTextureChanged += OnMaskChanged;
            maskData.OnMaskPrefabChanged += OnMaskPrefabChanged;
        }

        private void OnMaskPrefabChanged()
        {
            ActivateMask(maskData.SelectedMask);
        }

        private void OnEnable()
        {
            OnMaskChanged();
            ActivateMask(maskData.SelectedMask);

            selectionFeedback.onReachedEnd.AddListener(OnFeedbackFinished);
        }

        private void ActivateMask(string maskName)
        {
            Transform[] children =  maskRoot.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].parent == maskRoot)
                {
                    string gameObjectName = children[i].gameObject.name;
                    children[i].gameObject.SetActive(gameObjectName == maskName);
                }
            }
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
            
            maskData.OnMaskTextureChanged -= OnMaskChanged;
            maskData.OnMaskPrefabChanged -= OnMaskPrefabChanged;
        }

        private void OnDestroy()
        {
            maskData.OnMaskTextureChanged -= OnMaskChanged;
        }

        private void OnMaskChanged()
        {
            if (maskData && targetProjector)
            {
                MaskTexture = _bIsImposter ? maskData.ImposterMask : maskData.MaskTexture;
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