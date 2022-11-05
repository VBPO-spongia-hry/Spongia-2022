using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HintObject))]
public class Resource : MonoBehaviour
{
    [SerializeField] private Transform boneEffect;
    [SerializeField] private ItemSO toolToHarvest;
    [SerializeField] private ItemSO drop;
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private float breakTime = 5f;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject itemprefab;
    [SerializeField] private AudioClip[] chopClips;
    public static Resource activeResource;
    public static float miningBooster = 1;
    private bool _isBreaking = false;
    private bool _isBroken = false;
    private HintObject _hint;
    private Quaternion _initialBoneRotation;
    private AudioSource _audio;
    private Inventory _inventory;

    private void Start()
    {
        _hint = GetComponent<HintObject>();
        _audio = GetComponent<AudioSource>();
        _inventory = FindObjectOfType<Inventory>();
        if (boneEffect != null)
            _initialBoneRotation = boneEffect.transform.rotation;
        _hint.OnHintShow += () =>
        {
            activeResource = this;
        };
        slider.maxValue = breakTime;
        slider.value = breakTime;
        slider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isBroken) return;
        if (toolToHarvest != null)
        {
            _hint.IsDisabled = _inventory.ActiveTool != toolToHarvest;
        }
        if ((_isBreaking && activeResource != this) || _hint.IsDisabled)
            StopBreaking();
        if (!_isBreaking && boneEffect != null)
            boneEffect.rotation = _initialBoneRotation;
    }

    public void Break()
    {
        if (!_hint.PlayerNear || _isBroken || _isBreaking) return;
        _isBreaking = true;
        slider.gameObject.SetActive(true);
        StartCoroutine(StartBreaking());
    }

    public void StopBreaking()
    {
        LeanTween.cancel(gameObject);
        StopAllCoroutines();
        _isBreaking = false;
        slider.value = breakTime;
        slider.gameObject.SetActive(false);
    }

    private IEnumerator StartBreaking()
    {
        var time = breakTime * miningBooster;
        slider.maxValue = time;
        while (time > 1)
        {
            time--;
            slider.value = time;
            if (boneEffect != null)
                LeanTween.rotateLocal(boneEffect.gameObject, Vector3.forward * Random.Range(-20f, 20f), .2f).setEaseShake();
            if (chopClips != null)
            {
                _audio.clip = chopClips[Random.Range(0, chopClips.Length)];
                _audio.Play();
            }
            yield return new WaitForSeconds(1);
        }
        if (brokenSprite == null)
            Destroy(gameObject);
        else
        {
            _isBroken = true;
            GetComponentInChildren<SpriteRenderer>().sprite = brokenSprite;
            GetComponent<HintObject>().enabled = false;
        }


        slider.gameObject.SetActive(false);
        if (drop != null)
        {
            var go = Instantiate(itemprefab, transform.position, Quaternion.identity);
            go.GetComponent<Item>().Init(drop, Random.onUnitSphere);
        }
    }
}
