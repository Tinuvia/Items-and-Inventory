using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IItemContainer
    // abstract class --> we can't create instances of it, can't attach to gameObjects,
    // it implements the IItemContainer interface
    // all methods are virtual, so that they can be overwritten in subclasses if needed
{
    [SerializeField] protected ItemSlot[] itemSlots;

    public virtual bool CanAddItem(ItemSO item, int amount = 1)
    {
        int freeSpaces = 0;

        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.Item == null || itemSlot.Item.ID == item.ID)
            {
                freeSpaces += item.MaximumStacks - itemSlot.Amount;
            }
        }

        return freeSpaces >= amount;
    }

    public virtual bool AddItem(ItemSO item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].CanAddStack(item))
            {
                itemSlots[i].Item = item;
                itemSlots[i].Amount++;
                return true;
            }
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null)
            {
                itemSlots[i].Item = item;
                itemSlots[i].Amount++;
                return true;
            }
        }
        return false;
    }
    
    public virtual bool RemoveItem(ItemSO item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                itemSlots[i].Amount--;
                return true;
            }
        }
        return false;
    }
    
    public virtual ItemSO RemoveItem(string itemID)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            ItemSO item = itemSlots[i].Item;
            // if there's an item in the slot, compare with our ID
            if (item != null && item.ID == itemID)
            {
                itemSlots[i].Amount--;
                return item;
            }
        }
        return null;
    }

    public virtual int ItemCount(string itemID)
    {
        int number = 0;

        for (int i = 0; i < itemSlots.Length; i++)
        {
            ItemSO item = itemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                // #15 need to not only increase by 1, but take stacking into account
                number += itemSlots[i].Amount;
            }
        }
        return number;
    }

    public virtual void Clear()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
    }
}
