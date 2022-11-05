using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot[] slots;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject activeItemEffect;
    [SerializeField] private AudioClip equipClip;

    private Transform _player;
    private KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5
    };
    private int _activeSlot;
    private bool _isTowerActive => Tower.Tower.TowerActive;

    public ItemSO ActiveTool => slots[_activeSlot].Item;
    [SerializeField] private int _numSlots = 2;
    public int numSlots
    {
        get => _numSlots;
        set
        {
            _numSlots = value;
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].gameObject.SetActive(i < numSlots);
            }
        }
    }

    private void Start()
    {
        numSlots = _numSlots;
        _player = FindObjectOfType<PlayerController>().transform;
        SetActiveItem(0);
        activeItemEffect.transform.localPosition = Vector3.zero;
        LeanTween.cancel(activeItemEffect);
    }

    private void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
                SetActiveItem(i);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !Tower.Tower.TowerActive)
            ThrowItem();
        activeItemEffect.SetActive(!_isTowerActive);
    }

    public void SetActiveItem(int index)
    {
        if (index >= numSlots) return;
        _activeSlot = index;
        activeItemEffect.transform.SetParent(slots[index].transform, true);
        activeItemEffect.transform.SetSiblingIndex(0);
        LeanTween.moveX(activeItemEffect, slots[index].transform.position.x, .2f).setEaseInOutCirc();
    }

    public bool PickUp(ItemSO item)
    {
        if (!GetEmptySlot()) return false;
        if (slots[_activeSlot].IsFull)
            ThrowItem();
        LeanAudio.play(equipClip);
        slots[_activeSlot].AssignItem(item);
        return true;
    }

    private bool GetEmptySlot()
    {
        for (int i = 0; i < numSlots; i++)
        {
            if (!slots[i].IsFull)
            {
                SetActiveItem(i);
                return true;
            }
        }
        return false;
    }

    private void ThrowItem()
    {
        if (!slots[_activeSlot].IsFull) return;
        var item = slots[_activeSlot].ThrowItem();
        var go = Instantiate(itemPrefab, _player.position, Quaternion.identity).GetComponent<Item>();
        var offset = Random.onUnitSphere;
        go.Init(item, new Vector3(offset.x, offset.y));
    }

    public void ThrowItem(ItemSO item)
    {
        var go = Instantiate(itemPrefab, _player.position, Quaternion.identity).GetComponent<Item>();
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        go.Init(item, (mousePos - _player.position).normalized);
    }

    public void ReplaceActiveItem(ItemSO item)
    {
        slots[_activeSlot].ReplaceItem(item);
    }
}
