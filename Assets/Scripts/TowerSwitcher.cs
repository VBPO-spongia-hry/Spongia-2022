using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HintObject))]
public class TowerSwitcher : MonoBehaviour
{
    public Sprite[] TowerLevels;
    private HintObject _hint;
    private SpriteRenderer _renderer;
    private void Start()
    {
        _hint = GetComponent<HintObject>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        if (_hint.PlayerNear && Input.GetButtonDown("Interact"))
        {
            ShowTower();
        }
    }

    private void OnEnable()
    {
        if (Tower.Tower.Instance != null && Tower.Tower.Instance.level < TowerLevels.Length)
            SetSprite();
    }

    private void SetSprite()
    {
        Destroy(_renderer.GetComponent<PolygonCollider2D>());
        _renderer.sprite = TowerLevels[Tower.Tower.Instance.level - 1];
        _renderer.gameObject.AddComponent<PolygonCollider2D>();
    }

    private void ShowTower()
    {
        Tower.Tower.Show();
    }
}
