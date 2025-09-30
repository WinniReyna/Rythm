using UnityEngine;

public class DontDestroyBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(transform.root.gameObject);
    }
}

