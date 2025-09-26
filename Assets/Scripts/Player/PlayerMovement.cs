using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 2f;
    public float runMultiplier = 2f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private float turnSmoothVelocity;
    private Vector3 velocity;

    // Dependencias inyectables
    private IInputProvider inputProvider;
    private IPlayerAnimator playerAnimator;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputProvider = new KeyboardInputProvider(); // Se puede cambiar fácilmente
        playerAnimator = GetComponent<IPlayerAnimator>(); // Animator separado
    }

    void Update()
    {
        Vector2 input = inputProvider.GetMovement();
        bool isRunning = inputProvider.IsRunning();

        Move(input, isRunning);
        ApplyGravity();
    }

    private void Move(Vector2 input, bool isRunning)
    {
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float currentSpeed = walkSpeed * (isRunning ? runMultiplier : 1f);

        if (direction.magnitude >= 0.1f)
        {
            RotateTowards(direction);
            Vector3 moveDir = Quaternion.Euler(0f, GetTargetAngle(direction), 0f) * Vector3.forward;
            controller.Move(moveDir * currentSpeed * Time.deltaTime);
            playerAnimator.SetSpeed(currentSpeed / walkSpeed);
        }
        else
        {
            playerAnimator.SetSpeed(0f);
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        float targetAngle = GetTargetAngle(direction);
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private float GetTargetAngle(Vector3 direction)
    {
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
