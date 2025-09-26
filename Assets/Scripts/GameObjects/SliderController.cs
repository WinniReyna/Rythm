using UnityEngine;

public class SliderController : MonoBehaviour
{
    [Range(0, 1)]
    public int CurrentValue = 0;

    public void SetValue(int value)
    {
        CurrentValue = Mathf.Clamp(value, 0, 1);
    }
}
