using UnityEngine;

namespace GamePlay
{
    public class ExitGame : MonoBehaviour
    {
        public void CloseGame()
        {
            Application.Quit();
        }
    }
}