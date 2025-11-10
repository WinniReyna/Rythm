using UnityEngine;

public class KeyboardInputProvider : IInputProvider
{
    public Vector2 GetMovement() => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    public bool IsRunning() => Input.GetKey(KeyCode.LeftShift);
    //public bool JumpPressed() => Input.GetKeyDown(KeyCode.Space);
    public bool InteractPressed() => Input.GetKeyDown(KeyCode.E);
    public bool InventoryPanel() => Input.GetKeyDown(KeyCode.I);
    public bool PausePressed() => Input.GetKeyDown(KeyCode.Escape);
    public bool DialogueLine() => Input.GetKeyDown(KeyCode.Space);
}
