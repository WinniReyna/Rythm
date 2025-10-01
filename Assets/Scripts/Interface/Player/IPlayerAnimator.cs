public interface IPlayerAnimator
{
    void SetSpeed(float speed);
    void SetRunning(bool isRunning);
    void SetTrigger(string triggerName); // Jump
    void SetFalling(bool isFalling);     // Nuevo
    void SetGrounded(bool isGrounded);   // Nuevo
    void SetClimbing(bool isClimbing);
    void SetClimbSpeed(float speed);
}
