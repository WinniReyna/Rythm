using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitSlider : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float activeDuration = 2f; // cuánto tiempo permanece activo
    [SerializeField] private float defaultValue = 0f;
    [SerializeField] private float hitValue = 1f;

    private Slider slider;
    [SerializeField] private bool isActive = false;

    [SerializeField] private NoteSpawner noteSpawner;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        slider = GetComponent<Slider>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        slider.value = defaultValue;
        SetSliderVisible(false);
    }


    /// <summary>
    /// Activa el slider y lo resetea
    /// </summary>
    public void Activate()
    {
        slider.value = defaultValue;
        StartCoroutine(ActivateNextFrame());
    }

    private IEnumerator ActivateNextFrame()
    {
        // Espera un frame para asegurar que el Canvas/UI se haya renderizado
        yield return null;

        SetSliderVisible(true);
        isActive = true;
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        float timer = 0f;

        while (timer < activeDuration)
        {
            timer += Time.deltaTime;

            if (slider.value >= hitValue)
            {
                Debug.Log("Hit correcto en el slider!");

                if (noteSpawner != null)
                    noteSpawner.OnSliderCompleted(true); // avisar éxito

                yield return new WaitForSeconds(0.3f);
                Deactivate();
                yield break;
            }

            yield return null;
        }

        // Si el tiempo se acaba sin lograr hit
        Debug.Log("Tiempo agotado en el slider, no se logró hit.");
        if (noteSpawner != null)
            noteSpawner.OnSliderCompleted(false);

        Deactivate();
    }

    private void Deactivate()
    {
        isActive = false;
        slider.value = defaultValue;
        SetSliderVisible(false);

        var hitZones = FindObjectsOfType<HitZone>();
        foreach (var zone in hitZones)
        {
            zone.ResetZoneColor();
        }
    }

    /// <summary>
    /// Control seguro de visibilidad del slider
    /// </summary>
    private void SetSliderVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}
