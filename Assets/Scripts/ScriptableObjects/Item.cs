using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField] string id;
    public string ID {  get { return id; } }
    public string ItemName;
    [Range(1,999)]
    public int MaximumStacks = 1;
    public Sprite Icon;

    private void OnValidate() // this is not blue - is something wrong?
    {
        // gives us a unique ID for each item from the one Unity already uses
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }

    // the base Item class is used for non-unique items, that can be stacked so we don't have to instantiate new copies all the time
    // these methods are overwritten in the equippable item class
    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {

    }
}
