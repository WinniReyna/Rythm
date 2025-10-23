using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }

    [Header("UI de carga")]
    [Tooltip("Arrastramos nuestro menú de panel.")]
    [SerializeField] private GameObject loadingScreen;
    [Tooltip("Arrastramos nuestro slider.")]
    [SerializeField] private Slider progressBar;
    [Tooltip("Arrastramos nuestro texto de tipo TMPro")]
    [SerializeField] private TMP_Text progressText;

    private Coroutine currentLoadingCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject.transform.root.gameObject); // Mantener todo el Canvas
        loadingScreen.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        if (currentLoadingCoroutine != null)
        {
            StopCoroutine(currentLoadingCoroutine);
            currentLoadingCoroutine = null;
        }

        currentLoadingCoroutine = StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);

        progressBar.value = 0f;
        progressText.text = "0%";

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            progressBar.value = progress;
            progressText.text = (progress * 100f).ToString("F0") + "%";

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f); // Pequeña pausa opcional
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingScreen.SetActive(false);
        currentLoadingCoroutine = null;
    }
}




