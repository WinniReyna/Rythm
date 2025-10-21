using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Lean.Localization;
using System;

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
    public TMP_Text continueText; // Texto que indica "Presiona Space para continuar"

    [Header("Audio")]
    public AudioSource audioSource;

    private DialogueData currentDialogue;
    private IInputProvider inputProvider;
    private int currentLineIndex = 0;

    private Action onDialogueEnd;

    private void Awake()
    {

        inputProvider = new KeyboardInputProvider();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        // Avanzar con tecla Space
        if (dialoguePanel.activeSelf && inputProvider.DialogueLine())
        {
            if (currentDialogue != null)
            {
                if (HasMoreLines())
                    Debug.Log("Hay más líneas de diálogo...");
                NextLine();
            }
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        npcNameText.text = dialogue.npcName;

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

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

        // Texto con Lean Localization
        if (!string.IsNullOrEmpty(line.localizedKey))
            dialogueText.text = LeanLocalization.GetTranslationText(line.localizedKey);
        else
            dialogueText.text = "";

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

        // Limpiar botones de respuesta
        foreach (Transform child in responseContainer)
            Destroy(child.gameObject);

        // Crear botones si hay respuestas
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

        // Mostrar aviso de "continuar" si hay más líneas y no hay respuestas
        if (continueText != null)
        {
            if (HasMoreLines() && (line.responses == null || line.responses.Length == 0))
                continueText.gameObject.SetActive(true);
            else
                continueText.gameObject.SetActive(false);
        }
    }

    private bool HasMoreLines()
    {
        return currentDialogue != null && currentLineIndex < currentDialogue.lines.Length - 1;
    }

    public void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue(); // Apaga panel, texto e icono
        }
        else
        {
            ShowLine();
        }
    }

    public void SetPostDialogueAction(Action action)
    {
        onDialogueEnd = action;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        if (npcIcon != null) npcIcon.enabled = false;
        if (continueText != null) continueText.gameObject.SetActive(false);

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;

        currentDialogue = null;

        onDialogueEnd?.Invoke();
        onDialogueEnd = null; // Limpiar para no repetir
    }
}



