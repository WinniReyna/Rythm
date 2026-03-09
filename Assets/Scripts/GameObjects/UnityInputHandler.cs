using UnityEngine;

public class UnityInputHandler : IInputHandler
{
    public bool IsKeyPressed(NoteKey key)
    {
        switch (key)
        {
            case NoteKey.A: 
                return Input.GetKeyDown(KeyCode.A) ||
                       Input.GetKeyDown(KeyCode.JoystickButton1);

            case NoteKey.S: 
                return Input.GetKeyDown(KeyCode.S) ||
                       Input.GetKeyDown(KeyCode.JoystickButton0);

            case NoteKey.D: 
                return Input.GetKeyDown(KeyCode.D) ||
                       Input.GetKeyDown(KeyCode.JoystickButton2);

            case NoteKey.Shift: 
                return Input.GetKeyDown(KeyCode.LeftShift) ||
                       Input.GetKeyDown(KeyCode.JoystickButton3);

            case NoteKey.Space: 
                return Input.GetKeyDown(KeyCode.Space) ||
                       Input.GetAxis("LT") > 0.5f ||
                       Input.GetAxis("RT") > 0.5f;

            default:
                return false;
        }
    }
}
