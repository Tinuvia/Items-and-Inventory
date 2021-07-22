using System.Text;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] string id;
    public string ID {  get { return id; } }
    public string ItemName;
    [Range(1,999)]
    public int MaximumStacks = 1;
    public Sprite Icon;

    protected static readonly StringBuilder sb = new StringBuilder(); 
    // allows us to not use a new string every time we concatenate texts
    // protected makes it accessible for all our classes
    // set as static so we can share this one instance all over our classes - OK as long as only used on 1 thread (by default since a Unity object)

    private void OnValidate() // this is not blue - is something wrong?
    {
        // gives us a unique ID for each item from the one Unity already uses
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }

    // the base Item class is used for non-unique items, that can be stacked so we don't have to instantiate new copies all the time
    // these methods are overwritten in the equippable item class
    public virtual ItemSO GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {

    }

    public virtual string GetItemType()
    {
        return "";
    }

    public virtual string GetDescription()
    {
        return "";
    }
}
