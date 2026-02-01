using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawZone : MonoBehaviour
{
    [SerializeField] private Vector2Int textureDetail = new Vector2Int(32, 32);

    [SerializeField] private MeshRenderer targetRenderer;

    public MeshRenderer MeshRenderer
    {
        get => targetRenderer;
        private set { targetRenderer = value; }
    }

    public virtual Texture DrawTexture
    {
        get => MeshRenderer.material.mainTexture;
        set => MeshRenderer.material.mainTexture = value;
    }

    Texture initialTexture;

    protected virtual void Awake()
    {
        if (!MeshRenderer)
            MeshRenderer = GetComponent<MeshRenderer>();
        initialTexture = DrawTexture;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        CreateTexture();
    }

    public void CreateTexture()
    {
        if (!initialTexture)
        {
            initialTexture = new Texture2D(textureDetail.x, textureDetail.y);
            var tex2D = (Texture2D)initialTexture;
            var fillColorArray = tex2D.GetPixels();

            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = Color.white;
            }

            tex2D.SetPixels(fillColorArray);
            tex2D.Apply();
        }

        initialTexture.filterMode = FilterMode.Point;

        RenderTexture rt = new RenderTexture(textureDetail.x, textureDetail.y, 0);
        rt.filterMode = FilterMode.Point;
        rt.autoGenerateMips = false;

        RenderTexture.active = rt;
        Graphics.Blit(initialTexture, rt);

        DrawTexture = rt;
    }
}