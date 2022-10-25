using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crafting
{
    public class Crafter : MonoBehaviour
    {
        [SerializeField] private CraftingRecipe recipe;
        private DropZone[] _slots;
        public InventorySlot resultSlot;

        private void Start()
        {
            _slots = GetComponentsInChildren<DropZone>();
            if (_slots.Length != recipe.ingredients.Length)
                Debug.LogError("Invalid number of slots for recipe");
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].accepts = recipe.ingredients[i].item;
                _slots[i].capacity = recipe.ingredients[i].count;
            }
        }

        public void OnZoneUpdated()
        {
            if (HasAllIngredients() && !resultSlot.IsFull)
            {
                resultSlot.AssignItem(recipe.result);
            }
        }

        private bool HasAllIngredients()
        {
            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                if (!_slots[i].Full)
                    return false;
            }

            return true;
        }
    }
}
