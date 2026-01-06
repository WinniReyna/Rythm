using System.Collections;
using UnityEngine;
using TMPro;

public class HitFeedbackUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hitText;
    [SerializeField] private float showTime = 0.5f;

    private Coroutine currentRoutine;

    public void Show(string text, Color color)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(text, color));
    }

    private IEnumerator ShowRoutine(string text, Color color)
    {
        hitText.text = text;
        hitText.color = color;
        hitText.gameObject.SetActive(true);

        yield return new WaitForSeconds(showTime);

        hitText.gameObject.SetActive(false);
        currentRoutine = null;
    }
}
