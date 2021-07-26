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

    protected bool isPointerOver;
    protected Color normalColor = Color.white;
    protected Color disabledColor = new Color(1, 1, 1, 0); // transparent

    protected ItemSO _item;
    public ItemSO Item {
        get { return _item; }
        // use the setter to update the item's image - if the item is null the slot is empty and the image should be disabled
        set {
            _item = value;

            if (_item == null && Amount != 0)
                Amount = 0;

            if (_item == null) {
                image.sprite = null;
                image.color = disabledColor;
            } else {
                image.sprite = _item.Icon;
                image.color = normalColor;
            }

            // re-trigger mouse-over if we already had our mouse over the object 
            if (isPointerOver)
            {
                OnPointerExit(null);
                OnPointerEnter(null);
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
            if (_amount == 0 && Item != null) Item = null;

            if (amountText != null)
            {
                amountText.enabled = _item != null && _amount > 1;
                if (amountText.enabled) {
                    amountText.text = _amount.ToString();
                }
            }
        }
    }

    public virtual bool CanAddStack(ItemSO item, int amount = 1)
    {
        return ((Item != null) && (Item.ID == item.ID));
    }

    public virtual bool CanReceiveItem(ItemSO item)
    {
        return false;
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

        Item = _item;
        Amount = _amount;
    }

    // workaround for Unity not registering on mouse-over events on disabled objects
    protected virtual void OnDisable()
    {
        if(isPointerOver)
            OnPointerExit(null);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (OnRightClickEvent != null)
            {
                OnRightClickEvent(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;

        if (OnPointerEnterEvent != null)
        {
            OnPointerEnterEvent(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;

        if (OnPointerExitEvent != null)
        {
            OnPointerExitEvent(this);
        } 
    }
}
