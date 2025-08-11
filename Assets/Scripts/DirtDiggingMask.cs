using UnityEngine;

public class DirtDiggingMask : MonoBehaviour
{
    public int _maskResolution = 1024;
    public int _digRadius = 20;
    public Transform[] _diggers;

    private Texture2D _maskTexture;
    private Color32[] _fullWhite;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Material materialInstance = new(spriteRenderer.sharedMaterial);
        spriteRenderer.material = materialInstance;

        _maskTexture = new(_maskResolution, _maskResolution, TextureFormat.RGBA32, false)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Point
        };

        _fullWhite = new Color32[_maskResolution * _maskResolution];
        for(int i = 0; i < _fullWhite.Length; i++)
        {
            _fullWhite[i] = Color.white;
        }
        _maskTexture.SetPixels32(_fullWhite);
        _maskTexture.Apply();

        spriteRenderer.material.SetTexture("_MaskTex", _maskTexture);
    }

    void Update()
    {
        foreach(Transform digger in _diggers)
        {
            DigAtWorldPosition(digger.position);
        }
    }

    public void DigAtWorldPosition(Vector2 worldPos)
    {
        // Convert world position to texture UV (0-1) except this doesn't actually get the correct position at all
        Vector3 localPosition = transform.InverseTransformPoint(worldPos);
        Vector2 uv = new(localPosition.x + 0.5f, localPosition.y + 0.5f);

        int centerX = Mathf.RoundToInt(uv.x * _maskResolution);
        int centerY = Mathf.RoundToInt(uv.y * _maskResolution);

        for(int y = -_digRadius; y <= _digRadius; y++)
        {
            for(int x = -_digRadius; x <= _digRadius; x++)
            {
                if(x * x + y * y <= _digRadius * _digRadius)
                {
                    int px = centerX + x;
                    int py = centerY + y;

                    if(px >= 0 && px < _maskResolution && py >= 0 && py < _maskResolution)
                    {
                        _maskTexture.SetPixel(px, py, Color.black);
                    }
                }
            }
        }
        _maskTexture.Apply();
    }
}