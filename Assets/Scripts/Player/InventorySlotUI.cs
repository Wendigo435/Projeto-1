using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;

    public void Setup(ItemData item, int amount)
    {
        icon.sprite = item.icon;
        icon.enabled = true;
        amountText.text = amount.ToString();
    }

    public void Clear()
    {
        icon.enabled = false;
        amountText.text = "";
    }
}