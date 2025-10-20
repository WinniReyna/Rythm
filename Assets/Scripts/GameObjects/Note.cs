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
    public string lastHitType;

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

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x >= 1.01f)
        {
            Miss();
        }
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












