public interface IItemContainer
{
    // an interface allows decoupling of the inventory itself and any systems that use it

    int ItemCount(Item item);
    bool ContainsItem(Item item);
    bool RemoveItem(Item item);
    bool AddItem(Item item);
    bool IsFull();
}
