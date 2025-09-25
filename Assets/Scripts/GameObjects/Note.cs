using UnityEngine;

public class Note : MonoBehaviour, INote
{
    public float speed = 5f;

    private NoteKey requiredKey;
    private ScoreManager scoreManager;
    private GridPainter gridPainter;
    private int gridX;
    private int gridY;
    private Color paintColor = Color.white;
    private bool allowEmptyPaint = false;

    public void Initialize(NoteKey key)
    {
        Initialize(key, 0, 0, Color.white);
    }

    public void Initialize(NoteKey key, int x = -1, int y = -1, Color? color = null, bool allowEmpty = false)
    {
        requiredKey = key;
        gridX = x;
        gridY = y;
        paintColor = color ?? Color.clear;
        allowEmptyPaint = allowEmpty;

        scoreManager = FindObjectOfType<ScoreManager>();
        gridPainter = FindObjectOfType<GridPainter>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Miss();
        }
    }

    public void Hit()
    {
        scoreManager?.AddHit(100);

        FindObjectOfType<NoteSpawner>()?.UnregisterNote(this);
        Destroy(gameObject);
    }

    public void Miss()
    {
        scoreManager?.AddMiss(-50);

        FindObjectOfType<NoteSpawner>()?.UnregisterNote(this);
        Destroy(gameObject);
    }
}








