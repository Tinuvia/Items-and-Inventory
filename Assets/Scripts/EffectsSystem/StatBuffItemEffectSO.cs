using System.Collections;
using UnityEngine;
using Tinuvia.CharacterStats;

[CreateAssetMenu(fileName = "New Stat Buff", menuName = "Inventory/Effects/Stat Buff")]
public class StatBuffItemEffectSO : UsableItemEffectSO
{
    public int AgilityBuffAmount;
    public int IntelligenceBuffAmount;
    public int StrengthBuffAmount;
    public int VitalityBuffAmount;
    public float Duration;

    public override void ExecuteEffect(UsableItemSO parentItem, Character character)
    {
        StatModifier statModifier = (new StatModifier(AgilityBuffAmount, StatModType.Flat, parentItem));
        character.Agility.AddModifier(statModifier);
        character.UpdateStatValues();
        // Coroutines need to be run on Monobehaviors, the only one we have access to is the character
        character.StartCoroutine(RemoveBuff(character, statModifier, Duration));
    }

    public override string GetDescription()
    {
        return "Grants " + AgilityBuffAmount + " Agility for " + Duration + " seconds.";
    }

    private static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
    {
        yield return new WaitForSeconds(duration);
        character.Agility.RemoveModifier(statModifier);
        character.UpdateStatValues();
    }

    // REFACTORING  
    // to have several effects, see UsableItemSO GetDescription-method? Have a list/array initial for all buffs to loop over?
}