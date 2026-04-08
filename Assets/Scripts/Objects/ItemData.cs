using UnityEngine;

[CreateAssetMenu(fileName = "Novo Item", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject
{
    public int itemID; //ID do item
    public string itemName; //Nome do item
    public Sprite icon; //Icone do item
    public GameObject worldPrefab; // O prefab que o servidor vai spawnar no chão
    public bool stackable = true; //É estacavel?
}