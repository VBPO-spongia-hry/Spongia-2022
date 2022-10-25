using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crafting
{
    [System.Serializable]
    public class Ingredient
    {
        public ItemSO item;
        public int count;
    }

    [CreateAssetMenu(menuName = "Inventory/Recipe", fileName = "Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public ItemSO result;
        public Ingredient[] ingredients;
        public float craftingTime;
    }
}
