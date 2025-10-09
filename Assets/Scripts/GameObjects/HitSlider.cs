using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class HitSlider : MonoBehaviour
{
    [Header("Configuración")]
    public float activeDuration = 2f; // cuánto tiempo permanece activo
    public float defaultValue = 0f;
    public float hitValue = 1f;

    private Slider slider;
    public bool isActive = false;

    [SerializeField] private NoteSpawner noteSpawner;
    void Awake()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
            Debug.LogError("HitSlider necesita un componente Slider.");

        slider.value = defaultValue;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Activa el slider y lo resetea
    /// </summary>
    public void Activate()
    {
        slider.value = defaultValue;
        gameObject.SetActive(true);
        isActive = true;
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        float timer = 0f;
        bool completed = false;

        while (timer < activeDuration)
        {
            timer += Time.deltaTime;

            if (slider.value >= hitValue)
            {
                completed = true;
                Debug.Log("Slider completado correctamente");
                noteSpawner?.OnSliderCompleted(true); // true = éxito
                break;
            }

            yield return null;
        }

        if (!completed)
        {
            Debug.Log("Slider fallado");
            noteSpawner?.OnSliderCompleted(false); // false = fallo
        }

        Deactivate();
    }

    private void Deactivate()
    {
        isActive = false;
        slider.gameObject.SetActive(false);
        slider.value = defaultValue;
    }
}
