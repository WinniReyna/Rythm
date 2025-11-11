using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Lean.Localization;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private RawImage npcIcon;
    [SerializeField] private Transform responseContainer;
    [SerializeField] private GameObject responseButtonPrefab;
    [SerializeField] private TMP_Text continueText; // "Presiona Space para continuar"

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

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
        if (dialoguePanel.activeSelf && inputProvider.DialogueLine())
        {
            if (currentDialogue == null)
                return;

            if (HasMoreLines())
            {
                NextLine();
            }
            else
            {

                EndDialogue();
            }
        }
    }


    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);

        npcNameText.text = dialogue.GetNpcName();

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

        // Texto según idioma actual
        dialogueText.text = line.GetText();

        // Icono del NPC (opcional)
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

        // Audio de voz (opcional)
        if (audioSource != null && line.npcVoice != null)
            audioSource.PlayOneShot(line.npcVoice);

        // Limpiar respuestas anteriores
        foreach (Transform child in responseContainer)
            Destroy(child.gameObject);

        // Crear nuevas respuestas si las hay
        if (line.responses != null && line.responses.Length > 0)
        {
            foreach (DialogueResponse response in line.responses)
            {
                GameObject btnObj = Instantiate(responseButtonPrefab, responseContainer);
                TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();
                btnText.text = response.GetText();

                btnObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (response.nextDialogue != null)
                        StartDialogue(response.nextDialogue);
                    else
                        NextLine();
                });
            }
        }

        // Mostrar "presiona continuar" si no hay respuestas
        if (continueText != null)
            continueText.gameObject.SetActive(HasMoreLines() && (line.responses == null || line.responses.Length == 0));
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
            EndDialogue();
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

        if (onDialogueEnd != null)
        {
            var actionToRun = onDialogueEnd;
            onDialogueEnd = null;
            actionToRun.Invoke();
        }
    }
}
