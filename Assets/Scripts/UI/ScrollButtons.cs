using UnityEngine;
using UnityEngine.UI;
public class ScrollButtons : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollStep = 0.2f;

    public void ScrollRight()
    {
        scrollRect.horizontalNormalizedPosition =
            Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + scrollStep);
    }

    public void ScrollLeft()
    {
        scrollRect.horizontalNormalizedPosition =
            Mathf.Clamp01(scrollRect.horizontalNormalizedPosition - scrollStep);
    }
}
