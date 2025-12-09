using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour, ICollisionAction, IPositionProvider, IInteractable
{
    [Header("Destino y Transición")]
    [SerializeField] private Vector3 destination;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private RawImage fadeImage;

    [Header("Objetos a gestionar")]
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject[] allObjects;

    [Header("Opcional: Llave requerida")]
    [Tooltip("Si está vacío, la puerta no requiere llave.")]
    [SerializeField] private string requiredKeyID;
    [SerializeField] private bool isLocked = false;

    [Header("Estado único para guardado")]
    [SerializeField] private string doorID;

    [Header("Audio")]
    [SerializeField] private AudioClip doorSound; 
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Messages UI")]
    [SerializeField] private string lockedMessageSP= "La puerta está cerrada.";
    [SerializeField] private string lockedMessageEN = "The door is closed.";
    [SerializeField] private MonoBehaviour messageDisplayComponent;
    private IMessageDisplay messageDisplay;
    public Vector3 GetTargetPosition() => destination;
    public bool IsLocked => isLocked;
    public string DoorID => doorID;
    public string RequiredKeyID => requiredKeyID;

    private void Awake()
    {
        if (fadeImage == null) fadeImage = GetComponent<RawImage>();
        if (!string.IsNullOrEmpty(requiredKeyID)) isLocked = true;
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        messageDisplay = messageDisplayComponent as IMessageDisplay;

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }        
    }

    public void Interact()
    {
        OnCollide(GameObject.FindWithTag("Player"));
    }

    public void OnCollide(GameObject player)
    {
        string lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();

        if (isLocked)
        {
            Debug.Log("Puerta cerrada (requiere llave)");

            audioSource.PlayOneShot(closeSound);

            if (lang == "Spanish") messageDisplay?.ShowMessage(lockedMessageSP, 2f);
            if (lang == "English") messageDisplay?.ShowMessage(lockedMessageEN, 2f);

            return;
        }

        if (doorSound != null && audioSource != null)
            audioSource.PlayOneShot(doorSound);

        StartCoroutine(TeleportRoutine(player));
    }

    public void Unlock()
    {
        if (isLocked)
        {
            isLocked = false;
            Debug.Log($"Puerta desbloqueada ({requiredKeyID})");
        }
    }

    private IEnumerator TeleportRoutine(GameObject player)
    {
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        fadeAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(fadeOutDuration);

        Rigidbody rb2d = player.GetComponent<Rigidbody>();
        if (rb2d != null)
            rb2d.linearVelocity = Vector2.zero;

        PlayerAnimator playerAnim = player.GetComponent<PlayerAnimator>();
        if (playerAnim != null)
        {
            playerAnim.SetSpeed(0f);
            playerAnim.SetRunning(false);
        }

        player.transform.position = destination;

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        if (allObjects != null)
        {
            foreach (var obj in allObjects)
            {
                if (obj != null && obj != objectToActivate)
                    obj.SetActive(false);
            }
        }

        yield return new WaitForSeconds(1.5f);

        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeInDuration);

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }
    // Asigna el estado desde el cargador JSON
    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }

    // Devuelve el estado para que el JsonSaveManager pueda leerlo
    public bool GetLockedState()
    {
        return isLocked;
    }
}
