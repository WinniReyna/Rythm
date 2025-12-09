using UnityEngine;

public interface IMessageDisplay
{
    void ShowMessage(string text, float duration = 0f);
    void HideMessage();
}

