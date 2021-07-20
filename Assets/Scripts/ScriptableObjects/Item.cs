using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField] string id;
    public string ID {  get { return id; } }
    public string ItemName;
    public Sprite Icon;

    private void OnValidate() // this is not blue - is something wrong?
    {
        // gives us a unique ID for each item from the one Unity already uses
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }
}
