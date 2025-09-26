using UnityEngine;

public class KeyboardInputProvider : IInputProvider
{
    public Vector2 GetMovement()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
