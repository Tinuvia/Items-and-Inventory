using UnityEngine;

public class Inventory : ItemContainer
{
    [SerializeField] protected ItemSO[] startingItems;
    [SerializeField] protected Transform itemsParent;

    protected override void OnValidate()
    {
        // since we have our own method of getting the itemSlots (here from the parent) we won't be using base.OnValidate()
        if (itemsParent != null)
            itemsParent.GetComponentsInChildren(includeInactive: true, result: ItemSlots);

        SetStartingItems();
    }

    protected override void Awake() // overrides the method from the base class ItemContainer
    {
        base.Awake(); // assigns the listeners to events
        SetStartingItems();
    }
       
    private void SetStartingItems()
        // matches our itemSlots (UI-element) with our itemList (items)
    {
        Clear();
        foreach (ItemSO item in startingItems)
        {
            AddItem(item.GetCopy());
        }
    }
}