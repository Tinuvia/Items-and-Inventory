using System.Collections.Generic;
using UnityEngine;

public class ItemSaveManager : MonoBehaviour
{
    private const string InventoryFileName = "Inventory";
    private const string EquipmentFileName = "Equipment";

    public void LoadInventory(Character character)
    {
        ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(InventoryFileName);
        if (savedSlots == null) return;

        character.Inventory.Clear(); // removing any items that might be in it already

        for (int i = 0; i < savedSlots.SavedSlots.Length; i++)
        {
            ItemSlot itemSlot = character.Inventory.ItemSlots[i];
            ItemSlotSaveData savedSlot = savedSlots.SavedSlots[i];

            if (savedSlot == null)
            {
                itemSlot.Item = null;
                itemSlot.Amount = 0;
            } else
            {
                // itemSlot.Item = ;
                itemSlot.Amount = savedSlot.Amount;
            }
        }
    }

    public void LoadEquipment(Character character)
    {
        ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(EquipmentFileName);
        if (savedSlots == null) return;

        foreach (ItemSlotSaveData savedSlot in savedSlots.SavedSlots)
        {
            if (savedSlot == null) {
                continue;
            }

            ItemSO item = null; //will fix next episode
            character.Inventory.AddItem(item); // temporary workaround, since Equip only takes the item from the inventory
            character.Equip((EquippableItemSO)item);
        }
    }

    public void SaveInventory(Character character)
    {
        SaveItems(character.Inventory.ItemSlots, InventoryFileName);
    }

    public void SaveEquipment(Character character)
    {
        SaveItems(character.EquipmentPanel.EquipmentSlots, EquipmentFileName);
    }

    private void SaveItems(IList<ItemSlot> itemSlots, string fileName)
    {
        var saveData = new ItemContainerSaveData(itemSlots.Count);

        for (int i = 0; i < saveData.SavedSlots.Length; i++)
        {
            ItemSlot itemSlot = itemSlots[i];

            if (itemSlot.Item == null)
            {
                saveData.SavedSlots[i] = null;
            }
            else
            {
                saveData.SavedSlots[i] = new ItemSlotSaveData(itemSlot.Item.ID, itemSlot.Amount);
            }
        }
        ItemSaveIO.SaveItems(saveData, fileName);
    }
}
