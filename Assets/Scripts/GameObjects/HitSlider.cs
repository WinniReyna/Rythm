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

        while (timer < activeDuration)
        {
            timer += Time.deltaTime;

            if (slider.value >= hitValue)
            {
                Debug.Log("Hit correcto en el slider!");

                // Avisar al NoteSpawner
                if (noteSpawner != null)
                    noteSpawner.OnSliderCompleted();

                yield return new WaitForSeconds(0.3f); // opcional
                Deactivate();
                yield break;
            }

            yield return null;
        }

        Debug.Log("Tiempo agotado en el slider, no se logró hit.");
        Deactivate();
    }

    private void Deactivate()
    {
        isActive = false;
        slider.gameObject.SetActive(false);
        slider.value = defaultValue;
    }
}
