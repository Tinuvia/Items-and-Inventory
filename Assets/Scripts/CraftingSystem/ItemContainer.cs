using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IItemContainer
    // abstract class --> we can't create instances of it, can't attach to gameObjects,
    // it implements the IItemContainer interface
    // all methods are virtual, so that they can be overwritten in subclasses if needed
{
    [SerializeField] protected ItemSlot[] itemSlots;

    public virtual bool AddItem(ItemSO item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null || (itemSlots[i].Item.ID == item.ID && itemSlots[i].Amount < item.MaximumStacks))
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
                if (itemSlots[i].Amount == 0)
                {
                    itemSlots[i].Item = null;
                }
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
                if (itemSlots[i].Amount == 0)
                {
                    itemSlots[i].Item = null;
                }
                return item;
            }
        }
        return null;
    }

    public virtual bool IsFull()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null)
            {
                return false;
            }
        }
        return true;
    }

    public virtual int ItemCount(string itemID)
    {
        int number = 0;

        for (int i = 0; i < itemSlots.Length; i++)
        {
            ItemSO item = itemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                number++;
            }
        }
        return number;
    }
}
