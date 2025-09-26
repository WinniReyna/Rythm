using UnityEngine;

public interface IInputProvider
{
    Vector2 GetMovement();   // Movimiento horizontal/vertical
    bool IsRunning();        // Sprint
}
