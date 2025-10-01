using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJumpTest : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float fallMultiplier = 2.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private IInputProvider inputProvider;
    private IPlayerAnimator playerAnimator;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputProvider = new KeyboardInputProvider();
        playerAnimator = GetComponent<IPlayerAnimator>();
    }

    void Update()
    {
        HandleMove();
        HandleJump();
        ApplyGravity();
    }

    private void HandleMove()
    {
        Vector2 input = inputProvider.GetMovement();
        Vector3 move = new Vector3(input.x, 0, input.y);
        controller.Move(move * walkSpeed * Time.deltaTime);

        playerAnimator.SetSpeed(move.magnitude);
    }

    private void HandleJump()
    {
        if (controller.isGrounded && inputProvider.JumpPressed())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            playerAnimator.SetTrigger("Jump");
        }
    }

    private void ApplyGravity()
    {
        if (velocity.y < 0)
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;

        if (!controller.isGrounded)
            velocity.y += gravity * Time.deltaTime;

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        controller.Move(velocity * Time.deltaTime);
    }
}

