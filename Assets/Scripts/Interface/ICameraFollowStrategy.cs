using UnityEngine;

public interface ICameraFollowStrategy
{
    Vector3 GetDesiredPosition(Transform camera, Vector3 targetPosition);
}
