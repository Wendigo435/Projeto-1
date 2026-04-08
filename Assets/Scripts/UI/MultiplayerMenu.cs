using UnityEngine;
using Mirror;
using TMPro;

public class MultiplayerMenu : MonoBehaviour
{
    [Header("Configurações")]
    public string defaultIP = "localhost";
    public TMP_InputField portInput;

    private NetworkManager manager;

    void Start()
    {
        manager = NetworkManager.singleton;

        if (portInput != null) portInput.text = "7777";
    }

    private void SetupNetworkSettings()
    {
        if (Transport.active is kcp2k.KcpTransport transport)
        {
            if (ushort.TryParse(portInput.text, out ushort port))
            {
                transport.Port = port;
            }
            else
            {
                Debug.LogWarning("Porta inválida! Usando padrão 7777.");
                transport.Port = 7777;
            }
        }

        manager.networkAddress = defaultIP;
    }

    public void HostGame()
    {
        SetupNetworkSettings();
        manager.StartHost();
        Debug.Log("Host iniciado na porta: " + portInput.text);
    }

    public void JoinGame()
    {
        SetupNetworkSettings();
        manager.StartClient();
        Debug.Log("Tentando conectar em " + manager.networkAddress + ":" + portInput.text);
    }
}