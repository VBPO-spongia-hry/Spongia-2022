using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private bool enableDrag = true;
    private ItemSO _item;

    public bool IsFull => _item != null;

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

    private bool _isMouseOver = false;
    private void Update()
    {
        if (!enableDrag) return;
        var nowOver = IsPointerOverGameobject();
        if (nowOver == _isMouseOver) return;
        _isMouseOver = nowOver;
        if (nowOver) { OnPointerEnter(); }
        else { OnPointerExit(); }
    }

    public void OnPointerEnter()
    {
        if (enableDrag && IsFull)
            FindObjectOfType<Crafting.DragSlot>().Drag(this);
    }

    public void OnPointerExit()
    {
        if (enableDrag && !Input.GetMouseButton(0))
            FindObjectOfType<Crafting.DragSlot>().CancelDrag();
    }

    private bool IsPointerOverGameobject()
    {
        var eventSystemRaysastResults = GetEventSystemRaycastResults();
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject == gameObject)
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

}
