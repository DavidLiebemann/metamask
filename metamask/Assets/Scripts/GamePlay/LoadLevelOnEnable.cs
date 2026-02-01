using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class LoadLevelOnEnable : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        
        private void OnEnable()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}