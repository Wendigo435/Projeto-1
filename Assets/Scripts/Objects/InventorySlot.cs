using System;
using Mirror;

[Serializable]
public struct InventorySlot
{
    public int ItemID;
    public int Amount;

    public InventorySlot(int id, int amount)
    {
        ItemID = id;
        Amount = amount;
    }
}
