using Mirror;
using UnityEngine;

public class Collect : NetworkBehaviour
{
    public float distancia = 3f;

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, distancia))
            {
                if (hit.collider.TryGetComponent(out CollectItem coletavel))
                {
                    CmdColetarItem(hit.collider.gameObject);
                }
            }
        }
    }

    [Command]
    void CmdColetarItem(GameObject itemObj)
    {
        if (itemObj == null) return;

        CollectItem info = itemObj.GetComponent<CollectItem>();

        GetComponent<InventoryManager>().itensNomes.Add(info.dadosDoItem.name);
        NetworkServer.Destroy(itemObj);
    }
}