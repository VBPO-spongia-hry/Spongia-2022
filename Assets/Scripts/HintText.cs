using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

[System.Serializable]
public struct KeyboardHint
{
    public string name;
    public Sprite image;
}

public class HintText : MonoBehaviour
{
    private static HintText _instance;
    private static bool _hintShown;
    private Transform _focusTransform;
    private CanvasGroup _group;
    [SerializeField] private Vector3 panelOffset;
    [SerializeField] private Image keyboardHint;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private KeyboardHint[] keys;
    private Transform _player;
    private static Dictionary<Transform, Hint> _hintsToShow;

    private struct Hint
    {
        public string key;
        public string message;
        public Transform follow;
        public Action onShow;
    }

    private void Start()
    {
        _group = GetComponent<CanvasGroup>();
        _instance = this;
        _player = FindObjectOfType<PlayerController>().transform;
        _hintsToShow = new Dictionary<Transform, Hint>();
        Hide();
    }

    private void Show(Hint hint)
    {
        _focusTransform = hint.follow;
        var image = keys.FirstOrDefault((k) => k.name == hint.key);
        text.text = hint.message;
        hint.onShow();
        text.ForceMeshUpdate();
        text.GetComponent<RectTransform>().sizeDelta = text.textBounds.size;
        keyboardHint.sprite = image.image;
        _group.alpha = 0;
        _group.blocksRaycasts = true;
        LeanTween.cancel(gameObject);
        LeanTween.alphaCanvas(_group, 1, .2f).setEaseInExpo();
    }

    private void Hide()
    {
        _group.blocksRaycasts = false;
        _focusTransform = null;
        LeanTween.cancel(gameObject);
        LeanTween.alphaCanvas(_group, 0, .2f).setEaseOutSine();
    }

    public static void ShowHint(Transform focus, string message, string key, Action onShow)
    {
        if (_hintShown) return;
        _hintsToShow[focus] = new Hint
        {
            follow = focus,
            key = key,
            message = message,
            onShow = onShow
        };
        // _instance.Show(focus, message, key);
        // _hintShown = true;
    }

    public static void HideHint(Transform focus)
    {
        // _instance.Hide(focus);
        _hintsToShow.Remove(focus);
    }
    private Hint? _lastHint;
    private void Update()
    {
        var hint = GetClosestHint();
        if (hint.HasValue)
        {
            if (!_lastHint.HasValue || hint.Value.follow != _lastHint.Value.follow)
            {
                Hide();
                Show(hint.Value);
            }
            var pos = _focusTransform.position + panelOffset;
            transform.position = Camera.main.WorldToScreenPoint(pos);
        }
        else
        {
            if (_lastHint.HasValue)
                Hide();
            Resource.activeResource = null;
        }
        _lastHint = hint;
    }

    private Hint? GetClosestHint()
    {
        var minDist = float.PositiveInfinity;
        Hint? ret = null;
        foreach (var hint in _hintsToShow)
        {
            var dist = Physics2D.Distance(hint.Key.GetComponent<Collider2D>(), _player.GetComponent<Collider2D>()).distance;
            if (dist < minDist)
            {
                minDist = dist;
                ret = hint.Value;
            }
        }
        return ret;
    }
}
