using UnityEngine;
public interface IInputProvider
{
    Vector2 GetMovement();   
    bool IsRunning();        
    bool JumpPressed();
    bool InteractPressed();
    bool InventoryPanel();
    bool PausePressed();
}
