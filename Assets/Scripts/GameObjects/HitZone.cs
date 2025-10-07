using UnityEngine;

public class HitZone : MonoBehaviour
{
    [Tooltip("Tecla que se debe presionar cuando una nota está dentro de la zona")]
    public NoteKey keyToPress;

    private IInputHandler inputHandler;
    private Note currentNote; // nota dentro del hitzone

    void Start()
    {
        inputHandler = new UnityInputHandler();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var note = collision.GetComponent<Note>();
        if (note != null)
        {
            currentNote = note;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var note = collision.GetComponent<Note>();
        if (note != null && note == currentNote)
        {
            currentNote = null;
        }
    }

    void Update()
    {
        if (currentNote != null && inputHandler.IsKeyPressed(keyToPress))
        {
            currentNote.Hit();            
        }
    }
}




