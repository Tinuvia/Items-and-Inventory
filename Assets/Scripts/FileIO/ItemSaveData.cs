using System;

[Serializable]
public class ItemSlotSaveData
{
    public string ItemID;
    public int Amount;

    public ItemSlotSaveData(string id, int amount)
    {
        ItemID = id;
        Amount = amount;
    }
}

[Serializable]
public class ItemContainerSaveData
{
    public ItemSlotSaveData[] SavedSlots;
    public ItemContainerSaveData(int numItems)
    {
        SavedSlots = new ItemSlotSaveData[numItems];
    }
}

/* we could either:
 *  - save the ItemID and Amount (less data, just a string. If we update our game, the updated items can be retrieved)
 *  - the object itself (supports enchantments etc automatically but would need converter if game is updated)
 */
