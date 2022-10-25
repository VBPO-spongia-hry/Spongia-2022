using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace Crafting
{
    public class DropZone : MonoBehaviour
    {
        public ItemSO accepts;
        private int count;
        public int capacity;

        public bool Full => count == capacity;
        private TextMeshProUGUI countText;
        public bool Drop(ItemSO item)
        {
            if (item.name != accepts.name || count == capacity) return false;
            count++;
            countText.text = count.ToString();
            return true;
        }
    }
}
