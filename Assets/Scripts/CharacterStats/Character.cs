using UnityEngine;
using Tinuvia.CharacterStats;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public int Health = 50;  

    public CharacterStat Strength;
    public CharacterStat Agility;
    public CharacterStat Intelligence;
    public CharacterStat Vitality;

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] CraftingWindow craftingWindow;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;

    private BaseItemSlot dragItemSlot;


    private void OnValidate()
    {
        if (itemTooltip == null)
        {
            itemTooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    private void Start()
    {
        statPanel.SetStats(Strength, Agility, Intelligence, Vitality);
        statPanel.UpdateStatValues();

        // Setup Events;
        // --- Right Click
        inventory.OnRightClickEvent += InventoryRightClick;
        equipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
        // --- Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        craftingWindow.OnPointerEnterEvent += ShowTooltip;
        // --- Pointer Exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        craftingWindow.OnPointerEnterEvent += ShowTooltip;
        // --- Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        // --- End Drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        // --- Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        // --- Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
    }

    private void InventoryRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItemSO)
        {
            Equip((EquippableItemSO)itemSlot.Item);
        }
        else if (itemSlot.Item is UsableItemSO)
        {
            UsableItemSO usableItem = (UsableItemSO)itemSlot.Item;
            usableItem.Use(this);

            if (usableItem.IsConsumable)
            {
                inventory.RemoveItem(usableItem);
                usableItem.Destroy();
            }
        }
    }

    private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItemSO)
        {
            Unequip((EquippableItemSO)itemSlot.Item);
        }
    }

    private void ShowTooltip(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            itemTooltip.ShowTooltip(itemSlot.Item);
        }
    }

    private void HideTooltip(BaseItemSlot itemSlot)
    {
        itemTooltip.HideTooltip();
    }


    private void BeginDrag(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            dragItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(BaseItemSlot itemSlot)
    {
        dragItemSlot = null;
        draggableItem.enabled = false;
    }

    private void Drag(BaseItemSlot itemSlot)
    {
        if (draggableItem.enabled)
        {
            draggableItem.transform.position = Input.mousePosition;
        }
    }

    private void Drop(BaseItemSlot dropItemSlot)
    {
        if (dragItemSlot == null) return; //  from tutorial #12, but should probably be old draggedSlot --> renaming all those instances to new dragItemSlot

        // adds stacking
        if (dropItemSlot.CanAddStack(dragItemSlot.Item)) // checking if item is stackable
        {
            AddStacks(dropItemSlot);
        }
        // adds swapping (for equipment or other non-stackable items)
        else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
        {
            SwapItems(dropItemSlot);
        }
    }

    private void SwapItems(BaseItemSlot dropItemSlot)
    {
        EquippableItemSO dragItem = dragItemSlot.Item as EquippableItemSO;
        EquippableItemSO dropItem = dropItemSlot.Item as EquippableItemSO;

        if (dragItemSlot is EquipmentSlot)
        {
            // we are dragging an item OUT of an equipment slot
            if (dragItem != null) dragItem.Unequip(this);
            if (dropItem != null) dropItem.Equip(this);
        }

        if (dropItemSlot is EquipmentSlot)
        {
            // we are dragging an item INTO of an equipment slot
            if (dragItem != null) dragItem.Equip(this);
            if (dropItem != null) dropItem.Unequip(this);
        }

        statPanel.UpdateStatValues();

        // if dropped on replaceable - swap
        ItemSO draggedItem = dragItemSlot.Item;
        int draggedItemAmount = dragItemSlot.Amount;

        dragItemSlot.Item = dropItemSlot.Item;
        dragItemSlot.Amount = dropItemSlot.Amount;

        dropItemSlot.Item = draggedItem;
        dropItemSlot.Amount = draggedItemAmount;
    }

    private void AddStacks(BaseItemSlot dropItemSlot)
    {
        //Add stacks until dropItemSlot is full
        int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
        int stacksToAdd = Mathf.Min(numAddableStacks, dragItemSlot.Amount); // we add the smallest amount of the two

        dropItemSlot.Amount += stacksToAdd;
        //Remove the same number of stacks from dragItemSlot
        dragItemSlot.Amount -= stacksToAdd;
    }

    public void Equip(EquippableItemSO item)
    {
        if (inventory.RemoveItem(item))
        {
            EquippableItemSO previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null) // if this slot is occupied, return previous to inventory
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);
                statPanel.UpdateStatValues();
                    
            }
            else // if we can't equip it, return new to inventory
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItemSO item)
    {
        if (!inventory.CanAddItem(item) && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
        }
    }

    // we need a way to access this method (for the stat buff effects) since the statPanel in here is private
    public void UpdateStatValues()
    {
        statPanel.UpdateStatValues();
    }
}
