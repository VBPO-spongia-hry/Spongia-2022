using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Image itemImage;
    private ItemSO _item;

    public bool IsFull => _item != null;

    private bool TowerMode => Tower.Tower.TowerActive;

    public void AssignItem(ItemSO item)
    {
        _item = item;
        itemImage.sprite = item.icon;
        itemImage.gameObject.SetActive(true);
        itemImage.transform.localScale = Vector3.one * .8f;
        LeanTween.cancel(itemImage.gameObject);
        LeanTween.scale(itemImage.gameObject, Vector3.one, .5f).setEaseOutBounce();
    }

    public ItemSO ThrowItem()
    {
        var temp = _item;
        _item = null;
        LeanTween.cancel(itemImage.gameObject);
        LeanTween.scale(itemImage.gameObject, Vector3.one * .8f, .5f).setEaseOutCirc().setOnComplete(() =>
        {
            itemImage.gameObject.SetActive(false);
        });
        return temp;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (!TowerMode) return;
        FindObjectOfType<Crafting.DragSlot>().Drag(this, data);
    }
}
