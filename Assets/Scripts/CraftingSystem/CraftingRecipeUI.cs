using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform arrowParent;
    [SerializeField] BaseItemSlot[] itemSlots;

    [Header("Public Variables")]
    public ItemContainer ItemContainer;

    private CraftingRecipeSO craftingRecipe;
    public CraftingRecipeSO CraftingRecipe {
        get { return craftingRecipe;  }
        set { SetCraftingRecipe(value); }
    }

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    // only these two events, since these slots are "read-only" as recipes


    private void OnValidate()
    {
        itemSlots = GetComponentsInChildren<BaseItemSlot>(includeInactive: true);
    }

    void Start()
    {
        foreach (BaseItemSlot itemSlot in itemSlots)
        {
            itemSlot.OnPointerEnterEvent += (slot) => OnPointerEnterEvent(slot);
            itemSlot.OnPointerExitEvent += (slot) => OnPointerExitEvent(slot);
        }        
    }

    public void OnCraftButtonClick()
    {
        if (craftingRecipe != null && ItemContainer != null)
        {
            if (craftingRecipe.CanCraft(ItemContainer))
            {
                if (!ItemContainer.IsFull())
                {
                    craftingRecipe.Craft(ItemContainer);
                } else
                {
                    Debug.LogError("Inventory is full!");
                }
            }
            else
            {
                Debug.LogError("You don't have the reqired materials!");
            }
        }
    }

    private void SetCraftingRecipe(CraftingRecipeSO newCraftingRecipe)
    {
        craftingRecipe = newCraftingRecipe;

        if (craftingRecipe != null)
        {
            int slotIndex = 0;
            slotIndex = SetSlots(craftingRecipe.Materials, slotIndex);
            arrowParent.SetSiblingIndex(slotIndex);
            slotIndex = SetSlots(craftingRecipe.Results, slotIndex);

            for (int i = slotIndex; i < itemSlots.Length; i++)
            {
                itemSlots[i].transform.parent.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private int SetSlots(IList<ItemAmount> itemAmountList, int slotIndex)
        // IList can receive either a list or an array
    {
        for (int i = 0; i < itemAmountList.Count; i++, slotIndex++)
        {
            ItemAmount itemAmount = itemAmountList[i];
            BaseItemSlot itemSlot = itemSlots[slotIndex];

            itemSlot.Item = itemAmount.Item;
            itemSlot.Amount = itemAmount.Amount;
            itemSlot.transform.parent.gameObject.SetActive(true);
        }
        return slotIndex;
    }
}
