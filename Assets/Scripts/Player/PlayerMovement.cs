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
    private bool isFinishingClimb = false;

    // Puntos de escalada
    private Transform climbStartPoint;
    private Transform climbEndPoint;

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

        if (!isClimbing && !isFinishingClimb)
        {
            HandleJump();
            HandleMove(input, isRunning);
            CheckClimb(input);
        }
        else if (isClimbing && !isFinishingClimb)
        {
            HandleClimb(input);
            CheckExitClimb();
        }
    }

    #region Movimiento
    private void HandleMove(Vector2 input, bool isRunning)
    {
        if (isClimbing || isFinishingClimb) return;

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
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            isJumping = true;
            playerAnimator.SetTrigger("Jump");
        }

        if (rb.linearVelocity.y < -0.1f && !isGrounded)
            playerAnimator.SetFalling(true);
        else
            playerAnimator.SetFalling(false);

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
                var points = hit.collider.GetComponent<ClimbPoints>();
                if (points == null) return;

                climbStartPoint = points.startPoint;
                climbEndPoint = points.endPoint;

                rb.linearVelocity = Vector3.zero;
                transform.position = climbStartPoint.position;
                transform.rotation = climbStartPoint.rotation;

                isClimbing = true;
                rb.useGravity = false;
                playerAnimator.SetClimbing(true);
            }
        }
    }

    private void HandleClimb(Vector2 input)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExitClimb();
            return;
        }

        if (climbStartPoint == null || climbEndPoint == null) return;

        float verticalMove = input.y * climbSpeed * Time.deltaTime;
        transform.position += Vector3.up * verticalMove;
        transform.rotation = Quaternion.Slerp(transform.rotation, climbStartPoint.rotation, 10f * Time.deltaTime);

        playerAnimator.SetClimbSpeed(Mathf.Abs(input.y));
    }

    private void CheckExitClimb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ExitClimb();
    }

    private void ExitClimb()
    {
        isClimbing = false;
        rb.useGravity = true;
        playerAnimator.SetClimbing(false);
    }
    #endregion

    #region Climb Finish Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClimbFinish") && isClimbing && !isFinishingClimb)
        {
            StartFinishClimb();
        }
    }

    private void StartFinishClimb()
    {
        isFinishingClimb = true;
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;
        playerAnimator.SetClimbing(false);
        playerAnimator.SetTrigger("ClimbFinish");
    }

    // Método público para Animation Event
    public void OnFinishClimbAnimation()
    {
        if (climbEndPoint != null)
            transform.position = climbEndPoint.position;

        rb.useGravity = true;
        isClimbing = false;
        isFinishingClimb = false;
    }
    #endregion
}
