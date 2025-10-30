using UnityEngine;

public class Teleporter : MonoBehaviour, ICollisionAction, IPositionProvider
{
    [SerializeField] private Vector2 destination;

    public Vector2 GetTargetPosition() => destination;

    public void OnCollide(GameObject player)
    {
        // Bloquea movimiento del jugador
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        // Teletransporta
        player.transform.position = GetTargetPosition();

        // Desbloquea movimiento inmediatamente
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }
}

