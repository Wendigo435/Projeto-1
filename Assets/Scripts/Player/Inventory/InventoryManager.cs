using UnityEngine;
using Mirror;

public class InventoryManager : NetworkBehaviour
{
    [SyncVar] public int dummySync; // força sincronização

    public ItemData[] items = new ItemData[16];
    public int[] amounts = new int[16];

    private InventoryUI ui;
    private Camera cam;
    public float pickDistance = 3f;

    void Start()
    {
        if (!isLocalPlayer) return;

        cam = Camera.main;
        ui = FindObjectOfType<InventoryUI>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickDistance))
        {
            if (hit.collider.CompareTag("Item"))
            {
                NetworkIdentity identity = hit.collider.GetComponent<NetworkIdentity>();
                if (identity != null)
                {
                    CmdPickup(identity);
                }
            }
        }
    }

    [Command]
    void CmdPickup(NetworkIdentity itemNet)
    {
        if (itemNet == null) return;

        WorldItem worldItem = itemNet.GetComponent<WorldItem>();
        if (worldItem == null) return;

        AddItem(worldItem.itemData);
        NetworkServer.Destroy(itemNet.gameObject);
    }

    void AddItem(ItemData item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                amounts[i] = 1;
                TargetUpdateSlot(connectionToClient, i, item.icon, 1);
                return;
            }

            if (items[i] == item)
            {
                amounts[i]++;
                TargetUpdateSlot(connectionToClient, i, item.icon, amounts[i]);
                return;
            }
        }
    }

    [TargetRpc]
    void TargetUpdateSlot(NetworkConnection target, int index, Sprite icon, int amount)
    {
        if (ui == null) ui = FindObjectOfType<InventoryUI>();

        ui.UpdateSlot(index, icon, amount);
    }

    // Permite adicionar item manualmente
    public void GiveItem(ItemData item, int amount)
    {
        if (!isServer) return;

        for (int i = 0; i < amount; i++)
        {
            AddItem(item);
        }
    }
}