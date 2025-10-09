using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    private const string DifficultyKey = "SelectedDifficulty";

    [SerializeField] private DifficultySettings defaultDifficulty;
    private DifficultySettings currentDifficulty;

    public DifficultySettings CurrentDifficulty => currentDifficulty;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadDifficulty();
    }

    public void SetDifficulty(DifficultySettings newDifficulty)
    {
        currentDifficulty = newDifficulty;
        PlayerPrefs.SetString(DifficultyKey, newDifficulty.name);
        PlayerPrefs.Save();

        Debug.Log("Dificultad guardada: " + newDifficulty.name);
    }

    private void LoadDifficulty()
    {
        string savedName = PlayerPrefs.GetString(DifficultyKey, "");
        if (!string.IsNullOrEmpty(savedName))
        {
            // Busca en todos los assets del proyecto
            DifficultySettings[] allDifficulties = Resources.LoadAll<DifficultySettings>("");
            foreach (var diff in allDifficulties)
            {
                if (diff.name == savedName)
                {
                    currentDifficulty = diff;
                    Debug.Log("Dificultad cargada: " + diff.name);
                    return;
                }
            }
        }

        // Si no había una guardada, usar la predeterminada
        currentDifficulty = defaultDifficulty;
        Debug.Log("Dificultad por defecto usada: " + defaultDifficulty.name);
    }
}
