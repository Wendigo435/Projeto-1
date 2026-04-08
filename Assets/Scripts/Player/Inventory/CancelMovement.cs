using Mirror;
using UnityEngine;

public class CancelMovement : NetworkBehaviour
{
    private PlayerMovement movement;

    private void OnEnable()
    {
        if (!isLocalPlayer) return;
        movement = FindObjectOfType<PlayerMovement>();

        movement.CanMove = false;

    }

    private void OnDisable()
    {
        if (!isLocalPlayer) return;
        movement = FindObjectOfType<PlayerMovement>();

        movement.CanMove = true;
    }
}
