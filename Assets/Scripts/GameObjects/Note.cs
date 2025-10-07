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

    public void Initialize(NoteKey key, int x = -1, int y = -1, Sprite sprite = null)
    {
        requiredKey = key;
        gridX = x;
        gridY = y;
        paintSprite = sprite;

        spawner = FindObjectOfType<NoteSpawner>();
        gridPainter = FindObjectOfType<GridPainter>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Miss();
        }
    }

    public void Hit()
    {
        if (gridPainter != null && gridX >= 0 && gridY >= 0 && paintSprite != null)
        {
            gridPainter.PaintCellWithSprite(gridX, gridY, paintSprite);
            Debug.Log("coordenadas" + gridX + gridY + paintSprite);
        }
        spawner?.UnregisterNote(this);
        Destroy(gameObject);
    }

    public void Miss()
    {
        spawner?.UnregisterNote(this);
        Destroy(gameObject);
    }
}












