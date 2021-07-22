public interface IItemContainer
{
    // an interface allows decoupling of the inventory itself and any systems that use it

    int ItemCount(string itemID);
    ItemSO RemoveItem(string itemID);
    bool RemoveItem(ItemSO item);
    bool AddItem(ItemSO item);
    bool IsFull();
    void Clear();
}
