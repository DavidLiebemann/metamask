using System;
using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "MaskModel", menuName = "GAME/MaskModel")]
    public class MaskModel : ScriptableObject
    {
        private Texture2D _maskTexture;

        public Texture2D MaskTexture
        {
            get => _maskTexture;
            set
            {
                _maskTexture = value;
                if(null != OnMaskChanged)
                {
                    OnMaskChanged.Invoke();
                }
            }
        }

        public Action OnMaskChanged;
    }
}

