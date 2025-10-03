using UnityEngine;
using UnityEngine.UI;

public interface IExamineUI
{
    void Show(Texture2D texture, string text);
    void Hide();
}