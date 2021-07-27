using UnityEngine;

public class InfiniteInventory : Inventory
{
    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] int maxSlots;

    public int MaxSlots {
        get { return maxSlots;  }
        set { SetMaxSlots(value); }
    }

    protected override void Awake()
        // we can't use OnValidate() since Unity doesn't like calling Destroy in OnValidate
    {
        SetMaxSlots(maxSlots); // must be assigned before the general Start() with its assignments
        base.Awake();
    }

    private void SetMaxSlots(int value)
    {
        if (value <= 0){
            maxSlots = 1;
        } else {
            maxSlots = value;
        }

        if (maxSlots < ItemSlots.Count)
        {
            for (int i = maxSlots; i < ItemSlots.Count; i++)
                // the last slot we want to keep is at index maxSlots-1 (because of lists starting at 0)
            {
                Destroy(ItemSlots[i].transform.parent.gameObject);
            }
            int diff = ItemSlots.Count - maxSlots;
            ItemSlots.RemoveRange(maxSlots, diff);

        } else if (maxSlots > ItemSlots.Count)
        {
            int diff = maxSlots - ItemSlots.Count;

            for (int i = 0; i < diff; i++)
            {
                GameObject itemSlotGameObj = Instantiate(itemSlotPrefab);
                itemSlotGameObj.transform.SetParent(itemsParent, worldPositionStays: false);
                ItemSlots.Add(itemSlotGameObj.GetComponentInChildren<ItemSlot>());
            }
        }

    }
}
