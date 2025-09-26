using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private MonoBehaviour targetProviderMono; // Asignar PlayerTargetProvider
    [SerializeField] private IsometricCameraStrategy strategy;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;
    private ITargetProvider targetProvider;
    private ICameraFollowStrategy followStrategy;

    void Awake()
    {
        targetProvider = targetProviderMono as ITargetProvider;
        followStrategy = strategy as ICameraFollowStrategy;

        if (targetProvider == null || followStrategy == null)
            Debug.LogError("CameraFollow requiere TargetProvider y Strategy válidos");
    }

    void LateUpdate()
    {
        if (targetProvider == null) return;

        Vector3 desiredPos = followStrategy.GetDesiredPosition(transform, targetProvider.GetTargetPosition());
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothTime);
    }
}
