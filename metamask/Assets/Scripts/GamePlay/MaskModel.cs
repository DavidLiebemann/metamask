using System;
using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "MaskModel", menuName = "GAME/MaskModel")]
    public class MaskModel : ScriptableObject
    {
        private Texture2D _maskTexture;

        private string _selectedMask;
        public string SelectedMask
        {
            get
            {
                return _selectedMask;
            }
            set
            {
                _selectedMask = value;
                OnMaskPrefabChanged?.Invoke();
            }
        }

        public Texture2D MaskTexture
        {
            get => _maskTexture;
            set
            {
                _maskTexture = value;
                if(null != OnMaskTextureChanged)
                {
                    OnMaskTextureChanged.Invoke();
                }
            }
        }

        private Texture2D _imposterMask;
        public Texture2D ImposterMask
        {
            get => _imposterMask;
            set
            {
                _imposterMask = value;
                OnMaskTextureChanged?.Invoke();
            }
        }

        public Action OnMaskTextureChanged;
        public Action OnMaskPrefabChanged;
    }
}

