using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Lean.Localization; // Asegúrate de tener este using

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text npcNameText;
    public TMP_Text dialogueText;
    public RawImage npcIcon;
    public Transform responseContainer;
    public GameObject responseButtonPrefab;

    [Header("Audio")]
    public AudioSource audioSource;

    private DialogueData currentDialogue;
    private int currentLineIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        npcNameText.text = dialogue.npcName;
        ShowLine();
    }

    private void ShowLine()
    {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.lines[currentLineIndex];

        // Texto usando Lean Localization
        if (!string.IsNullOrEmpty(line.localizedKey))
        {
            dialogueText.text = LeanLocalization.GetTranslationText(line.localizedKey);
        }
        else
        {
            dialogueText.text = "";
        }

        // Icono opcional
        if (npcIcon != null)
        {
            if (line.icon != null)
            {
                npcIcon.texture = line.icon;
                npcIcon.enabled = true;
            }
            else
            {
                npcIcon.enabled = false;
            }
        }

        // Audio opcional
        if (audioSource != null && line.npcVoice != null)
            audioSource.PlayOneShot(line.npcVoice);

        // Limpiar respuestas anteriores
        foreach (Transform child in responseContainer)
            Destroy(child.gameObject);

        // Mostrar respuestas si existen
        if (line.responses != null && line.responses.Length > 0)
        {
            foreach (DialogueResponse response in line.responses)
            {
                GameObject btnObj = Instantiate(responseButtonPrefab, responseContainer);
                TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();
                if (!string.IsNullOrEmpty(response.localizedKey))
                    btnText.text = LeanLocalization.GetTranslationText(response.localizedKey);

                btnObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (response.nextDialogue != null)
                        StartDialogue(response.nextDialogue);
                    else
                        NextLine();
                });
            }
        }
        else
        {
            // Avanzar con clic si no hay respuestas
            Button panelBtn = dialoguePanel.GetComponent<Button>();
            if (panelBtn != null)
            {
                panelBtn.onClick.RemoveAllListeners();
                panelBtn.onClick.AddListener(() => NextLine());
            }
        }
    }

    public void NextLine()
    {
        currentLineIndex++;
        ShowLine();
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentDialogue = null;
    }
}


