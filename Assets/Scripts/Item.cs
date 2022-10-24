using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HintObject))]
public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private float throwDistance = .5f;

    private HintObject _hint;

    private void Start()
    {
        _hint = GetComponent<HintObject>();
    }

    public void Init(ItemSO itemSO)
    {
        this.item = itemSO;
        GetComponent<SpriteRenderer>().sprite = item.icon;
        var offset = Random.insideUnitCircle;
        LeanTween.move(gameObject, transform.position + throwDistance * new Vector3(offset.x, offset.y), .5f).setEaseOutCirc();
    }

    private void PickUp()
    {
        FindObjectOfType<Inventory>().PickUp(item);
        HintText.HideHint();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (_hint.PlayerNear && Input.GetButtonDown("Interact"))
            PickUp();
    }
}
