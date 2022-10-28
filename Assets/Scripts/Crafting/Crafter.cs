using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
    public class Crafter : MonoBehaviour
    {
        [SerializeField] private CraftingRecipe recipe;
        [SerializeField] private Slider craftingSlider;
        [SerializeField] private Image resultPreview;
        private DropZone[] _slots;
        public InventorySlot resultSlot;
        public event Action onCraftingComplete;

        private void Start()
        {
            _slots = GetComponentsInChildren<DropZone>();
            craftingSlider.gameObject.SetActive(false);
            if (_slots.Length != recipe.ingredients.Length)
                Debug.LogError("Invalid number of slots for recipe");
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].accepts = recipe.ingredients[i].item;
                _slots[i].capacity = recipe.ingredients[i].count;
                _slots[i].Init();
            }
            if (recipe.result)
            {
                resultPreview.sprite = recipe.result.icon;
            }
        }

        public void OnZoneUpdated()
        {
            if (resultSlot && resultSlot.IsFull) return;
            if (HasAllIngredients())
            {
                foreach (var slot in _slots)
                {
                    slot.Clear();
                }
                craftingSlider.gameObject.SetActive(true);
                LeanTween.value(gameObject, 0, 1, recipe.craftingTime).setOnUpdate((val) =>
                {
                    craftingSlider.value = val;
                }).setOnComplete(OnCraftingComplete);
            }
        }

        private void OnCraftingComplete()
        {
            onCraftingComplete?.Invoke();
            if (recipe.result)
                resultSlot.AssignItem(recipe.result);
            else
                Tower.Tower.UnlockNextFloor();
            craftingSlider.gameObject.SetActive(false);
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

        public void OnResultTaken()
        {
            if (!resultSlot.IsFull) return;
            if (FindObjectOfType<Inventory>().PickUp(recipe.result))
            {
                resultSlot.ThrowItem();
            }
        }
    }
}
