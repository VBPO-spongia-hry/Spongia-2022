using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    [CreateAssetMenu(menuName = "Tower/Floor", fileName = "New Floor", order = 1)]
    public class FloorSO : ScriptableObject
    {
        public string floorName;
        public string[] items;
        public int level;
        public Crafting.CraftingRecipe recipe;
        public Crafting.CraftingRecipe unlockRecipe;
    }
}
