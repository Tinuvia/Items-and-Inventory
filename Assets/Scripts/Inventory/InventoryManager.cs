using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;

    private void Awake()
    {
        inventory.OnItemRightClickedEvent += EquipFromInventory;
        equipmentPanel.OnItemRightClickedEvent += UnEquipFromEquipPanel;
    }

    private void EquipFromInventory(Item item)
        // check first since not all items can be equipped
    {
        if (item is EquippableItem)
        {
            Equip((EquippableItem)item);
        }
    }

    private void UnEquipFromEquipPanel(Item item)
    {
        if (item is EquippableItem)
        {
            UnEquip((EquippableItem)item);
        }
    }

    public void Equip(EquippableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                    // if this slot is occupied
                {
                    inventory.AddItem(previousItem);
                }
            }
            else // if we can't equip it, return to inventory
            {
                inventory.AddItem(item);
            }
        }
    }

    public void UnEquip(EquippableItem item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            inventory.AddItem(item);
        }
    }
}
