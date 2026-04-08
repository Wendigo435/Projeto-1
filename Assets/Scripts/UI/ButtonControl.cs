using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}