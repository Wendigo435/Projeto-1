using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [Header("Arraste todos os seus ScriptableObjects aqui")]
    public List<ItemData> allItems;

    private void Awake()
    {
        // Singleton simples para facilitar o acesso de outros scripts
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Função que a UI vai usar para achar o ícone pelo ID
    public ItemData GetItemByID(string id)
    {
        return allItems.FirstOrDefault(item => item.id == id);
    }
}