using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : ItemContainer
{
    [FormerlySerializedAs("items")]
    [SerializeField] Item[] startingItems;
    [SerializeField] Transform itemsParent;

    // same events in Inventory, EquipmentPanel and ItemSlot - to facilitate later in Character class
    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    private void Start() // prev Awake
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            itemSlots[i].OnPointerExitEvent += OnPointerExitEvent;
            itemSlots[i].OnRightClickEvent += OnRightClickEvent;
            itemSlots[i].OnBeginDragEvent += OnBeginDragEvent;
            itemSlots[i].OnEndDragEvent += OnEndDragEvent;
            itemSlots[i].OnDragEvent += OnDragEvent;
            itemSlots[i].OnDropEvent += OnDropEvent;
            // add listener to when the ItemSlot scripts event is sent
        }

        SetStartingItems();
    }

    private void OnValidate()
    {
        if(itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();

        SetStartingItems();
    }

    private void SetStartingItems()
        // matches our itemSlots (UI-element) with our itemList (items)
    {
        int i = 0;
        for (; (i < startingItems.Length && i < itemSlots.Length); i++)
            //every item in the list is instantiated and gets assigned to an item-slot
        {
            itemSlots[i].Item = startingItems[i].GetCopy();
            itemSlots[i].Amount = 1; // if more than 1 of same is added, they don't stack and leads to an empty slot (no amount counter)
        }

        for (; (i < itemSlots.Length); i++)
            // for any item-slot that doesn't have an item to go into it, set to null
        {
            itemSlots[i].Item = null;
            itemSlots[i].Amount = 0;
        }
    }
}