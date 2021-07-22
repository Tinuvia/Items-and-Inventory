public class EquipmentSlot : ItemSlot
{
    public EquipmentType EquipmentType;

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = EquipmentType.ToString() + " Slot";
    }

    public override bool CanReceiveItem(ItemSO item)
    {
        if (item == null)
            return true;

        EquippableItemSO equippableItem = item as EquippableItemSO;
        return equippableItem != null && equippableItem.EquipmentType == EquipmentType;
    }
}
