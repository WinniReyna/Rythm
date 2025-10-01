using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 2f;
    public float runMultiplier = 2f;
    public float turnSmoothTime = 0.1f;

    [Header("Salto")]
    public float jumpForce = 5f;           // Fuerza de salto
    public float fallMultiplier = 2.5f;    // Para ca�da m�s r�pida

    [Header("Escalada")]
    public float climbSpeed = 2f;
    public LayerMask climbableLayer;
    public float climbDetectionDistance = 1f;

    private Rigidbody rb;
    private float turnSmoothVelocity;
    private Vector3 moveDirection;

    private IInputProvider inputProvider;
    private IPlayerAnimator playerAnimator;

    private bool isClimbing = false;
    private bool isJumping = false;
    private bool isGrounded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Evita que el Rigidbody gire
        inputProvider = new KeyboardInputProvider();
        playerAnimator = GetComponent<IPlayerAnimator>();
    }

    void Update()
    {
        Vector2 input = inputProvider.GetMovement();
        bool isRunning = inputProvider.IsRunning();

        CheckGrounded();
        HandleJump();
        HandleMove(input, isRunning);

        if (!isClimbing)
            CheckClimb(input);
        else
            HandleClimb(input);
    }

    #region Movimiento
    private void HandleMove(Vector2 input, bool isRunning)
    {
        moveDirection = new Vector3(input.x, 0, input.y).normalized;
        float currentSpeed = walkSpeed * (isRunning ? runMultiplier : 1f);

        if (moveDirection.magnitude >= 0.1f)
        {
            RotateTowards(moveDirection);
            Vector3 horizontalVelocity = moveDirection * currentSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);

            playerAnimator.SetSpeed(moveDirection.magnitude);
            playerAnimator.SetRunning(isRunning);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            playerAnimator.SetSpeed(0f);
            playerAnimator.SetRunning(false);
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
    #endregion

    #region Salto
    private void HandleJump()
    {
        if (isGrounded && inputProvider.JumpPressed() && !isClimbing)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // reset vertical
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            isJumping = true;
            playerAnimator.SetTrigger("Jump");
        }

        // Detecta si está cayendo
        if (rb.linearVelocity.y < -0.1f && !isGrounded)
        {
            playerAnimator.SetFalling(true);
        }
        else
        {
            playerAnimator.SetFalling(false);
        }

        // Caída más rápida
        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);
        playerAnimator.SetGrounded(isGrounded);

        if (isGrounded)
            isJumping = false;
    }
    #endregion
    #region Escalada
    private void CheckClimb(Vector2 input)
    {
        if (isJumping) return; // No escalar mientras saltamos

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, climbDetectionDistance, climbableLayer))
        {
            if (input.y > 0)
            {
                isClimbing = true;
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
                playerAnimator.SetClimbing(true);
            }
        }
    }

    private void HandleClimb(Vector2 input)
    {
        Vector3 climbMove = Vector3.up * input.y * climbSpeed;
        rb.MovePosition(rb.position + climbMove * Time.deltaTime);

        playerAnimator.SetClimbSpeed(Mathf.Abs(input.y));

        if (!Physics.Raycast(transform.position, transform.forward, climbDetectionDistance, climbableLayer))
        {
            isClimbing = false;
            rb.useGravity = true;
            playerAnimator.SetClimbing(false);
        }
    }
    #endregion
}
