using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Rendering;

public class SwitchToGamePlayBehaviour : MonoBehaviour
{

   [SerializeField] private DrawZone drawZone;
   [SerializeField] private MeshRenderer targetDisplay;
   
   private Texture2D _GameplayMaskTexture;
   
   private void Awake()
   {
      Assert.IsNotNull(drawZone);
   }

   private void OnEnable()
   {
      StartGameplay();
   }


   public void StartGameplay()
   {
      if (drawZone)
      {
         var drawZoneDrawTexture = drawZone.DrawTexture;
         _GameplayMaskTexture = new Texture2D(drawZoneDrawTexture.width, drawZoneDrawTexture.height, drawZoneDrawTexture.graphicsFormat, drawZoneDrawTexture.mipmapCount, TextureCreationFlags.None);
         Graphics.CopyTexture(drawZoneDrawTexture, _GameplayMaskTexture);
         

         targetDisplay.material.mainTexture = _GameplayMaskTexture;
      }
   }
}
