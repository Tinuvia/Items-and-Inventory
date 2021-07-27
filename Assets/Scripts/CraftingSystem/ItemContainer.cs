using System;
using System.Collections.Generic;
using UnityEngine;

// base class for the Inventory and ItemContainer classes

public abstract class ItemContainer : MonoBehaviour, IItemContainer
    // abstract class --> we can't create instances of it, can't attach to gameObjects,
    // it implements the IItemContainer interface
    // all methods are virtual, so that they can be overwritten in subclasses if needed
{
    public List<ItemSlot> ItemSlots;
  
    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;


    protected virtual void OnValidate()
    {
        GetComponentsInChildren(includeInactive: true, result: ItemSlots);
    }

    protected virtual void Awake()
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            ItemSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent?.Invoke(slot);
            ItemSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent?.Invoke(slot);
            ItemSlots[i].OnRightClickEvent += slot => OnRightClickEvent?.Invoke(slot);
            ItemSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent?.Invoke(slot);
            ItemSlots[i].OnEndDragEvent += slot => OnEndDragEvent?.Invoke(slot);
            ItemSlots[i].OnDragEvent += slot => OnDragEvent?.Invoke(slot);
            ItemSlots[i].OnDropEvent += slot => OnDropEvent?.Invoke(slot);
            // add listener to when the ItemSlot scripts event is sent
        }
    }

    public virtual bool CanAddItem(ItemSO item, int amount = 1)
    {
        int freeSpaces = 0;

        foreach (ItemSlot itemSlot in ItemSlots)
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
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (ItemSlots[i].CanAddStack(item))
            {
                ItemSlots[i].Item = item;
                ItemSlots[i].Amount++;
                return true;
            }
        }

        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (ItemSlots[i].Item == null)
            {
                ItemSlots[i].Item = item;
                ItemSlots[i].Amount++;
                return true;
            }
        }
        return false;
    }
    
    public virtual bool RemoveItem(ItemSO item)
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (ItemSlots[i].Item == item)
            {
                ItemSlots[i].Amount--;
                return true;
            }
        }
        return false;
    }
    
    public virtual ItemSO RemoveItem(string itemID)
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            ItemSO item = ItemSlots[i].Item;
            // if there's an item in the slot, compare with our ID
            if (item != null && item.ID == itemID)
            {
                ItemSlots[i].Amount--;
                return item;
            }
        }
        return null;
    }

    public virtual int ItemCount(string itemID)
    {
        int number = 0;

        for (int i = 0; i < ItemSlots.Count; i++)
        {
            ItemSO item = ItemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                // #15 need to not only increase by 1, but take stacking into account
                number += ItemSlots[i].Amount;
            }
        }
        return number;
    }

    public virtual void Clear()
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            ItemSlots[i].Item = null;
            ItemSlots[i].Amount = 0;
        }
    }
}
