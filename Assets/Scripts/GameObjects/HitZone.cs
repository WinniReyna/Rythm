using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitZone : MonoBehaviour
{
    [Tooltip("Tecla que se debe presionar cuando una nota está dentro de la zona")]
    [SerializeField] private NoteKey keyToPress;

    [Tooltip("Feedback")]
    [SerializeField] private HitFeedbackUI hitFeedbackUI;
    public NoteKey keyToHandle;

    [Header("Sprites visuales de la zona")]
    [SerializeField] private List<SpriteRenderer> zoneSprites = new List<SpriteRenderer>();

    [Header("Colores")]
    [SerializeField] private Color defaultColor = Color.gray;
    [SerializeField] private Color hitColor = Color.white;

    [Header("Rangos de precisión")]
    [SerializeField] private float perfectRange = 0.5f;
    [SerializeField] private float badRange = 1.5f;
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
        {
            currentNote = note;
            note.OnMissed += HandleNoteMissed;
        }
    }

    public void SubscribeNote(Note note)
    {
        note.OnMissed += HandleNoteMissed;
    }

    private void HandleNoteMissed(Note note)
    {
        if (!note.WasHit)
        {
            scoreManager.AddHit(50, "Bad!");
            hitFeedbackUI.Show("Bad!", Color.red);
            note.PaintGridOnHit("Bad!");
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        var note = collision.GetComponent<Note>();
        if (note != null && note == currentNote)
        {
            note.OnMissed -= HandleNoteMissed;
            currentNote = null;
        }
    }


    void Update()
    {
        // Detecta la tecla solo cuando hay nota en la zona
        if (currentNote != null && inputHandler.IsKeyPressed(keyToPress))
            HandleHit(currentNote);
        
    }

    private void HandleHit(Note note)
    {
        float distance = Vector2.Distance(note.transform.position, transform.position);

        int points = 0;
        string hitType = "";
        Color feedbackColor = Color.white;

        if (distance <= perfectRange)
        {
            points = 300;
            hitType = "Perfect!";
            feedbackColor = Color.yellow;
        }
        else if (distance <= goodRange)
        {
            points = 150;
            hitType = "Good!";
            feedbackColor = Color.green;
        }
        else if (distance <= badRange)
        {
            points = 50;
            hitType = "Bad!";
            feedbackColor = Color.red;
        }
        else
        {
            return;
        }


        scoreManager.AddHit(points, hitType);

        hitFeedbackUI.Show(hitType, feedbackColor);

        note.Hit();
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
