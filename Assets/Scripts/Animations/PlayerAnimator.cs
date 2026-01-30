using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
{
    private Animator animator;

    void Awake() => animator = GetComponent<Animator>();

    public void SetSpeed(float speed) => animator.SetFloat("Speed", speed);
    public void SetRunning(bool isRunning) => animator.SetBool("IsRunning", isRunning);
    public void SetTrigger(string triggerName) => animator.SetTrigger(triggerName);
    //public void SetFalling(bool isFalling) => animator.SetBool("IsFalling", isFalling);
    //public void SetGrounded(bool isGrounded) => animator.SetBool("IsGrounded", isGrounded);
    public Animator GetAnimator() => animator;
}

