using UnityEngine;
using System;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    private void Start() // prev Awake
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnPointerEnterEvent += (slot) => OnPointerEnterEvent(slot);
            equipmentSlots[i].OnPointerExitEvent += (slot) => OnPointerExitEvent(slot);
            equipmentSlots[i].OnRightClickEvent += (slot) => OnRightClickEvent(slot);
            equipmentSlots[i].OnBeginDragEvent += (slot) => OnBeginDragEvent(slot);
            equipmentSlots[i].OnEndDragEvent += (slot) => OnEndDragEvent(slot);
            equipmentSlots[i].OnDragEvent += (slot) => OnDragEvent(slot);
            equipmentSlots[i].OnDropEvent += (slot) => OnDropEvent(slot);
        }
    }

    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(EquippableItemSO item, out EquippableItemSO previousItem)
        // using out parameter, since we want both the bool and the item
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                previousItem = (EquippableItemSO)equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquippableItemSO item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }
}
