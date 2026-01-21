using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExamineUIManager : MonoBehaviour, IExamineUI, IMenuPanel
{
    [SerializeField] private GameObject panelUI;
    [SerializeField] private RawImage panelRawImage;
    [SerializeField] private TMP_Text panelText;

    private void Awake()
    {
        if (panelUI != null)
            panelUI.SetActive(false);
    }

    public void Show(Texture2D texture, string text)
    {
        if (panelRawImage != null) panelRawImage.texture = texture;
        if (panelText != null) panelText.text = text;

        PauseManager.Instance.OpenPanel(gameObject);
    }

    public void Hide()
    {
        if (panelUI != null)
            panelUI.SetActive(false);

        if (PauseManager.Instance != null && PauseManager.Instance.CurrentPanel == this)
            PauseManager.Instance.CloseCurrentPanel();
    }

    public void Open()
    {
        if (panelUI != null)
            panelUI.SetActive(true);
    }

    public void Close()
    {
        if (panelUI != null)
            panelUI.SetActive(false);
    }
}
