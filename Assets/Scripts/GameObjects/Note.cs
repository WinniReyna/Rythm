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
    private float movementDuration;
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
        
        // el tiempo de inicio es el spawnDspTime,
        // no el tiempo actual del frame
        double startTime = spawnDspTime;
        float journey = 0f;

        while (journey < 1f)
        {
            double elapsed = spawner.GetMusicTime() - startTime;
            journey = Mathf.Clamp01((float)(elapsed / movementDuration));

            transform.position = new Vector3(
                Mathf.Lerp(spawnPos.x, hitPos.x, journey),
                spawnPos.y,
                spawnPos.z
            );

            yield return null;
        }

        transform.position = hitPos;
        Miss();
    }


    public void InitializeMovement(double dspSpawn, Vector3 hitPosition, float speedOverride = -1f)
    {
        // Fijar posiciones spawn y hit
        spawnPos = transform.position;
        hitPos = hitPosition;

        // Distancia solo en X
        travelDistance = Mathf.Abs(hitPos.x - spawnPos.x);

        // Aplicar speedOverride si existe
        if (speedOverride > 0)
            speed = speedOverride;

        // Calcular duración exacta del movimiento
        movementDuration = travelDistance / speed;

        spawnDspTime = dspSpawn;
        initializedMovement = true;
    }

    public void PaintGridOnHit(string hitType)
    {
        if (gridPainter != null && gridX >= 0 && gridY >= 0 && paintSprite != null)
        {
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
        Destroy(gameObject);
    }
}













