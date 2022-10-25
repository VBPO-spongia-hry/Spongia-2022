using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crafting
{
    public class DragSlot : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public static DropZone Zone;
        public ItemSO item;
        [SerializeField] private Image itemImage;
        private InventorySlot _from;

        public void Drag(InventorySlot from, PointerEventData data)
        {
            if (!from.IsFull || _from != null) return;
            _from = from;
            transform.position = _from.transform.position;
            item = from.ThrowItem();
            itemImage.enabled = true;
            itemImage.sprite = item.icon;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.dragging)
                transform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (_from == null) return;
            LeanTween.move(gameObject, _from.transform.position, .5f).setEaseInOutCirc().setOnComplete(() =>
            {
                if (Zone == null)
                {
                    _from.AssignItem(item);
                }
                else
                {
                    Zone.Drop(item);
                }
                item = null;
                itemImage.enabled = false;
                itemImage.sprite = null;
                _from = null;
            });

        }
    }
}
