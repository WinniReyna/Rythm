using UnityEngine;

public abstract class MenuPanelBase : MonoBehaviour, IMenuPanel
{
    public virtual void Open()
    {
        gameObject.SetActive(true);

        // Bloquea movimiento del jugador
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);

        // Reactiva movimiento del jugador
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }
}
