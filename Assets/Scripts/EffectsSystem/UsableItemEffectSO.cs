using UnityEngine;

public abstract class UsableItemEffectSO : ScriptableObject
{
    public abstract void ExecuteEffect(UsableItemSO parentItem, Character character);
    public abstract string GetDescription();
    // abstract since all functionlity will reside in other scripts  
    // parent item parameter so we can tell which item caused the effect

}
