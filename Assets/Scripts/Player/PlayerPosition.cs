using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Teleport")
        {
            var action = other.gameObject.GetComponent<ICollisionAction>();
            action?.OnCollide(gameObject);
        }
    }
}
