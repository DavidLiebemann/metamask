using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class SwitchToGamePlayBehaviour : MonoBehaviour
    {
        [SerializeField] private MaskModel maskModel;

        [SerializeField] private DrawZone drawZone;
        [SerializeField] private string danceRoomSceneName;


    
        private Texture2D _gameplayMaskTexture;
        private Scene _loadedScene;

        private void Awake()
        {
            Assert.IsNotNull(drawZone);
            Assert.IsNotNull(maskModel);
        }

        private void OnEnable()
        {
            StartGameplay();
        }

        private void OnDisable()
        {
            if (_loadedScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(_loadedScene);
            }
        }


        public void StartGameplay()
        {
            if (drawZone)
            {
                var drawZoneDrawTexture = drawZone.DrawTexture;
                _gameplayMaskTexture = new Texture2D(drawZoneDrawTexture.width, drawZoneDrawTexture.height,
                    drawZoneDrawTexture.graphicsFormat, drawZoneDrawTexture.mipmapCount, TextureCreationFlags.None);
                Graphics.CopyTexture(drawZoneDrawTexture, _gameplayMaskTexture);
                maskModel.MaskTexture = _gameplayMaskTexture;

                var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
                _loadedScene = SceneManager.LoadScene(danceRoomSceneName, parameters);
                
            }
        }
    }
}