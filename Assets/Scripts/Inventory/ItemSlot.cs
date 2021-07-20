using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] Image image;
    [SerializeField] ItemTooltip tooltip;
    [SerializeField] Text amountText;

    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
    public event Action<ItemSlot> OnRightClickEvent; //need to fix this later - from Item to ItemSlot
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;

    private Color normalColor = Color.white;
    private Color disabledColor = new Color(1,1,1,0); // transparent

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
                image.color = disabledColor;
            }
            else
            {
                image.sprite = _item.Icon;
                image.color = normalColor;
            }
        }
    }

    private int _amount;
    public int Amount {
        get { return _amount; }
        set {
            _amount = value;
            amountText.enabled = _item != null && _item.MaximumStacks > 1 && _amount > 1;
            if (amountText.enabled) {
                amountText.text = _amount.ToString();
            }
        }
    }

    // Is only called in the editor, triggers when script is loaded or items changed in editor
    // In this case it's used to automatically fill in all the images in the slots
    protected virtual void OnValidate()
    // protected and virtual so we can override from the Equipment script
    {
        if (image == null)
            image = GetComponent<Image>();

        if (amountText == null)
            amountText = GetComponentInChildren<Text>();
    }

    public virtual bool CanReceiveItem(Item item)
    {
        return true;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClickEvent?.Invoke(this); 
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropEvent?.Invoke(this);
    }
}
