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

        while (timer < activeDuration)
        {
            timer += Time.deltaTime;

            // Detectar si el jugador logró el hit
            if (slider.value >= hitValue)
            {
                Debug.Log("Hit correcto en el slider!");
                isActive = false;
                gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }

        // Tiempo agotado
        isActive = false;
        gameObject.SetActive(false);
        slider.value = defaultValue; // reset
        Debug.Log("Tiempo agotado en el slider, no se logró hit.");
    }
}
