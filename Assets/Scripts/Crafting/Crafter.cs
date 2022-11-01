using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
    public class Crafter : MonoBehaviour
    {
        private CraftingRecipe recipe;
        [SerializeField] private GameObject craftingSlotPrefab;
        [SerializeField] private GameObject resultSlotPrefab;
        [SerializeField] private Slider craftingSlider;
        private Image resultPreview;
        private DropZone[] _slots;
        private InventorySlot resultSlot;
        private CraftingRecipe[] _floorRecipes;
        private int _recipeIndex = 0;
        public event Action onCraftingComplete;

        public void InitFloorCrafter(CraftingRecipe[] floorRecipes)
        {
            Debug.Log(floorRecipes.Length);
            _floorRecipes = floorRecipes;
            InitCrafter(floorRecipes[0]);
            onCraftingComplete += () =>
            {
                _recipeIndex++;
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
                InitCrafter(_floorRecipes[_recipeIndex]);
            };
        }

        public void InitCrafter(CraftingRecipe recipe)
        {
            craftingSlider.gameObject.SetActive(false);
            this.recipe = recipe;
            if (recipe == null)
                return;
            _slots = new DropZone[recipe.ingredients.Length];
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i] = Instantiate(craftingSlotPrefab, transform).GetComponent<DropZone>();
                _slots[i].accepts = recipe.ingredients[i].item;
                _slots[i].capacity = recipe.ingredients[i].count;
                _slots[i].Init();
            }
            if (recipe.result)
            {
                resultSlot = Instantiate(resultSlotPrefab, transform).GetComponent<InventorySlot>();
                resultSlot.GetComponent<Button>().onClick.AddListener(() => OnResultTaken());
                resultPreview = resultSlot.transform.Find("ResultPreview").GetComponent<Image>();
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
