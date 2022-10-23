using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item", fileName = "Item")]
public class ItemSO : ScriptableObject
{
    public Sprite icon;
    public string itemName;
}
