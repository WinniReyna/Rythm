using UnityEngine;
using TMPro;
using System.Collections;

public class MessageUI : MonoBehaviour, IMessageDisplay, IMenuPanel
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text messageText;

    private Coroutine hideRoutine;

    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void Open()
    {
        if (panel != null)
            panel.SetActive(true);
    }

    public void Close()
    {
        HideMessage();
    }

    public void ShowMessage(string text, float duration = 2f)
    {
        if (panel == null || messageText == null)
            return;

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        PauseManager.Instance.OpenPanel(gameObject);

        messageText.text = text;

        if (duration > 0f)
            hideRoutine = StartCoroutine(HideAfterTime(duration));
    }

    public void HideMessage()
    {
        if (panel != null)
            panel.SetActive(false);

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
    }

    private IEnumerator HideAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (PauseManager.Instance != null &&
            PauseManager.Instance.CurrentPanel == this)
        {
            PauseManager.Instance.CloseCurrentPanel();
        }
        else
        {
            HideMessage();
        }
    }
}
