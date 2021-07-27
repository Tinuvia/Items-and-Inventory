using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New ItemDatabase", menuName = "Inventory/Database/ItemDB")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField] ItemSO[] items;

    public ItemSO GetItemReference(string itemID)
    {
        foreach (ItemSO item in items)
        {
            if (item.ID == itemID)
            {
                return item;
            }
        }
        return null;
    }

    public ItemSO GetItemCopy(string itemID)
    {
        ItemSO item = GetItemReference(itemID);
        if (item == null) return null;
        return item.GetCopy();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        LoadItems();
    }

    private void OnEnable()
    {
        // runs when we add new items to our project
        EditorApplication.projectChanged += LoadItems;
    }

    private void OnDisable()
    {
        EditorApplication.projectChanged -= LoadItems;
    }

    private void LoadItems()
    {
        items = FindAssetsByType<ItemSO>("Assets/Data/Items");
    }

    public static T[] FindAssetsByType<T>(params string[] folders) where T : Object
    {
        string type = typeof(T).ToString().Replace("UnityEngine.", "");
        // Example: string mat = typeof(Material).ToString(); // == UnityEngine.Material

        // used to separate databases for different sets of items
        string[] guids; // Globally Unique Identifier
        if (folders == null || folders.Length == 0) {
            guids = AssetDatabase.FindAssets("t:" + type);
        } else {
            guids = AssetDatabase.FindAssets("t:" + type, folders);
        }

        T[] assets = new T[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
        return assets;
    }
#endif
}
