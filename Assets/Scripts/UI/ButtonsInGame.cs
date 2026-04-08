using UnityEngine;
using UnityEngine.UI;

public class ButtonHotkey : MonoBehaviour
{
    public KeyCode ActiveKey;

    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetKeyDown(ActiveKey) && button.interactable)
        {
            button.onClick.Invoke();
        }
    }
}
