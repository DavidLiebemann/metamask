using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay
{
    public class ClickSound : MonoBehaviour
    {
        [SerializeField] private InputActionReference click;
        [SerializeField] private AudioClip toPlay;
        [SerializeField] private AudioSource player;

        private void OnEnable()
        {
            click.action.Enable();
        }

  

        private void Update()
        {
            if (click.action.WasPressedThisFrame())
            {
                player.PlayOneShot(toPlay);

            }
        }
    }
}