using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExamineUIManager : MonoBehaviour, IExamineUI
{
    [SerializeField] private GameObject panelUI;
    [SerializeField] private RawImage panelRawImage; 
    [SerializeField] private TMP_Text panelText;

    public void Show(Texture2D texture, string text)
    {
        if (panelRawImage != null) panelRawImage.texture = texture;
        if (panelText != null) panelText.text = text;
        if (panelUI != null) panelUI.SetActive(true);
    }

    public void Hide()
    {
        if (panelUI != null) panelUI.SetActive(false);
    }
}
