using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitZone : MonoBehaviour
{
    [Tooltip("Tecla que se debe presionar cuando una nota está dentro de la zona")]
    [SerializeField] private NoteKey keyToPress;

    [Header("Sprites visuales de la zona (en orden)")]
    [SerializeField] private List<SpriteRenderer> zoneSprites = new List<SpriteRenderer>();

    [Header("Colores")]
    [SerializeField] private Color defaultColor = Color.gray;
    [SerializeField] private Color hitColor = Color.white;

    [Header("Rangos de precisión (en unidades del mundo)")]
    [SerializeField] private float perfectRange = 0.5f;
    private float goodRange = 1.0f;

    private IInputHandler inputHandler;
    private Note currentNote;
    private int currentActiveIndex = 0;

    private ScoreManager scoreManager;

    void Start()
    {
        inputHandler = new UnityInputHandler();
        scoreManager = FindObjectOfType<ScoreManager>();

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
        float distance = Vector2.Distance(note.transform.position, transform.position);

        int points = 0;
        string hitType = "";

        if (distance <= perfectRange)
        {
            points = 300;
            hitType = "Perfect!";
        }
        else if (distance <= goodRange)
        {
            points = 150;
            hitType = "Good!";
        }
        else
        {
            points = 50;
            hitType = "Bad!";
        }

        Debug.Log($"Hit: {hitType} | Distancia: {distance:F3}");

        scoreManager.AddHit(points, hitType);

        Debug.Log($"Perfect: {scoreManager.GetPerfectPercentage()}% " +
                $"Good: {scoreManager.GetGoodPercentage()}% " +
                $"Bad: {scoreManager.GetBadPercentage()}%");

        note.Hit(); // destruye la nota

        OnSuccessfulHit();
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
