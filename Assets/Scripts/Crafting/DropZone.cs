using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crafting
{
    public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ItemSO accepts;
        private int count;
        public int capacity;

        public bool Full => count == capacity;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Image acceptsImage;
        [SerializeField] private Crafter _crafter;

        public void Init()
        {
            acceptsImage.sprite = accepts.icon;
            _crafter = GetComponentInParent<Crafter>();
        }

        public bool Drop(ItemSO item)
        {
            if (item.name != accepts.name || count == capacity) return false;
            count++;
            countText.text = count.ToString();
            _crafter.OnZoneUpdated();
            return true;
        }

        private void Update()
        {
            if (IsPointerOverGameobject())
            {
                DragSlot.Zone = this;
            }
            else
            {
                DragSlot.Zone = null;
            }
        }

        public void clear()
        {
            count = 0;
            countText.text = "0";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DragSlot.Zone = this;
            LeanTween.scale(gameObject, Vector3.one * 1.2f, .3f).setEaseInCirc();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DragSlot.Zone = null;
            LeanTween.scale(gameObject, Vector3.one * .8f, .3f).setEaseInCirc();
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
}
