using UnityEngine;

[CreateAssetMenu(fileName = "New Health Effect", menuName = "Inventory/Effects/Health Effect")]
public class HealItemEffectSO : UsableItemEffectSO
{
    public int HealthAmount;

    public override void ExecuteEffect(UsableItemSO parentItem, Character character)
    {
        character.Health += HealthAmount;
    }

    public override string GetDescription()
    {
        return "Heals for " + HealthAmount + " health.";
    }
}
