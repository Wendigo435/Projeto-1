using UnityEngine;
using Mirror;

public class InWorldItem : NetworkBehaviour
{
    public ItemData itemData;

    [Server]
    public void Coletar()
    {
        NetworkServer.Destroy(gameObject);
    }
}