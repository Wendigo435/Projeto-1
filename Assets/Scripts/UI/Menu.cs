using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Mirror.Discovery;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Configurações Mirror")]
    public NetworkManager networkManager;
    public NetworkDiscovery networkDiscovery;

    [Header("Paineis de Menu")]
    public List<GameObject> menus = new List<GameObject>();

    [Header("Animação (Fade)")]
    public CanvasGroup faderCanvasGroup;
    public float fadeSpeed = 2.0f;

    [Header("Lista de Salas")]
    public Transform listaSalasContent;
    public GameObject salaBotaoPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        faderCanvasGroup.alpha = 1;
        StartCoroutine(FadeIn());
        OpenMenu(0);
    }

    public void SwitchMenuAnim(int indexAlvo)
    {
        StartCoroutine(SequenceFadeMenu(indexAlvo));
    }

    public void StartHostAnim()
    {
        StartCoroutine(SequenceFadeHost());
    }


    private IEnumerator SequenceFadeMenu(int index)
    {
        yield return StartCoroutine(FadeOut());
        OpenMenu(index);
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator SequenceFadeHost()
    {
        yield return StartCoroutine(FadeOut());
        networkManager.StartHost();
        networkDiscovery.AdvertiseServer();
    }

    private void OpenMenu(int index)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].SetActive(i == index);
        }
    }

    public void SearchRooms()
    {
        foreach (Transform child in listaSalasContent) Destroy(child.gameObject);
        networkDiscovery.StartDiscovery();
    }

    public void OnDiscoveredServer(ServerResponse response)
    {
        GameObject btn = Instantiate(salaBotaoPrefab, listaSalasContent);
        btn.GetComponentInChildren<TextMeshProUGUI>().text = "Rooms";
        btn.GetComponent<Button>().onClick.AddListener(() => {
            networkDiscovery.StopDiscovery();
            networkManager.StartClient(response.uri);
        });
    }

    public IEnumerator FadeOut()
    {
        faderCanvasGroup.blocksRaycasts = false;
        while (faderCanvasGroup.alpha < 1)
        {
            faderCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        while (faderCanvasGroup.alpha > 0)
        {
            faderCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        faderCanvasGroup.blocksRaycasts = true;
    }

    public void FinalizeQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}