using UnityEngine;

public class AutoFadeIn : MonoBehaviour
{
    void Start()
    {
        if (MenuManager.instance != null)
            MenuManager.instance.StartCoroutine(MenuManager.instance.FadeIn());
    }
}