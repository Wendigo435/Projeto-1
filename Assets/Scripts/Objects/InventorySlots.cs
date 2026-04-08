using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Obrigatório para o Drag and Drop

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variaveis
    public Image icon; //Icone do slot
    public TMP_Text amountText; //Texto da quantidade
    public Button removeButton;  //Botão de remover item

    [HideInInspector] public int myIndex; // O RefreshUI vai preencher isso
    private InventoryUI uiParent; //Script "InventoryUI"
    private CanvasGroup canvasGroup; //Grupo do canvas
    private Vector3 originalIconPosition; //Posição original do slot
    #endregion

    private void Awake()
    {
        uiParent = GetComponentInParent<InventoryUI>();

        // Adicione o componente Canvas Group no objeto do Slot no Unity!
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    #region Atualiza Slot
    public void UpdateSlot(string itemName, int amount, Sprite itemSprite)
    {
        icon.sprite = itemSprite;
        icon.enabled = (itemSprite != null);
        amountText.text = amount > 1 ? amount.ToString() : "";
    }
    #endregion

    #region Limpa Slot
    public void ClearSlot()
    {
        icon.sprite = null;
        icon.enabled = false;
        amountText.text = "";
    }
    #endregion

    #region Arrastar "item"
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (icon.sprite == null || !icon.enabled) return;
        originalIconPosition = icon.transform.localPosition; // salva local
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }
    #endregion

    public void ResetIconPosition()
    {
        icon.transform.localPosition = Vector3.zero;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    #region Em "Puxe" do mouse
    public void OnDrag(PointerEventData eventData)
    {
        if (icon.sprite == null || !icon.enabled) return;
        icon.transform.position = eventData.position;
    }
    #endregion

    #region Mouse "solta" item
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        icon.transform.localPosition = originalIconPosition; // restaura local
        if (icon.sprite == null || !icon.enabled) return;
        GameObject hit = eventData.pointerCurrentRaycast.gameObject;
        if (hit != null)
        {
            if (hit.TryGetComponent(out InventorySlot targetSlot))
            {
                uiParent.RequestSwap(this.myIndex, targetSlot.myIndex);
            }
        }
        else
        {
            uiParent.UIButton_RemoveItem(this.myIndex);
        }
    }
    #endregion
}