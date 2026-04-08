using UnityEngine;
using Mirror;
using UnityEngine.UI;
using NUnit.Framework.Internal.Execution;

[System.Serializable]
public struct InventoryItem
{
    public string id;
}

public class PlayerInventory : NetworkBehaviour
{
    [Header("Configurações de Inventário")]
    public readonly SyncList<InventoryItem> inventory = new SyncList<InventoryItem>();

    [Header("Referências de UI")]
    public GameObject inventoryPanel; // O painel que liga/desliga
    private bool isInventoryOpen = false;

    public override void OnStartLocalPlayer()
    {
        // 1. Acha o painel da UI pelo nome na cena
        inventoryPanel = GameObject.Find("InventoryBackground");

        // 2. Acha o script que gerencia a exibição e passa a referência do jogador para ele
        InventoryDisplay display = inventoryPanel.GetComponentInParent<InventoryDisplay>();
        if (display != null)
        {
            display.playerInventory = this;
            display.VincularCallback(); // Função que vamos criar abaixo
        }

        inventoryPanel.SetActive(false); // Começa fechado
    }

    void Update()
    {
        // Se não for o nosso jogador, não faz nada
        if (!isLocalPlayer) return;

        // Tecla para abrir/fechar (I ou Tab)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        // Só permite coletar itens se o inventário estiver FECHADO
        if (!isInventoryOpen && Input.GetKeyDown(KeyCode.E))
        {
            TentarColetar();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isInventoryOpen);

            // Controle do Mouse
            Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isInventoryOpen;
        }
    }

    void TentarColetar()
    {
        // Raio sai do centro da tela (onde está a mira)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.TryGetComponent<InWorldItem>(out InWorldItem item))
            {
                // Envia o ID do item e o objeto para o servidor
                CmdAddItem(item.itemData.id, item.gameObject);
            }
        }
    }

    [Command]
    void CmdAddItem(string itemId, GameObject itemObject)
    {
        if (itemObject == null) return;

        // Adiciona na lista sincronizada
        inventory.Add(new InventoryItem { id = itemId });

        // Deleta o item do mundo para todos os jogadores
        NetworkServer.Destroy(itemObject);
    }
}