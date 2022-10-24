using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

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

    private void Start()
    {
        _group = GetComponent<CanvasGroup>();
        _instance = this;
        HideHint();
    }

    private void Show(Transform focus, string message, string key)
    {
        _focusTransform = focus;
        var image = keys.FirstOrDefault((k) => k.name == key);
        text.text = message;
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

    public static void ShowHint(Transform focus, string message, string key)
    {
        if (_hintShown) return;
        _hintShown = true;
        _instance.Show(focus, message, key);
    }

    public static void HideHint()
    {
        _hintShown = false;
        _instance.Hide();
    }

    private void Update()
    {
        if (!_hintShown) return;
        var pos = _focusTransform.position + panelOffset;
        transform.position = Camera.main.WorldToScreenPoint(pos);
    }
}
