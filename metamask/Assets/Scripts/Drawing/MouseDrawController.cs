using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Visuals;

public class MouseDrawController : MonoBehaviour
{
    public int brushSize = 10;
    public Texture2D brushTexture;
    [Range(0, 1)] public float brushHardness = 1f;

    [SerializeField] private InputActionReference drawAction;
    [SerializeField] private InputActionReference drawPosition;
    [SerializeField] private LayerMask drawLayer;

    [SerializeField] private ColorSelectionData colorData;

    


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
        bIsFirstDraw = true;
    }

    private bool bIsFirstDraw = true;

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
                    DrawOnTextureGPU(drawZone.DrawTexture as RenderTexture, hit.textureCoord);
                }
            }
        }
    }

    private RenderTexture drawTarget;

    RenderTexture DrawOnTextureGPU(RenderTexture src, Vector2 nrmPos)
    {
        int srcWidth = src.width;

        if (!drawTarget)
        {
            drawTarget = new RenderTexture(srcWidth, src.height, 32);
        }

        // TODO: Optimize this
        gpuDrawerMaterial.SetVector("_BrushPosition", nrmPos);
        gpuDrawerMaterial.SetFloat("_BrushSize", brushSize / (float)srcWidth);
        gpuDrawerMaterial.SetColor("_BrushColor", colorData.CurrentColor);

        Graphics.Blit(src, drawTarget, gpuDrawerMaterial);
        Graphics.Blit(drawTarget, src);
        // Destroy(src);

        return drawTarget;
    }
}