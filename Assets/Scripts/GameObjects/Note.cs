using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    private Sprite[] paintSprites;
    private GridPosition[] paintPositions;

    private NoteKey requiredKey;
    private NoteSpawner spawner;
    private GridPainter gridPainter;

    private ScoreManager scoreManager;
    private string lastHitType;

    public Vector3 spawnPos;
    public Vector3 hitPos;
    private double spawnDspTime;
    private float travelDistance;
    private bool initializedMovement = false;
    private HitFeedbackUI hitFeedbackUI;
    public Vector3 endPos;

    [HideInInspector] public BeatNoteSpawner beatSpawner;
    public System.Action<Note> OnMissed;

    public bool WasHit { get; private set; }

    public void Initialize(NoteKey key, GridPosition[] positions, Sprite[] sprites = null)
    {
        requiredKey = key;
        paintSprites = sprites;
        paintPositions = positions;

        hitFeedbackUI = FindObjectOfType<HitFeedbackUI>();
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
        double movementStartTime = GetSongTime();

        float totalDistance =
            Vector3.Distance(spawnPos, hitPos) +
            Vector3.Distance(hitPos, endPos);

        float movementDuration = totalDistance / speed;

        while (true)
        {
            double elapsed = GetSongTime() - movementStartTime;
            float t = Mathf.Clamp01((float)(elapsed / movementDuration));

            if (t < Vector3.Distance(spawnPos, hitPos) / totalDistance)
            {
                // Antes del hit
                float localT = t / (Vector3.Distance(spawnPos, hitPos) / totalDistance);
                transform.position = Vector3.Lerp(spawnPos, hitPos, localT);
            }
            else
            {
                // Después del hit
                float localT = (t - (Vector3.Distance(spawnPos, hitPos) / totalDistance))
                             / (Vector3.Distance(hitPos, endPos) / totalDistance);

                transform.position = Vector3.Lerp(hitPos, endPos, localT);
            }

            if (t >= 1f)
                break;

            yield return null;
        }

        // SOLO aquí es Miss
        if (!WasHit)
            OnMissed?.Invoke(this);

        Miss();
    }



    public double GetSongTime()
    {
        if (spawner != null)
            return spawner.GetMusicTime(); // llama al método público de NoteSpawner
        return AudioSettings.dspTime;      // fallback
    }





    public void InitializeMovement(Vector3 spawnPosition, Vector3 hitPosition, double dspSpawn)
    {
        spawnPos = spawnPosition;      // posición real del spawn
        hitPos = hitPosition;          // hit point
        spawnDspTime = dspSpawn;
        travelDistance = Vector3.Distance(spawnPos, hitPos);
        initializedMovement = true;

        // Forzar posición inicial
        transform.position = spawnPos;
    }

    public void PaintGridOnHit(string hitType)
    {
        if (gridPainter == null)
            return;

        if (paintSprites == null || paintPositions == null)
            return;

        Color color = Color.white;

        switch (hitType)
        {
            case "Perfect!": color.a = 1f; break;
            case "Good!": color.a = 0.75f; break;
            case "Bad!": color.a = 0.6f; break;
        }

        int length = Mathf.Min(paintSprites.Length, paintPositions.Length);

        for (int i = 0; i < length; i++)
        {
            gridPainter.PaintCellWithSprite(
                paintPositions[i].x,
                paintPositions[i].y,
                paintSprites[i],
                color
            );
        }
    }

    public void Hit()
    {
        WasHit = true;
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












