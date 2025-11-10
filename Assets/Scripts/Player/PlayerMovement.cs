using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidades")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runMultiplier = 2f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Chequeos")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private float turnSmoothVelocity;
    private Vector3 moveDirection;

    private IInputProvider inputProvider;
    private IPlayerAnimator playerAnimator;

    private bool isJumping = false;
    private bool isGrounded = false;

    public static PlayerMovement Instance { get; private set; }
    public bool canMove = true;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        inputProvider = new KeyboardInputProvider();
        playerAnimator = GetComponent<IPlayerAnimator>();
    }

    void Update()
    {
        if (!canMove) return;

        Vector2 input = inputProvider.GetMovement();
        bool isRunning = inputProvider.IsRunning();

        //CheckGrounded();

        HandleMove(input, isRunning);
        //HandleJump();
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
    /*private void HandleJump()
    {
        if (!canMove) return;

        if (isGrounded && inputProvider.JumpPressed())
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

        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
        playerAnimator.SetGrounded(isGrounded);

        if (isGrounded)
            isJumping = false;
    }*/
    #endregion  
}
