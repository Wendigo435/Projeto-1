using Mirror;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{
    #region Variaveis
    [Header("Configurações")]
    public float PickDistance = 3f; //Distancia para pegar Item
    public string itemTag = "Item"; //Tag que todos itens precisam
    public KeyCode InvKey = KeyCode.Tab; //Tecla para abrir/fechar inventario
    public KeyCode PickKey = KeyCode.E; //Tecla de pegar item

    [Header("Componentes")]
    public readonly SyncList<Item> inventory = new SyncList<Item>(); //Lista do inventario
    private InventoryUI UI; //Script da UI
    private Camera Cam; //Camera FPS
    public ItemDatabase database;

    #endregion

    #region Iniciador
    public override void OnStartLocalPlayer()
    {
        GameObject uiObj = GameObject.FindWithTag("Inventory");
        if (uiObj != null) UI = uiObj.GetComponent<InventoryUI>();

        if (UI != null) UI.BindPlayer(this);

        inventory.Callback += OnInventoryUpdated;
        Cam = Camera.main;
    }
    #endregion

    #region Update
    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(InvKey))
        {
            if (UI != null)
            {
                UI.ToggleUI();
            }
            else
            {
                UI = FindFirstObjectByType<InventoryUI>();
                if (UI != null) UI.ToggleUI();
            }
        }

        if (Input.GetKeyDown(PickKey))
        {
            TryPickup();
        }
    }
    #endregion

    #region Ao Atualizar Inventario
    void OnInventoryUpdated(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem)
    {
        if (isLocalPlayer && UI != null) UI.RefreshUI(inventory);
    }
    #endregion

    #region Tenta Pegar itens
    void TryPickup()
    {
        if (Cam == null) return;

        //Define de onde vem o ray
        Ray ray = Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, PickDistance))
        {
            //Ve se o ray tocou no obj com tag
            if (hit.collider.CompareTag(itemTag))
            {
                if (hit.collider.TryGetComponent(out PickupItem pickup))
                {
                    //Chama função de pegar item
                    CmdPickupItem(pickup.gameObject);
                }
            }
        }
    }
    #endregion

    #region Pega o Item
    [Command]
    public void CmdPickupItem(GameObject itemObject)
    {
        if (itemObject == null) return;
        if (!itemObject.TryGetComponent(out PickupItem pickup)) return;

        Item newItem = pickup.GetNetworkItem();

        if (database == null) { Debug.LogError("Esqueceu de arrastar a Database!"); return; }

        ItemData dataSO = database.GetItemByID(newItem.itemID);

        bool found = false;
        if (dataSO != null && dataSO.stackable)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemID == newItem.itemID)
                {
                    Item temp = inventory[i];
                    temp.amount += newItem.amount;
                    inventory[i] = temp;
                    found = true;
                    break;
                }
            }
        }

        if (!found) inventory.Add(newItem);

        //Destroi item coletado
        NetworkServer.Destroy(itemObject);
    }
    #endregion

    #region Troca de Itens
    [Command]
    public void CmdSwapItems(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= inventory.Count || indexB < 0 || indexB >= inventory.Count) return;

        Item temp = inventory[indexA];
        inventory[indexA] = inventory[indexB];
        inventory[indexB] = temp;
    }
    #endregion

    #region Remover Items
    [Command]
    public void CmdRemoveItem(int index)
    {
        if (index < 0 || index >= inventory.Count) return;

        Item networkItem = inventory[index];

        //Acha a UI
        InventoryUI ui = Object.FindAnyObjectByType<InventoryUI>();
        if (ui == null || ui.database == null) return;

        ItemData dataSO = ui.database.GetItemByID(networkItem.itemID);

        if (dataSO != null && dataSO.worldPrefab != null)
        {
            //Calcula a posição para spawnar
            Vector3 spawnPos = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;

            //Qual objeto deve ser instanciado
            GameObject dropped = Instantiate(dataSO.worldPrefab, spawnPos, Quaternion.identity);

            if (dropped.TryGetComponent(out PickupItem pickup))
            {
                //Define que o obj do drop tem a mesma quantidade do propio drop
                pickup.amount = networkItem.amount;
            }

            //Instancia o prefab do item
            NetworkServer.Spawn(dropped);
        }

        //Remove o item do inventario
        inventory.RemoveAt(index);
    }
    #endregion
}