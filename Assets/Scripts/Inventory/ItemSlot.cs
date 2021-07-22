using UnityEngine;
using UnityEngine.EventSystems;
using System;

// this class holds everything related to dragging
public class ItemSlot : BaseItemSlot, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    private Color dragColor = new Color(1,1,1,0.5f); // semi-transparent

    public override bool CanReceiveItem(Item item)
    {
        return true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item != null)
            image.color = dragColor;

        OnBeginDragEvent?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Item != null)
            image.color = normalColor;

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
