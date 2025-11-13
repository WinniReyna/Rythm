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

    private bool waitingForRewardClick = false; 
    private DialogueData currentDialogue;
    private IInputProvider inputProvider;
    private int currentLineIndex = 0;
    private Action onDialogueEnd;

    private bool rewardShown = false;

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
        if (!dialoguePanel.activeSelf) return;

        if (waitingForRewardClick) return; 

        if (inputProvider.DialogueLine())
        {
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
        rewardShown = false; 
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

        if (npcIcon != null) npcIcon.enabled = line.icon != null ? true : false;
        if (line.icon != null) npcIcon.texture = line.icon;
        if (audioSource != null && line.npcVoice != null) audioSource.PlayOneShot(line.npcVoice);

        foreach (Transform child in responseContainer)
            Destroy(child.gameObject);

        if (currentDialogue is ChoiceRewardDialogueData choiceDialogue &&
            currentLineIndex == currentDialogue.lines.Length - 1 &&
            !rewardShown)
        {
            string npcID = choiceDialogue.name;

            if (!choiceDialogue.onlyOnce || !QuestManager.Instance.HasNpcEventCompleted(npcID))
            {
                rewardShown = true;
                waitingForRewardClick = true; 
                ShowRewardChoices(choiceDialogue);
                return;
            }
            else if (choiceDialogue.afterChoiceDialogue != null)
            {
                StartDialogue(choiceDialogue.afterChoiceDialogue);
                return;
            }
        }

        // --- Respuestas normales ---
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

        if (continueText != null)
            continueText.gameObject.SetActive(responseContainer.childCount == 0 && HasMoreLines());
    }


    private void ShowRewardChoices(ChoiceRewardDialogueData dialogue)
    {
        foreach (Transform child in responseContainer)
            Destroy(child.gameObject);

        foreach (var item in dialogue.rewardOptions)
        {
            GameObject btnObj = Instantiate(responseButtonPrefab, responseContainer);
            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();
            btnText.text = item.itemName;

            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!waitingForRewardClick) return;
                waitingForRewardClick = false; 

                InventoryManager.Instance.AddItem(item.itemID);
                QuestManager.Instance.MarkNpcEventCompleted(dialogue.name);

                foreach (Transform c in responseContainer)
                    Destroy(c.gameObject);

                // Si hay diálogo posterior, lo mostramos
                if (dialogue.afterChoiceDialogue != null)
                {
                    StartDialogue(dialogue.afterChoiceDialogue);
                }
                else
                {
                    // Si no hay diálogo posterior, simplemente avanzamos al final del current
                    EndDialogue();
                }
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
