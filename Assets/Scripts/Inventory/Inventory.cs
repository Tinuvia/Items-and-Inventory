using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : ItemContainer
{
    [FormerlySerializedAs("items")]
    [SerializeField] ItemSO[] startingItems;
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
            itemSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
            itemSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
            itemSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
            itemSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
            itemSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
            itemSlots[i].OnDragEvent += slot => OnDragEvent(slot);
            itemSlots[i].OnDropEvent += slot => OnDropEvent(slot);
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
        Clear();
        for (int i = 0; i < startingItems.Length; i++)
        {
            AddItem(startingItems[i].GetCopy());
        }
    }
}