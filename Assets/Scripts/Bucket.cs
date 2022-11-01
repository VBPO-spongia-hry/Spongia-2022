using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bucket : MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [SerializeField] private ItemSO emptyBucket;
    [SerializeField] private ItemSO fullBucket;
    [SerializeField] private AudioClip fillClip;
    [SerializeField] private TileBase waterTile;

    private Inventory _inventory;
    private Vector3Int[] directions = { Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down };

    private void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
    }
    private bool _hintShown;
    private void Update()
    {
        if (_inventory.ActiveTool != emptyBucket)
        {
            if (_hintShown)
            {
                HintText.HideHint(transform);
                _hintShown = false;
            }
            return;
        }
        var pos = map.WorldToCell(transform.parent.position);
        if (IsWaterNearby(pos))
        {
            if (!_hintShown)
            {
                HintText.ShowHint(transform, "Fill Bucket", "E", () => { Debug.Log("ShowingHint"); });
                _hintShown = true;
            }
            if (Input.GetButtonDown("Interact"))
            {
                LeanAudio.playClipAt(fillClip, transform.position);
                _inventory.ReplaceActiveItem(fullBucket);
            }
        }
        else if (_hintShown)
        {
            HintText.HideHint(transform);
            _hintShown = false;
        }
    }

    private bool IsWaterNearby(Vector3Int pos)
    {
        foreach (var dir in directions)
        {
            if (map.GetTile(pos + dir) == waterTile)
                return true;
        }
        return false;
    }
}
