using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image;
    [SerializeField] ItemTooltip tooltip;

    public event Action<Item> OnRightClickEvent;

    private Item _item;
    public Item Item
    {
        get { return _item; }
        // use the setter to update the item's image - if the item is null the slot is empty and the image should be disabled
        set
        {
            _item = value;

            if(_item == null)
            {
                image.enabled = false;
            }
            else
            {
                image.sprite = _item.Icon;
                image.enabled = true;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item != null && OnRightClickEvent != null)
            {
                OnRightClickEvent(Item);
            }
        }
    }


    // Is only called in the editor, triggers when script is loaded or items changed in editor
    // In this case it's used to automatically fill in all the images in the slots
    protected virtual void OnValidate()
        // protected and virtual so we can override from the Equipment script
    {
        if(image == null)
            image = GetComponent<Image>();

        if (tooltip == null)
            tooltip = FindObjectOfType<ItemTooltip>(); // OK in validate method since is only used in Editor
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item is EquippableItem) // we so far only made the tooltip only work for Equippable items
        {
            tooltip.ShowTooltip((EquippableItem)Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
