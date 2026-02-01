using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay
{
    public class WinDisplay : MonoBehaviour
    {
        [SerializeField] private string winMessage = "YOU FOUND YOUR PRINCE CHARMING!\nYou found the changed mask!";
        [SerializeField] private string looseMessage = "THE ONE THAT GOT AWAY...\nYou did not find the changed mask.";

        [SerializeField] private string winButtonMessage = "FIND ANOTHER";
        [SerializeField] private string looseButtonMessage = "TRY AGAIN";

        [SerializeField] private TMP_Text messageDisplay;
        [SerializeField] private TMP_Text buttonText;
        

        [FormerlySerializedAs("_gameplayStateBehaviour")] [SerializeField] private GameplayStateBehaviour gameplayStateBehaviour;

        
        private void OnEnable()
        {
            if (gameplayStateBehaviour.IsSelectionFinished)
            {
                buttonText.text = gameplayStateBehaviour.WasSelectionCorrect ? winButtonMessage : looseButtonMessage;
                messageDisplay.text = gameplayStateBehaviour.WasSelectionCorrect ? winMessage : looseMessage;
                
            }
        }
    }
}