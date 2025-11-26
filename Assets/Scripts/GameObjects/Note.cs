using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    public Sprite paintSprite;

    private NoteKey requiredKey;
    private int gridX;
    private int gridY;
    private NoteSpawner spawner;
    private GridPainter gridPainter;

    private ScoreManager scoreManager;
    private string lastHitType;

    public Vector3 spawnPos;
    public Vector3 hitPos;
    private double spawnDspTime;
    private float travelDistance;
    private bool initializedMovement = false;

    public void Initialize(NoteKey key, int x = -1, int y = -1, Sprite sprite = null)
    {
        requiredKey = key;
        gridX = x;
        gridY = y;
        paintSprite = sprite;

        spawner = FindObjectOfType<NoteSpawner>();
        gridPainter = FindObjectOfType<GridPainter>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void StartMovement()
    {
        if (!initializedMovement) return;
        StartCoroutine(MoveNoteCoroutine());
    }

    private IEnumerator MoveNoteCoroutine()
    {
        // Esperar hasta el momento de spawn
        double waitTime = spawnDspTime - AudioSettings.dspTime;
        if (waitTime > 0)
            yield return new WaitForSecondsRealtime((float)waitTime);

        // Calcular duración real del viaje
        float movementDuration = travelDistance / speed;
        double movementStartTime = AudioSettings.dspTime;

        // Mover la nota
        float journey = 0f;
        while (journey < 1f)
        {
            double elapsed = AudioSettings.dspTime - movementStartTime;
            journey = Mathf.Clamp01((float)(elapsed / movementDuration));
            transform.position = Vector3.Lerp(spawnPos, hitPos, journey);
            yield return null;
        }

        // Llegó al hit → Miss
        transform.position = hitPos;
        Miss();
    }


    public void InitializeMovement(double dspSpawn, Vector3 hitPosition)
    {
        spawnPos = transform.position;   // posición actual de spawn
        hitPos = hitPosition;            // posición del hit point
        spawnDspTime = dspSpawn;
        travelDistance = Vector3.Distance(spawnPos, hitPos);
        initializedMovement = true;
    }


    public void PaintGridOnHit(string hitType)
    {
        if (gridPainter != null && gridX >= 0 && gridY >= 0 && paintSprite != null)
        {
            // Crear una copia del sprite con alpha ajustado según el hit
            Color color = Color.white;

            switch (hitType)
            {
                case "Perfect!": color.a = 1f; break;
                case "Good!": color.a = 0.7f; break;
                case "Bad!": color.a = 0.05f; break;
            }

            gridPainter.PaintCellWithSprite(gridX, gridY, paintSprite, color);
        }
    }

    public void Hit()
    {
        spawner?.UnregisterNote(this);
        spawner.notesDestroyed++;
        Destroy(gameObject);
    }

    public void HitSlider()
    {
        spawner?.UnregisterNote(this);
        Destroy(gameObject);
    }

    public void Miss()
    {
        spawner?.UnregisterNote(this);
        spawner.notesDestroyed++;
        Destroy(gameObject);
    }
}












