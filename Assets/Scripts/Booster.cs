using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

[Serializable]
public enum BoosterType
{
    movement,
    inventory,
    vision,
    mining,
    crafting,
}

[CreateAssetMenu(menuName = "Inventory/Booster", fileName = "Booster")]
public class Booster : ScriptableObject
{
    public string boosterName;
    public BoosterType type;
    public float boostAmount;

    public void Activate()
    {
        Debug.Log("activate");
        switch (type)
        {
            case BoosterType.movement:
                PlayerController.speedBoost *= boostAmount;
                break;
            case BoosterType.inventory:
                FindObjectOfType<Inventory>().numSlots++;
                break;
            case BoosterType.mining:
                Resource.miningBooster *= boostAmount;
                break;
            case BoosterType.crafting:
                Crafting.Crafter.craftingBoost *= boostAmount;
                break;
            case BoosterType.vision:
                PlayerController.orthoSize *= boostAmount;
                break;
        }
    }
}
