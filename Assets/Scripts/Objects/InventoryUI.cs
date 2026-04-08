using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    #region Variaveis
    public GameObject inventoryPanel; //Painel do inventario
    public static bool isOpen = false;

    [Header("Slots")]
    public List<InventorySlot> uiSlots = new List<InventorySlot>();

    private PlayerInventory localPlayerInventory; //Script de inventario

    public ItemDatabase database; //Script de base de itens


    public void BindPlayer(PlayerInventory player)
    {
        localPlayerInventory = player;
        RefreshUI(player.inventory);
    }
    #endregion

    #region Abre/Fecha UI
    public void ToggleUI()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;

        // Ao fechar, reseta todos os ícones
        if (!isOpen)
        {
            foreach (var slot in uiSlots)
            {
                if (slot != null)
                    slot.ResetIconPosition();
            }
        }
    }
    #endregion

    #region Trocar itens de lugar
    public void RequestSwap(int fromIndex, int toIndex)
    {
        if (localPlayerInventory != null)
        {
            // Envia o comando para o PlayerInventory que está no servidor
            localPlayerInventory.CmdSwapItems(fromIndex, toIndex);
        }
    }
    #endregion

    #region Atualizar UI
    public void RefreshUI(SyncList<Item> inventory)
    {
        if (uiSlots == null || uiSlots.Count == 0)
        {
            Debug.LogWarning("A lista de UI Slots está vazia no Inspector!");
            return;
        }

        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (uiSlots[i] == null) continue;

            uiSlots[i].myIndex = i; // <--- ADICIONE ISSO: Garante que o slot sabe sua posição
            uiSlots[i].ClearSlot();

            if (uiSlots[i].removeButton != null)
            {
                int index = i;
                uiSlots[i].removeButton.onClick.RemoveAllListeners();
                uiSlots[i].removeButton.onClick.AddListener(() => UIButton_RemoveItem(index));
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            if (i < uiSlots.Count && uiSlots[i] != null)
            {
                ItemData dataSO = database.GetItemByID(inventory[i].itemID);

                if (dataSO != null)
                {
                    uiSlots[i].UpdateSlot(dataSO.itemName, inventory[i].amount, dataSO.icon);
                }
            }
        }
    }
    #endregion

    #region Botão de Retirar item
    public void UIButton_RemoveItem(int slotIndex)
    {
        if (localPlayerInventory != null)
        {
            localPlayerInventory.CmdRemoveItem(slotIndex);
        }
    }
    #endregion
}