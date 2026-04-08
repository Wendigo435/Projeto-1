using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomListManager : MonoBehaviour
{
    [Header("Referências de UI")]
    public Transform contentParent;
    public GameObject roomEntryPrefab;
    public MenuManager menuManager;

    public void RefreshRoomList()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= 3; i++)
        {
            GameObject novaEntrada = Instantiate(roomEntryPrefab, contentParent);
            novaEntrada.GetComponentInChildren<Text>().text = "Sala do Player " + i;

            int roomIndex = i;
            novaEntrada.GetComponent<Button>().onClick.AddListener(() => EntrarNaSala(roomIndex));
        }
    }

    public void HostGame()
    {
        Debug.Log("Criando nova sala...");
    }

    public void EntrarNaSala(int id)
    {
        Debug.Log("Entrando na sala: " + id);
    }

    public void VoltarMenuPrincipal()
    {
        menuManager.AbrirMenu(0);
    }
}
