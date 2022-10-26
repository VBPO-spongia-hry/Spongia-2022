using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
    public class Crafter : MonoBehaviour
    {
        [SerializeField] private CraftingRecipe recipe;
        [SerializeField] private Image craftingSlider;
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
                _slots[i].Init();
            }
        }

        public void OnZoneUpdated()
        {
            if (resultSlot && resultSlot.IsFull) return;
            if (HasAllIngredients())
            {
                foreach (var slot in _slots)
                {
                    slot.clear();
                }
                LeanTween.value(gameObject, 0, 1, recipe.craftingTime).setOnUpdate((val) =>
                {
                    craftingSlider.fillAmount = val;
                }).setOnComplete(() =>
                {
                    if (resultSlot)
                        resultSlot.AssignItem(recipe.result);
                    else
                        Tower.Tower.UnlockNextFloor();
                });
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
