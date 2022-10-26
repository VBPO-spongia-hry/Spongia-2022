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
        private bool TowerMode => Tower.Tower.TowerActive;

        public void Drag(InventorySlot from, PointerEventData data)
        {
            if (!from.IsFull || _from != null) return;
            _from = from;
            transform.position = _from.transform.position;
            itemImage.enabled = true;
            item = from.ThrowItem();
            itemImage.sprite = item.icon;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.dragging)
                transform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (TowerMode)
            {
                if (_from == null) return;
                if (Zone == null)
                {
                    LeanTween.move(gameObject, _from.transform.position, .5f).setEaseInOutCirc().setOnComplete(() =>
                    {
                        _from.AssignItem(item);
                        Clear();
                    });
                }
                else
                {
                    if (Zone.Drop(item))
                    {
                        Clear();
                    }
                    else
                    {
                        LeanTween.move(gameObject, _from.transform.position, .5f).setEaseInOutCirc().setOnComplete(() =>
                        {
                            _from.AssignItem(item);
                            Clear();
                        });
                    }
                }
            }
            else
            {
                FindObjectOfType<Inventory>().ThrowItem(item);
                Clear();
            }
        }

        private void Clear()
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
            _from = null;
            item = null;
        }
    }
}
