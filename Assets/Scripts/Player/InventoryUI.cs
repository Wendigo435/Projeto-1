using UnityEngine;
using Mirror;

public class InventoryUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject inventoryPanel;
    public InventorySlotUI[] slots;

    [Header("Config")]
    public KeyCode toggleKey = KeyCode.I;

    private InventoryManager playerInventory;
    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
        LockCursor(true);
    }

    void Update()
    {
        // Procurar player local
        if (playerInventory == null)
        {
            if (NetworkClient.localPlayer != null)
            {
                playerInventory = NetworkClient.localPlayer.GetComponent<InventoryManager>();

                if (playerInventory != null)
                {
                    playerInventory.items.Callback += OnInventoryChanged;
                    UpdateUI();
                }
            }
        }

        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
        LockCursor(!isOpen);
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    void OnInventoryChanged(SyncList<InventoryItem>.Operation op, int index, InventoryItem oldItem, InventoryItem newItem)
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (playerInventory == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i >= playerInventory.items.Count)
            {
                slots[i].Clear();
                continue;
            }

            var item = playerInventory.items[i];

            if (item.itemID == -1)
            {
                slots[i].Clear();
            }
            else
            {
                var data = ItemDatabase.instance.GetItem(item.itemID);
                slots[i].Setup(data, item.amount);
            }
        }
    }
}