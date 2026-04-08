using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Image[] slotImages;
    public TMP_Text[] slotAmounts;

    bool isOpen;

    void Start()
    {
        inventoryPanel.SetActive(false);
        ClearAll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Toggle();
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }

    public void UpdateSlot(int index, Sprite icon, int amount)
    {
        if (index < 0 || index >= slotImages.Length) return;

        slotImages[index].enabled = true;
        slotImages[index].sprite = icon;
        slotAmounts[index].text = amount > 1 ? amount.ToString() : "";
    }

    public void ClearAll()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].enabled = false;
            slotImages[i].sprite = null;
            slotAmounts[i].text = "";
        }
    }
}