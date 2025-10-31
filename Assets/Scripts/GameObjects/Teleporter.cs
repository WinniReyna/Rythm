using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour, ICollisionAction, IPositionProvider
{
    [Header("Destino y Transición")]
    [SerializeField] private Vector2 destination; // punto al que se moverá el jugador
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeOutDuration = 1f; // duración del fade a negro
    [SerializeField] private float fadeInDuration = 1f;  // duración del fade desde negro

    [SerializeField] private RawImage fadeImage;

    public Vector2 GetTargetPosition() => destination;

    private void Awake()
    {
        if (fadeImage == null)
            fadeImage = GetComponent<RawImage>();

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f; // completamente transparente al inicio
            fadeImage.color = c;
        }
    }

    public void OnCollide(GameObject player)
    {
        StartCoroutine(TeleportRoutine(player));
    }

    private IEnumerator TeleportRoutine(GameObject player)
    {
        // 🔹 Bloquear movimiento antes de empezar el fade
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        // 🔹 Fade a negro
        fadeAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(fadeOutDuration);

        // Reinicia velocidad para que no se vea caminando
        Rigidbody rb2d = player.GetComponent<Rigidbody>();
        if (rb2d != null)
            rb2d.linearVelocity = Vector2.zero;

        // Reinicia Animator para que deje de mostrar caminar/correr
        PlayerAnimator playerAnim = player.GetComponent<PlayerAnimator>();
        if (playerAnim != null)
        {
            playerAnim.SetSpeed(0f);
            playerAnim.SetRunning(false);
        }

        // 🔹 Teletransportar al jugador
        player.transform.position = destination;

        // 🔹 Mantener la pantalla negra un momento opcional
        yield return new WaitForSeconds(0.1f);

        

        // 🔹 Volver a mostrar la escena
        fadeAnimator.SetTrigger("FadeOut");

        // ⏳ Esperar hasta que termine el fade in (la pantalla vuelve a verse)
        yield return new WaitForSeconds(fadeInDuration);

        // 🔹 Reactivar movimiento solo después de que termine el fade
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }
}
