using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image Image;

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
                Image.enabled = false;
            }
            else
            {
                Image.sprite = _item.Icon;
                Image.enabled = true;
            }
        }
    }


    // Is only called in the editor, triggers when script is loaded or items changed in editor
    // In this case it's used to automatically fill in all the images in the slots
    private void OnValidate()
    {
        if(Image == null)
        {
            Image = GetComponent<Image>();
        }
    }
}
