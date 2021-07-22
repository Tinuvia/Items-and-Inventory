public interface IItemContainer
{
    // an interface allows decoupling of the inventory itself and any systems that use it

    int ItemCount(string itemID);
    ItemSO RemoveItem(string itemID);
    bool RemoveItem(ItemSO item);
    bool CanAddItem(ItemSO item, int amount = 1); 
    bool AddItem(ItemSO item);
    void Clear();
}
