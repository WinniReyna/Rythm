using UnityEngine;

[CreateAssetMenu(fileName = "IsometricCameraStrategy", menuName = "Camera/IsometricStrategy")]
public class IsometricCameraStrategy : ScriptableObject, ICameraFollowStrategy
{
    public Vector3 offset = new Vector3(0f, 10f, -10f);

    public Vector3 GetDesiredPosition(Transform camera, Vector3 targetPosition)
    {
        return targetPosition + offset;
    }
}
