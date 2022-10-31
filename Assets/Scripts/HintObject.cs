using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintObject : MonoBehaviour
{
    [SerializeField] private string hintMessage;
    [SerializeField] private float hintDistance;
    [SerializeField] private string hintKey;
    [SerializeField] private Transform follow;
    [SerializeField] private string disabledHintMessage;
    private Transform _player;
    private bool _hintShown;

    public bool PlayerNear => _hintShown;
    [NonSerialized] public bool IsDisabled;
    public event Action OnHintShow;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>().transform;
        _hintShown = false;
    }

    private void Update()
    {
        var dist = Vector2.Distance(_player.position, transform.position);
        if (dist < hintDistance)
        {
            if (IsDisabled)
                HintText.ShowHint(follow, disabledHintMessage, "Lock", OnHintShown);
            else
                HintText.ShowHint(follow, hintMessage, hintKey, OnHintShown);
            _hintShown = true;
        }
        else
        {
            if (_hintShown)
                HintText.HideHint(follow);
            _hintShown = false;
        }
    }

    public void HideHint()
    {
        HintText.HideHint(follow);
        _hintShown = false;
    }

    private void OnHintShown()
    {
        if (!IsDisabled)
            OnHintShow?.Invoke();
    }

    private void OnDisable()
    {
        if (_hintShown)
            HintText.HideHint(follow);
    }
}
