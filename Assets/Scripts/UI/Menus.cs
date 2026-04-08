using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Mirror.Discovery;
using System.Collections.Generic;

// Mudamos para NetworkBehaviour para usar [SyncVar]
public class MenuERedeManager : NetworkBehaviour
{
    public static MenuERedeManager instancia;

    [Header("Scripts de Apoio")]
    public NetworkManager networkManager;
    public NetworkDiscovery networkDiscovery;

    [Header("Paineis de Menu")]
    public List<GameObject> menus = new List<GameObject>();

    [Header("Configuração de Sala")]
    public TMP_InputField inputNomeSala;

    // SyncVar faz com que o nome da sala apareça para quem entra DEPOIS
    [SyncVar(hook = nameof(OnNomeSalaChanged))]
    public string nomeDaSalaSincronizado;

    [Header("Lobby")]
    public TextMeshProUGUI textoTituloLobby;
    public Transform listaPlayersContent;
    public GameObject playerEntryPrefab;

    [Header("Lista de Salas")]
    public Transform listaSalasContent;
    public GameObject salaBotaoPrefab;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        if (isClient || isServer) return; // Evita resetar se já estiver em rede
        AbrirMenu(0);
    }

    public void AbrirMenu(int index)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].SetActive(i == index);
        }
    }

    // --- LÓGICA DO HOST ---
    public void BotaoConfirmarCriarSala()
    {
        string nome = string.IsNullOrEmpty(inputNomeSala.text) ? "Minha Sala" : inputNomeSala.text;

        // Inicia o Host
        networkManager.StartHost();

        // No servidor, definimos a SyncVar
        nomeDaSalaSincronizado = nome;

        networkDiscovery.AdvertiseServer();
        AbrirMenu(3);
    }

    // Hook: Chamado automaticamente quando o nome da sala chega no cliente
    void OnNomeSalaChanged(string oldNome, string newNome)
    {
        textoTituloLobby.text = "SALA: " + newNome;
    }

    // --- LÓGICA DOS PLAYERS ---
    // Chamado pelo sistema de Mirror quando um player entra
    public void AdicionarPlayerNaUI(string nomeDoPlayer)
    {
        GameObject novaEntrada = Instantiate(playerEntryPrefab, listaPlayersContent);
        novaEntrada.GetComponentInChildren<TextMeshProUGUI>().text = nomeDoPlayer;
    }

    // --- LÓGICA DO CLIENTE (LISTA DE SALAS) ---
    public void BotaoRefreshSalas()
    {
        foreach (Transform child in listaSalasContent) Destroy(child.gameObject);
        networkDiscovery.StartDiscovery();
    }

    public void OnDiscoveredServer(ServerResponse response)
    {
        GameObject novoBotao = Instantiate(salaBotaoPrefab, listaSalasContent);
        // Aqui você pode mudar para exibir o IP ou nome
        novoBotao.GetComponentInChildren<TextMeshProUGUI>().text = "Sala: " + response.EndPoint.Address;

        novoBotao.GetComponent<Button>().onClick.AddListener(() => {
            networkDiscovery.StopDiscovery();
            networkManager.StartClient(response.uri);
            AbrirMenu(3);
        });
    }
}