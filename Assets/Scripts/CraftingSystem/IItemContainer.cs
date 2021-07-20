public interface IItemContainer
{
    // an interface allows decoupling of the inventory itself and any systems that use it

    int ItemCount(string itemID);
    Item RemoveItem(string itemID);
    bool RemoveItem(Item item);
    bool AddItem(Item item);
    bool IsFull();
}
