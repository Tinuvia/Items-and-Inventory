using UnityEngine;
using System;

public class EquipmentPanel : MonoBehaviour
{
    public EquipmentSlot[] EquipmentSlots;
    [SerializeField] Transform equipmentSlotsParent;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    private void Start() // prev Awake
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            EquipmentSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent?.Invoke(slot);
            EquipmentSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent?.Invoke(slot);
            EquipmentSlots[i].OnRightClickEvent += slot => OnRightClickEvent?.Invoke(slot);
            EquipmentSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent?.Invoke(slot);
            EquipmentSlots[i].OnEndDragEvent += slot => OnEndDragEvent?.Invoke(slot);
            EquipmentSlots[i].OnDragEvent += slot => OnDragEvent?.Invoke(slot);
            EquipmentSlots[i].OnDropEvent += slot => OnDropEvent?.Invoke(slot);
        }
    }

    private void OnValidate()
    {
        EquipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(EquippableItemSO item, out EquippableItemSO previousItem)
        // using out parameter, since we want both the bool and the item
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            if (EquipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                previousItem = (EquippableItemSO)EquipmentSlots[i].Item;
                EquipmentSlots[i].Item = item;
                EquipmentSlots[i].Amount = 1;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquippableItemSO item)
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            if (EquipmentSlots[i].Item == item)
            {
                EquipmentSlots[i].Item = null;
                EquipmentSlots[i].Amount = 0;
                return true;
            }
        }
        return false;
    }
}
