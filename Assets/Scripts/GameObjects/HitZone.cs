using UnityEngine;

public class HitZone : MonoBehaviour
{
    [Tooltip("Tecla que se debe presionar cuando una nota está dentro de la zona")]
    public NoteKey keyToPress;

    [Tooltip("Slider que controla el valor 0 o 1")]
    public SliderController slider; // Debes tener un script que devuelva CurrentValue 0 o 1

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
            if (slider != null && currentNote.RequiredSliderValue == slider.CurrentValue)
            {
                currentNote.Hit();
                currentNote = null;
            }
            else
            {
                Debug.Log("Slider en valor incorrecto para esta nota");
            }
        }
    }
}




