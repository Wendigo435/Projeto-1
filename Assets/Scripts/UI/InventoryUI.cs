using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class InventoryDisplay : MonoBehaviour
{
    public PlayerInventory playerInventory; // Arraste o jogador aqui (ou busque via código)
    public GameObject slotPrefab;           // O prefab do quadradinho
    public Transform container;             // Onde os slots vão ficar (o Grid Layout)
    public ItemDatabase database;           // Seu banco de dados de ScriptableObjects

    void Start()
    {
        // Se o inventário já tiver itens ao começar, desenha eles
        RefreshUI();

        // Se inscreve no callback para atualizar sempre que ganhar/perder item
        playerInventory.inventory.Callback += OnInventoryChanged;
    }

    public void VincularCallback()
    {
        if (playerInventory != null)
        {
            // Se inscreve para atualizar a UI sempre que a lista do player mudar
            playerInventory.inventory.Callback += OnInventoryChanged;

            // Desenha o que já estiver no inventário (caso o player já tenha itens)
            RefreshUI();
        }
    }

    void OnInventoryChanged(SyncList<InventoryItem>.Operation op, int index, InventoryItem oldItem, InventoryItem newItem)
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // 1. Limpa a UI antiga
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // 2. Cria os novos slots baseados na SyncList
        foreach (InventoryItem item in playerInventory.inventory)
        {
            // Busca os dados visuais no banco de dados usando o ID
            ItemData data = database.GetItemByID(item.id);

            if (data != null)
            {
                GameObject newSlot = Instantiate(slotPrefab, container);
                // Procura a imagem do ícone (geralmente o segundo componente Image)
                newSlot.transform.GetChild(0).GetComponent<Image>().sprite = data.icone;
            }
        }
    }
}
