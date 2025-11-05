using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour, ICollisionAction, IPositionProvider
{
    [Header("Destino y Transición")]
    [SerializeField] private Vector3 destination;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private RawImage fadeImage;

    [Header("Objetos a gestionar")]
    [SerializeField] private GameObject objectToActivate; // el objeto que queremos prender
    [SerializeField] private GameObject[] allObjects;     // todos los objetos posibles para esta puerta

    public Vector3 GetTargetPosition() => destination;

    private void Awake()
    {
        // Inicializar fade
        if (fadeImage == null)
            fadeImage = GetComponent<RawImage>();

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    public void OnCollide(GameObject player)
    {
        StartCoroutine(TeleportRoutine(player));
    }

    private IEnumerator TeleportRoutine(GameObject player)
    {
        // Bloquear movimiento
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        // Fade a negro
        fadeAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(fadeOutDuration);

        // Reiniciar velocidad y Animator
        Rigidbody rb2d = player.GetComponent<Rigidbody>();
        if (rb2d != null)
            rb2d.linearVelocity = Vector2.zero;

        PlayerAnimator playerAnim = player.GetComponent<PlayerAnimator>();
        if (playerAnim != null)
        {
            playerAnim.SetSpeed(0f);
            playerAnim.SetRunning(false);
        }

        // Teletransportar
        player.transform.position = destination;

        // Activar el objeto deseado
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        // Apagar todos los demás
        if (allObjects != null)
        {
            foreach (var obj in allObjects)
            {
                if (obj != null && obj != objectToActivate)
                    obj.SetActive(false);
            }
        }

        // Pequeña espera para mantener pantalla negra
        yield return new WaitForSeconds(1.5f);

        // Fade out
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeInDuration);

        // Reactivar movimiento
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }
}
