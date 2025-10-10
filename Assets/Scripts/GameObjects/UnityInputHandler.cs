using UnityEngine;

public class UnityInputHandler : IInputHandler
{
    public bool IsKeyPressed(NoteKey key)
    {
        switch (key)
        {
            case NoteKey.A: return Input.GetKeyDown(KeyCode.A);
            case NoteKey.S: return Input.GetKeyDown(KeyCode.S);
            case NoteKey.D: return Input.GetKeyDown(KeyCode.D);
            case NoteKey.Space: return Input.GetKeyDown(KeyCode.Space);
            case NoteKey.Shift: return Input.GetKeyDown(KeyCode.LeftShift);
            default: return false;
        }
    }
}

