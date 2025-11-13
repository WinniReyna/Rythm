using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public System.Action OnDialogueEnded; 

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private RawImage npcIcon;
    [SerializeField] private Transform responseContainer;
    [SerializeField] private GameObject responseButtonPrefab;
    [SerializeField] private TMP_Text continueText;

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
            if (currentDialogue == null) return;

            // si es un diálogo de recompensa, no avanzamos con espacio si hay botones
            if (currentDialogue is ChoiceRewardDialogueData choiceDialogue)
            {
                // si estamos en la última línea, esperamos que el jugador elija
                if (!HasMoreLines())
                    return;
            }

            if (HasMoreLines())
                NextLine();
            else
                EndDialogue();
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
        dialogueText.text = line.GetText();

        // Icono
        if (npcIcon != null)
        {
            if (line.icon != null)
            {
                npcIcon.texture = line.icon;
                npcIcon.enabled = true;
            }
            else npcIcon.enabled = false;
        }

        // Audio
        if (audioSource != null && line.npcVoice != null)
            audioSource.PlayOneShot(line.npcVoice);

        // Limpiar respuestas anteriores
        foreach (Transform child in responseContainer)
            Destroy(child.gameObject);

        // Si este diálogo tiene recompensas y es la última línea
        if (currentDialogue is ChoiceRewardDialogueData choiceDialogue &&
            currentLineIndex == currentDialogue.lines.Length - 1)
        {
            string npcID = choiceDialogue.name;

            if (!choiceDialogue.onlyOnce || !QuestManager.Instance.HasNpcEventCompleted(npcID))
            {
                ShowRewardChoices(choiceDialogue);
                return;
            }
            else if (choiceDialogue.afterChoiceDialogue != null)
            {
                StartDialogue(choiceDialogue.afterChoiceDialogue);
                return;
            }
        }

        // Respuestas normales
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

        // Mostrar "presiona continuar"
        if (continueText != null)
            continueText.gameObject.SetActive(HasMoreLines() && (line.responses == null || line.responses.Length == 0));
    }

    private void ShowRewardChoices(ChoiceRewardDialogueData dialogue)
    {
        dialogueText.text = "Elige un objeto:";

        foreach (Transform child in responseContainer)
            Destroy(child.gameObject);

        foreach (var item in dialogue.rewardOptions)
        {
            GameObject btnObj = Instantiate(responseButtonPrefab, responseContainer);
            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();
            btnText.text = item.itemName;

            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                InventoryManager.Instance.AddItem(item.itemID);
                QuestManager.Instance.MarkNpcEventCompleted(dialogue.name);

                // Limpia botones
                foreach (Transform c in responseContainer)
                    Destroy(c.gameObject);

                // Si hay diálogo post-elección
                if (dialogue.afterChoiceDialogue != null)
                    StartDialogue(dialogue.afterChoiceDialogue);
                else
                    EndDialogue();
            });
        }

        if (continueText != null)
            continueText.gameObject.SetActive(false);
    }

    private bool HasMoreLines() =>
        currentDialogue != null && currentLineIndex < currentDialogue.lines.Length - 1;

    public void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= currentDialogue.lines.Length)
            EndDialogue();
        else
            ShowLine();
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
        onDialogueEnd = null;

    }
}


