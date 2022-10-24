using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private float throwDistance = .5f;
    public void Init(ItemSO itemSO)
    {
        this.item = itemSO;
        GetComponent<SpriteRenderer>().sprite = item.icon;
        var offset = Random.insideUnitCircle;
        LeanTween.move(gameObject, transform.position + throwDistance * new Vector3(offset.x, offset.y), .5f).setEaseOutCirc();
    }
}
