using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(HintObject))]
public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private float throwDistance = .5f;

    private HintObject _hint;
    private Light2D _light;
    private string[] toolNames = { "Pickaxe", "Axe", "Empty Bucket", "Full Bucket" };

    private void Start()
    {
        _hint = GetComponent<HintObject>();
        _light = GetComponent<Light2D>();
        _light.enabled = IsTool();
    }

    public void Init(ItemSO itemSO, Vector3 offset)
    {
        this.item = itemSO;
        _light = GetComponent<Light2D>();
        GetComponent<SpriteRenderer>().sprite = item.icon;
        LeanTween.move(gameObject, transform.position + throwDistance * offset, .5f).setEaseOutCirc();
        _light.enabled = IsTool();
    }

    private bool IsTool()
    {
        foreach (var tool in toolNames)
        {
            if (item.itemName == tool)
                return true;
        }
        return false;
    }

    private void PickUp()
    {
        if (FindObjectOfType<Inventory>().PickUp(item))
        {
            HintText.HideHint(transform);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_hint.PlayerNear && Input.GetButtonDown("Interact"))
            PickUp();
    }
}
