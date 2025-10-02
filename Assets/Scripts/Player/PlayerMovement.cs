using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 2f;
    public float runMultiplier = 2f;
    public float turnSmoothTime = 0.1f;

    [Header("Salto")]
    public float jumpForce = 5f;
    public float fallMultiplier = 2.5f;

    [Header("Chequeos")]
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundLayer;

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
        rb.constraints = RigidbodyConstraints.FreezeRotation;
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

        if (isClimbing)
            CheckExitClimb(); 

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

        if (moveDirection.magnitude >= 0.1f && !isClimbing)
        {
            RotateTowards(moveDirection);
            Vector3 horizontalVelocity = moveDirection * currentSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);

            playerAnimator.SetSpeed(moveDirection.magnitude);
            playerAnimator.SetRunning(isRunning);
        }
        else if (!isClimbing)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            playerAnimator.SetSpeed(0f);
            playerAnimator.SetRunning(false);
        }
    }
    private void CheckExitClimb()
    {
        // Si el jugador presiona la barra espaciadora, salir de climbing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isClimbing = false;
            rb.useGravity = true;
            playerAnimator.SetClimbing(false);
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
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            isJumping = true;
            playerAnimator.SetTrigger("Jump");
        }

        if (rb.linearVelocity.y < -0.1f && !isGrounded)
        {
            playerAnimator.SetFalling(true);
        }
        else
        {
            playerAnimator.SetFalling(false);
        }

        // caída más rápida
        if (rb.linearVelocity.y < 0 && !isClimbing)
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
        playerAnimator.SetGrounded(isGrounded);

        if (isGrounded)
            isJumping = false;
    }
    #endregion

    #region Escalada
    private void CheckClimb(Vector2 input)
    {
        if (isJumping) return;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, climbDetectionDistance, climbableLayer))
        {
            if (input.y > 0)
            {
                isClimbing = true;
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
                transform.position = hit.point - transform.forward * 0.5f; // anclaje al muro
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
