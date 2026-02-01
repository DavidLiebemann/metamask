using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class MouseDrawController : MonoBehaviour
{
    public int brushSize = 10;
    public Color brushColor = Color.black;
    public Texture2D brushTexture;
    [Range(0, 1)] public float brushHardness = 1f;

    [SerializeField] private InputActionReference drawAction;
    [SerializeField] private InputActionReference drawPosition;
    [SerializeField] private LayerMask drawLayer;

    
    

    Camera cam;
    Material gpuDrawerMaterial;

    private void Awake()
    {
        gpuDrawerMaterial = new Material(Shader.Find("Hidden/DrawOnTexture"));
        gpuDrawerMaterial.SetTexture("_BrushTexture", brushTexture);
        Assert.IsNotNull(drawAction);
        Assert.IsNotNull(drawPosition);
    }

    private void OnEnable()
    {
        drawAction.action.Enable();
        drawPosition.action.Enable();
    }

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(drawPosition.action.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out var hit, 100.0f, drawLayer))
        {
            if (hit.collider.TryGetComponent<DrawZone>(out var drawZone))
            {
                if (drawAction.action.IsPressed())
                {
                    drawZone.DrawTexture = DrawOnTextureGPU(drawZone.DrawTexture, hit.textureCoord);
                }
            }
        }
    }


    RenderTexture DrawOnTextureGPU(Texture src, Vector2 nrmPos)
    {
        int srcWidth = src.width;
        // TODO: Optimize this
        gpuDrawerMaterial.SetVector("_BrushPosition", nrmPos);
        gpuDrawerMaterial.SetFloat("_BrushSize", brushSize / (float)srcWidth);
        gpuDrawerMaterial.SetColor("_BrushColor", brushColor);

        RenderTexture copiedTexture = new RenderTexture(srcWidth, src.height, 32);
        Graphics.Blit(src, copiedTexture, gpuDrawerMaterial);
        DestroyImmediate(src);

        return copiedTexture;
    }
}