using UnityEngine;

public class TestNote : MonoBehaviour
{
    public float speed = 5f;
    public float xLimit = 20f;
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > xLimit)
            Destroy(gameObject);
    }
}
