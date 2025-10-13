using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    [Tooltip("Tecla que se debe presionar cuando una nota está dentro de la zona")]
    public NoteKey keyToPress;

    [Header("Sprites visuales de la zona (en orden)")]
    public List<SpriteRenderer> zoneSprites = new List<SpriteRenderer>();

    [Header("Colores")]
    public Color defaultColor = Color.gray;
    public Color hitColor = Color.white;

    private IInputHandler inputHandler;
    private Note currentNote;
    private int currentActiveIndex = 0;

    public HitSlider slider;

    void Start()
    {
        inputHandler = new UnityInputHandler();

        if (zoneSprites.Count == 0)
            zoneSprites.AddRange(GetComponentsInChildren<SpriteRenderer>());

        ResetZoneColor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var note = collision.GetComponent<Note>();
        if (note != null)
            currentNote = note;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var note = collision.GetComponent<Note>();
        if (note != null && note == currentNote)
            currentNote = null;
    }

    void Update()
    {
        // Detecta la tecla solo cuando hay nota en la zona
        if (currentNote != null && inputHandler.IsKeyPressed(keyToPress))
        {
            HandleHit(currentNote);
        }
    }

    private void HandleHit(Note note)
    {
        note.Hit(); // Ejecuta el impacto
        OnSuccessfulHit(); // Actualiza el sprite visual
    }

    private void OnSuccessfulHit()
    {
        if (currentActiveIndex < zoneSprites.Count)
        {
            var sprite = zoneSprites[currentActiveIndex];
            if (sprite != null)
                sprite.color = hitColor;

            currentActiveIndex++;
        }
    }

    public void ResetZoneColor()
    {
        foreach (var sprite in zoneSprites)
        {
            if (sprite != null)
                sprite.color = defaultColor;
        }
        currentActiveIndex = 0;
    }
}
