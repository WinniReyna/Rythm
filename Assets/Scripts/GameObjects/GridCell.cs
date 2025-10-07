using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridCell : MonoBehaviour
{
    private SpriteRenderer sr;
    private Sprite defaultSprite;
    private Color defaultColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        defaultSprite = sr.sprite; // guarda el sprite que tiene el prefab
        defaultColor = sr.color;   // guarda el color inicial
    }

    /// <summary>
    /// Cambia el sprite de la celda
    /// </summary>
    public void SetSprite(Sprite sprite)
    {
        if (sr != null && sprite != null)
            sr.sprite = sprite;
    }

    /// <summary>
    /// Resetea la celda a su sprite y color originales
    /// </summary>
    public void ResetCell()
    {
        if (sr != null)
        {
            sr.sprite = defaultSprite;
            sr.color = defaultColor;
        }
    }
}
