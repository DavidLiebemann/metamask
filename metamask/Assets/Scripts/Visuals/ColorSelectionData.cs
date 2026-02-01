using UnityEngine;

namespace Visuals
{
    [CreateAssetMenu(fileName = "ColorSelection", menuName = "GAME/ColorSelection", order = 0)]
    public class ColorSelectionData : ScriptableObject
    {
        [SerializeField] private Color currentColor = Color.red;

        public Color CurrentColor
        {
            get => currentColor;
            set => currentColor = value;
        }
    }   
}