using System;
using Mirror;

[Serializable]
public struct InventoryItem
{
    public int itemID;
    public int amount;

    public InventoryItem(int id, int amt)
    {
        itemID = id;
        amount = amt;
    }
}