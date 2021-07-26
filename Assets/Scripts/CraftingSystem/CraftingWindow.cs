using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CraftingRecipeUI recipeUIPrefab;
    [SerializeField] RectTransform recipeUIParent;
    [SerializeField] List<CraftingRecipeUI> craftingRecipeUIs;

    [Header("Public Variables")]
    public ItemContainer ItemContainer;
    public List<CraftingRecipeSO> CraftingRecipeSOs;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;

    private void OnValidate()
    {
        Init();
    }

    void Start()
    {
        Init();

        foreach (CraftingRecipeUI craftingRecipeUI in craftingRecipeUIs)
        {
            craftingRecipeUI.OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
            craftingRecipeUI.OnPointerExitEvent += slot => OnPointerExitEvent(slot);
        }
    }

    private void Init()
    {
        recipeUIParent.GetComponentsInChildren<CraftingRecipeUI>(includeInactive: true, result: craftingRecipeUIs);
        // we use an override of GetComponentsInChildren that takes a list as a parameter and adds all into that list instead of allocating a new array each time
        UpdateCraftingRecipes();   
    }

    private void UpdateCraftingRecipes()
    {
        for (int i = 0; i < CraftingRecipeSOs.Count; i++)
        {
            if (craftingRecipeUIs.Count == i)
                // if the count is i we are at the end of the list and need to add another element
            {
                craftingRecipeUIs.Add(Instantiate(recipeUIPrefab, recipeUIParent, false));
            } else if (craftingRecipeUIs[i] == null) // for safety purposes - instead of adding to the list, assign to new object
            {
                craftingRecipeUIs[i] = Instantiate(recipeUIPrefab, recipeUIParent, false);
            }

            // now we know that the craftingRecipeUI-object exists and can set it up
            craftingRecipeUIs[i].ItemContainer = ItemContainer;
            craftingRecipeUIs[i].CraftingRecipe = CraftingRecipeSOs[i];
        }

        // if we have more UI-objects than recipes, deactivate the UIs (through the CraftingRecipeUI.SetCraftingRecipe-method)
        for (int i = CraftingRecipeSOs.Count; i < craftingRecipeUIs.Count; i++)
        {
            craftingRecipeUIs[i].CraftingRecipe = null; ;
        }
    }
}
