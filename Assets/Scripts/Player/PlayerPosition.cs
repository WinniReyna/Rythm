using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Teleport")
        {
            var action = collision.gameObject.GetComponent<ICollisionAction>();
            action?.OnCollide(gameObject);
        }
    }
}
