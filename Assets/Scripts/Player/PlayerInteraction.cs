using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask interactableMask;

    private Camera cam;
    private IInputProvider inputProvider;

    void Start()
    {
        cam = Camera.main;
        inputProvider = new KeyboardInputProvider();
    }

    void Update()
    {
        if (inputProvider.InteractPressed())
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}


