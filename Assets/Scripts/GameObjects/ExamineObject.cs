using UnityEngine;
using Lean.Localization;
using System.Collections;

public class ExamineObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ExamineData data;
    private IExamineUI uiManager;
    private bool isExamining = false;
    private IInputProvider inputProvider;

    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<ExamineUIManager>();
            inputProvider = new KeyboardInputProvider();
            if (uiManager == null)
            {
                //Debug.LogError("ExamineUIManager not found in the scene!");
            }
        }
    }
    private void Update()
    {
        if (isExamining && inputProvider.DialogueLine())
            EndExamine();
        
    }

    public void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);

        if (uiManager == null || data == null) return;

        string localizedTitle = data.GetTitle();
        string localizedDescription = data.GetDescription();

        uiManager.Show(data.objectTexture, localizedTitle + "\n\n" + localizedDescription);

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        isExamining = true;
    }

    public void EndExamine()
    {
        if (uiManager != null)
            uiManager.Hide();

        StartCoroutine(ReenableMovement());

        isExamining = false;
    }

    private IEnumerator ReenableMovement()
    {

        yield return new WaitForSeconds(0.1f); 

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }
}


