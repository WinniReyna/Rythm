using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private RawImage characterImage;

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(DialogueData data)
    {
        dialoguePanel.SetActive(true);

        // Nombre
        nameText.text = string.IsNullOrEmpty(data.characterName)
            ? ""
            : Lean.Localization.LeanLocalization.GetTranslationText(data.characterName);

        // Texto principal
        dialogueText.text = Lean.Localization.LeanLocalization.GetTranslationText(data.textKey);

        // Imagen (RawImage usa Texture en lugar de Sprite)
        if (data.characterIcon != null)
        {
            characterImage.texture = data.characterIcon.texture;
            characterImage.gameObject.SetActive(true);
        }
        else
        {
            characterImage.gameObject.SetActive(false);
        }
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}

