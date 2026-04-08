using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("Slots configurados manualmente")]
    public Image[] slotImages;

    void Awake()
    {
        Instance = this;
    }
}