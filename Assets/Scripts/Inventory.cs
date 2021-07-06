using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;

    private void OnValidate()
    {
        if(itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        }

        RefreshUI();
    }

    private void RefreshUI()
        // matches our itemSlots (UI-element) with our itemList (items)
    {
        int i = 0;
        for (; (i < items.Count && i < itemSlots.Length); i++)
            //every item gets assigned to an item-slot
        {
            itemSlots[i].Item = items[i];
        }

        for (; (i < itemSlots.Length); i++)
            // for any item-slot that doesn't have an item to go into it, set to null
        {
            itemSlots[i].Item = null;
        }
    }
}
