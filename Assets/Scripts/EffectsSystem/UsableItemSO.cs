using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Inventory/Items/Usable Item")]
public class UsableItemSO : ItemSO
{
    public bool IsConsumable;
    public List<UsableItemEffectSO> Effects;

    public virtual void Use(Character character)
    {
        foreach (UsableItemEffectSO effect in Effects)
        {
            effect.ExecuteEffect(this, character);
        }
    }

    public override string GetItemType()
    {
        return IsConsumable ? "Consumable" : "Usable";
    }

    public override string GetDescription()
    {
        sb.Length = 0;

        foreach (UsableItemEffectSO effect in Effects)
        {
            sb.AppendLine(effect.GetDescription());
        }

        return sb.ToString();
    }
}
