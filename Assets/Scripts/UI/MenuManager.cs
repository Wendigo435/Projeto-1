using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Configurações de Menu")]
    public GameObject[] menus;
    public CanvasGroup faderGroup;
    public float fadeDuration = 0.5f;

    private int currentMenuIndex = 0;

    void Start()
    {
        faderGroup.alpha = 0;
        faderGroup.blocksRaycasts = false;
        ShowMenu(0);
    }

    public void ChangeMenu(int newIndex)
    {
        StartCoroutine(FadeRoutine(newIndex));
    }

    private IEnumerator FadeRoutine(int targetIndex)
    {
        yield return StartCoroutine(Fade(1));

        ShowMenu(targetIndex);

        yield return StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        faderGroup.blocksRaycasts = true;
        float startAlpha = faderGroup.alpha;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            faderGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            yield return null;
        }

        faderGroup.alpha = targetAlpha;
        if (targetAlpha == 0) faderGroup.blocksRaycasts = false;
    }

    private void ShowMenu(int index)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(i == index);
        }
        currentMenuIndex = index;
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}