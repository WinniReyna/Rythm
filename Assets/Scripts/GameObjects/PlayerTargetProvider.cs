using UnityEngine;

public class PlayerTargetProvider : MonoBehaviour, ITargetProvider
{
    [SerializeField] private Transform playerTransform;

    public Vector3 GetTargetPosition()
    {
        return playerTransform.position;
    }
}
