using UnityEngine;

public class PlayerDontWalk : MonoBehaviour
{
    [Header("Anti-Climb Settings")]
    [Tooltip("Layers que se consideran obstáculos para no poder subirse encima")]
    public LayerMask obstacleLayers;

    [Tooltip("Fuerza hacia abajo para impedir subirse")]
    public float antiClimbForce = 120f;

    private Rigidbody rb;
    private float previousY;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        previousY = transform.position.y;
    }

    void FixedUpdate()
    {
        HandleAntiClimb();
        previousY = transform.position.y; // actualizar altura para next frame
    }

    void HandleAntiClimb()
    {
        // Detectar si hay obstáculos debajo / tocando pies
        Collider[] hits = Physics.OverlapBox(
            transform.position + Vector3.up * 0.1f,   // altura de los pies
            new Vector3(0.5f, 0.05f, 0.5f),          // tamaño del detector
            Quaternion.identity,
            obstacleLayers
        );

        if (hits.Length > 0)
        {
            // Mantener altura anterior
            Vector3 pos = transform.position;
            pos.y = previousY;
            transform.position = pos;

            // Cancelar velocidad vertical
            Vector3 vel = rb.linearVelocity;
            vel.y = 0;
            rb.linearVelocity = vel;

            // Empuje hacia abajo extra
            rb.AddForce(Vector3.down * antiClimbForce, ForceMode.Force);
        }
    }

}
