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

        public void Drag(InventorySlot from)
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

        public void OnEndDrag(PointerEventData eventData)
        {
            if (TowerMode)
            {
                if (_from == null) return;
                if (Zone == null)
                {
                    LeanTween.move(gameObject, _from.transform.position, .5f).setEaseInOutCirc().setOnComplete(() =>
                    {
                        CancelDrag();
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
                        var from = _from;
                        LeanTween.move(gameObject, _from.transform.position, .5f).setEaseInOutCirc().setOnComplete(() =>
                        {
                            from.AssignItem(item);
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

        public void CancelDrag()
        {
            if (_from)
                _from.AssignItem(item);
            Clear();
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
