using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class InventoryManager : NetworkBehaviour
{
    public int maxSlots = 20;

    public readonly SyncList<InventoryItem> items = new SyncList<InventoryItem>();

    private void Start()
    {
        if (isServer)
        {
            for (int i = 0; i < maxSlots; i++)
            {
                items.Add(new InventoryItem(-1, 0));
            }
        }
    }

    #region ADD ITEM

    [Command]
    public void CmdAddItem(int itemID, int amount)
    {
        AddItem(itemID, amount);
    }

    void AddItem(int itemID, int amount)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemID == itemID)
            {
                var temp = items[i];
                temp.amount += amount;
                items[i] = temp;
                return;
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemID == -1)
            {
                items[i] = new InventoryItem(itemID, amount);
                return;
            }
        }
    }

    #endregion

    #region REMOVE ITEM

    [Command]
    public void CmdRemoveItem(int itemID, int amount)
    {
        RemoveItem(itemID, amount);
    }

    void RemoveItem(int itemID, int amount)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemID == itemID)
            {
                var temp = items[i];
                temp.amount -= amount;

                if (temp.amount <= 0)
                    items[i] = new InventoryItem(-1, 0);
                else
                    items[i] = temp;

                return;
            }
        }
    }

    #endregion
}