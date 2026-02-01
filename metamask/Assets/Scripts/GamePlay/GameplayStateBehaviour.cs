using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class GameplayStateBehaviour : MonoBehaviour
    {
        [SerializeField] private MaskModel maskModel;

        [SerializeField] private DrawZone drawZone;
        [SerializeField] private string danceRoomSceneName;
        [SerializeField] private UnityEvent<GameplayStateBehaviour> onSelectionFinished;
        

        private Texture2D _gameplayMaskTexture;
        private Scene _loadedScene;
        
        public bool IsSelectionFinished { get; private set; }
        public bool WasSelectionCorrect { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(drawZone);
            Assert.IsNotNull(maskModel);
        }
        
        

        private void OnFinishedSelectionFeedback(PlayerMaskBehaviour caller)
        {
            IsSelectionFinished = true;
            WasSelectionCorrect = caller.IsImposter;
            if (caller.IsImposter)
            {
                Debug.Log("YOU WON THATS CRAZY");
            }
            else
            {
                Debug.Log("NOPE");
            }
            onSelectionFinished.Invoke(this);
        }

        private void OnEnable()
        {
            IsSelectionFinished = false;
            WasSelectionCorrect = false;
            
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
            PlayerMaskBehaviour.onFinishedSelectionFeedback += OnFinishedSelectionFeedback;
            
            var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
            _loadedScene = SceneManager.LoadScene(danceRoomSceneName, parameters);
        }

        private void SceneManagerOnsceneLoaded(Scene _loadedScene, LoadSceneMode arg1)
        {
            StartGameplay();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;

            if (_loadedScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(_loadedScene);
            }

            PlayerMaskBehaviour.onFinishedSelectionFeedback -= OnFinishedSelectionFeedback;
        }

        private Texture2D CreateCopy(Texture src)
        {
            Texture2D copy = new Texture2D(src.width, src.height,
                src.graphicsFormat, src.mipmapCount, TextureCreationFlags.None);
            Graphics.CopyTexture(src, copy);
            return copy;
        }


        public void StartGameplay()
        {
            SetupDancerMasks();
            if (drawZone)
            {
                maskModel.MaskTexture = CreateCopy(drawZone.FinalSavedTexture);
                Texture2D imposterMask = CreateCopy(drawZone.FinalSavedTexture);


                maskModel.ImposterMask = imposterMask;
            }
        }

        public void SetupDancerMasks()
        {
            PlayerMaskBehaviour[] masks = FindObjectsByType<PlayerMaskBehaviour>(FindObjectsSortMode.None);

            if (masks.Length > 0)
            {
                int randomIndex = Random.Range(0, masks.Length);

                for (int i = 0; i < masks.Length; i++)
                {
                    masks[i].SetupMask(i == randomIndex);
                }
            }
            else
            {
                Debug.LogError("No Masks found!!");
            }
        }
    }
}