using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// this class holds everything related to clicking
public class BaseItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    // new class because these are read-only slots, no drag-and-drop interactivity
    // but the scroll rect would get intercepted if we used the old ItemSlot class
{
    [SerializeField] protected Image image;
    [SerializeField] protected Text amountText;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;

    protected Color normalColor = Color.white;
    protected Color disabledColor = new Color(1, 1, 1, 0); // transparent

    protected ItemSO _item;
    public ItemSO Item {
        get { return _item; }
        // use the setter to update the item's image - if the item is null the slot is empty and the image should be disabled
        set {
            _item = value;

            if (_item == null)
                image.color = disabledColor;
            else {
                image.sprite = _item.Icon;
                image.color = normalColor;
            }
        }
    }

    private int _amount;
    public int Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            if (_amount < 0) _amount = 0; 
            if (_amount == 0) Item = null;

            if (amountText != null)
            {
                amountText.enabled = _item != null && _amount > 1;
                if (amountText.enabled) {
                    amountText.text = _amount.ToString();
                }
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

    public virtual bool CanReceiveItem(ItemSO item)
    {
        return false;
    }
    
    public virtual bool CanAddStack(ItemSO item, int amount = 1)
    {
        return (Item != null) && (Item.ID == item.ID);
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
}
