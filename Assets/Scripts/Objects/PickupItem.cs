using UnityEngine;
using Mirror;

public class PickupItem : NetworkBehaviour
{
    public ItemData data;
    [SyncVar] public int amount = 1; //Quantidade que o obj carrega

    public Item GetNetworkItem()
    {
        if (data == null)
        {
            Debug.LogError($"O objeto {gameObject.name} não tem um ScriptableObject (ItemData) atribuído!");
            return new Item(0, 0);
        }
        return new Item(data.itemID, amount);
    }
}