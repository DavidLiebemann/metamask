using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GamePlay
{
    public class WinDisplay : MonoBehaviour
    {
        [SerializeField] private string winMessage = "YOU FOUND YOUR PRINCE CHARMING!\nYou found the changed mask!";
        [SerializeField] private string looseMessage = "THE ONE THAT GOT AWAY...\nYou did not find the changed mask.";
        [SerializeField] private string timesUpMessage = "TIMES UP...\nYou did not find the changed mask.";
        

        [SerializeField] private string winButtonMessage = "FIND ANOTHER";
        [SerializeField] private string looseButtonMessage = "TRY AGAIN";

        [SerializeField] private TMP_Text messageDisplay;
        [SerializeField] private TMP_Text buttonText;

        [SerializeField] private Sprite winScreen;
        [SerializeField] private Sprite looseScreen;
        [SerializeField] private Image background;
        
        

        [FormerlySerializedAs("_gameplayStateBehaviour")] [SerializeField] private GameplayStateBehaviour gameplayStateBehaviour;

        
        private void OnEnable()
        {
            buttonText.text = gameplayStateBehaviour.WasSelectionCorrect ? winButtonMessage : looseButtonMessage;
            
            if (gameplayStateBehaviour.IsSelectionFinished)
            {
                messageDisplay.text = gameplayStateBehaviour.WasSelectionCorrect ? winMessage : looseMessage;
            }
            else
            {
                messageDisplay.text = timesUpMessage;
            }

            background.sprite = gameplayStateBehaviour.WasSelectionCorrect ? winScreen : looseScreen;
        }
    }
}