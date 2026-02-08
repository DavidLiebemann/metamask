using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.Universal;

public class DrawZone : MonoBehaviour
{
    private static readonly int BaseMapId = Shader.PropertyToID("Base_Map");
    [SerializeField] private Vector2Int textureDetail = new Vector2Int(32, 32);

    [SerializeField] private DecalProjector targetProjector;

    public DecalProjector TargetProjector
    {
        get => targetProjector;
        private set { targetProjector = value; }
    }

    public virtual Texture DrawTexture
    {
        get => TargetProjector.material.GetTexture(BaseMapId);
        set => TargetProjector.material.SetTexture(BaseMapId, value);
    }
    
    
    
    public Texture FinalSavedTexture { get; private set; }

    Texture initialTexture;

    protected virtual void Awake()
    {
        Assert.IsNotNull(TargetProjector);
        initialTexture = DrawTexture;
    }
    

    private void OnEnable()
    {
        StartCoroutine(ShortDelayBeforeTexture());
    }

    private IEnumerator ShortDelayBeforeTexture()
    {
        yield return new WaitForSeconds(0.1f);
        CreateTexture();
    }

    public void CreateTexture()
    {
        
        initialTexture = new Texture2D(textureDetail.x, textureDetail.y, DefaultFormat.LDR, 0, TextureCreationFlags.None);
        var tex2D = (Texture2D)initialTexture;
        var fillColorArray = tex2D.GetPixels();

        for (var i = 0; i < fillColorArray.Length; ++i)
        {
            fillColorArray[i] = new Color(0, 0, 0, 0);
        }

        tex2D.SetPixels(fillColorArray);
        tex2D.Apply();
        
        RenderTexture rt = new RenderTexture(textureDetail.x, textureDetail.y,0, RenderTextureFormat.ARGB32);
        rt.autoGenerateMips = false;
        rt.useMipMap = false;

        RenderTexture.active = rt;
        Graphics.Blit(initialTexture, rt);

        DrawTexture = rt;
    }

    private void OnDisable()
    {
        FinalSavedTexture = DrawTexture;
    }
}