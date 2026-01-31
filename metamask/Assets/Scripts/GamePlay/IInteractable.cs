using UnityEngine;

namespace GamePlay
{
    public interface IInteractable
    {
        public void OnSelect(GameObject originator, bool bIsSelected);

        public void OnHover(GameObject originator, bool bIsHovered);
    }
}