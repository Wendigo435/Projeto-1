[System.Serializable]
public struct Item
{
    public int itemID; //ID do item
    public int amount; //Quantidade

    public Item(int id, int qty)
    {
        itemID = id;
        amount = qty;
    }
}