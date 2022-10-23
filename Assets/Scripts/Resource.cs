using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    public static Transform player;
    [SerializeField] private Transform boneEffect;
    [SerializeField] private ItemSO drop;
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private float breakTime = 5f;
    [SerializeField] private float breakDistance = .8f;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject itemprefab;
    public static Resource activeResource;
    private bool _playerNear = false;
    private bool _isBreaking = false;
    private bool _isBroken = false;

    private void Start()
    {
        slider.maxValue = breakTime;
        slider.value = breakTime;
        slider.gameObject.SetActive(false);
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        if (_isBroken) return;
        var distance = Vector2.Distance(transform.position, player.position);
        if (distance < breakDistance)
        {
            _playerNear = true;
            HintText.ShowHint(transform, "Mine resource", "LMB");
            activeResource = this;
        }
        else
        {
            if (_playerNear)
                HintText.HideHint();
            _playerNear = false;
            activeResource = null;
        }
        if (_isBreaking && !_playerNear)
            StopBreaking();
    }

    public void Break()
    {
        if (!_playerNear || _isBroken) return;
        _isBreaking = true;
        slider.gameObject.SetActive(true);
        StartCoroutine(StartBreaking());
    }

    public void StopBreaking()
    {
        StopAllCoroutines();
        _isBreaking = false;
        slider.value = breakTime;
        slider.gameObject.SetActive(false);
    }

    private IEnumerator StartBreaking()
    {
        var time = breakTime;
        while (time > 1)
        {
            time--;
            slider.value = time;
            LeanTween.rotateLocal(boneEffect.gameObject, Vector3.forward * Random.Range(-20f, 20f), .2f).setEaseShake();
            yield return new WaitForSeconds(1);
        }
        if (brokenSprite == null)
            Destroy(gameObject);
        else
        {
            _isBroken = true;
            GetComponentInChildren<SpriteRenderer>().sprite = brokenSprite;
        }

        if (drop != null)
        {
            var go = Instantiate(itemprefab, transform.position, Quaternion.identity);
            go.GetComponent<Item>().Init(drop);
        }
    }
}
