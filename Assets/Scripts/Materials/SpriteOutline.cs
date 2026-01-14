using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutline : MonoBehaviour
{
    public Color outlineColor = Color.black;
    [Range(0, 5)] public float outlineSize = 1f;
    public bool enableOutline = true;

    SpriteRenderer sr;
    MaterialPropertyBlock mpb;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();

        EnableOutline(false);
    }

    void LateUpdate()
    {
        sr.GetPropertyBlock(mpb);

        if (enableOutline)
        {
            mpb.SetColor("_OutlineColor", outlineColor);
            mpb.SetFloat("_OutlineSize", outlineSize);
        }
        else
        {
            mpb.SetColor("_OutlineColor", new Color(0, 0, 0, 0));
        }

        sr.SetPropertyBlock(mpb);
    }

    public void EnableOutline(bool enabled)
    {
        this.enableOutline = enabled;
        // Para aplicar inmediatamente al material
        if (sr != null)
        {
            var mpb = new MaterialPropertyBlock();
            sr.GetPropertyBlock(mpb);
            mpb.SetColor("_OutlineColor", enabled ? outlineColor : new Color(0, 0, 0, 0));
            sr.SetPropertyBlock(mpb);
        }
    }

}

