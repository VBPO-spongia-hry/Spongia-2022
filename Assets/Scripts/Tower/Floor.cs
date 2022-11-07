using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Video;

namespace Tower
{
    public class Floor : MonoBehaviour
    {
        [SerializeField] private float animationTime = 1f;
        [SerializeField] private GameObject itemUIPrefab;
        [SerializeField] private Transform itemsContainer;
        [SerializeField] private TextMeshProUGUI selectedItem;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject dirtEffect;
        private float floorHeight => Tower.Instance.levelHeight;
        [SerializeField] private FloorSO floorSO;
        [SerializeField] private GameObject[] buildings;
        public GameObject rockRenderer;
        public GameObject woodRenderer;
        private void Start()
        {
            selectedItem.gameObject.SetActive(false);
        }

        public void Init(int level, FloorSO floor)
        {
            if (floor.isWood)
            {
                woodRenderer.SetActive(true);
                rockRenderer.SetActive(false);
            }
            else
            {
                rockRenderer.SetActive(true);
                woodRenderer.SetActive(false);
            }
            transform.localPosition = new Vector3(0, floor.level * floorHeight);
            Debug.Log(transform.localPosition);
            Destroy(dirtEffect, 3);
            nameText.text = floor.floorName;
            LeanTween.scale(gameObject, Vector3.one, animationTime).setEaseInOutCubic();
            LeanTween.moveLocalY(gameObject, transform.localPosition.y + floorHeight, animationTime).setEaseSpring().setOnComplete(() =>
            {
                dirtEffect.SetActive(true);
            });
            GetComponentInChildren<Canvas>().worldCamera = Camera.main;
            floorSO = floor;
            InitItems();
            foreach (var building in buildings)
            {
                if (floor.floorName == building.name)
                {
                    building.SetActive(true);
                    if (building.TryGetComponent<VideoPlayer>(out var video))
                    {
                        video.Play();
                        video.errorReceived += (vid, err) =>
                        {
                            Debug.Log(err);
                        };
                    }
                }
            }
            GetComponentInChildren<Crafting.Crafter>().InitCrafter(floor.recipe);
        }

        private void InitItems()
        {
            if (floorSO.items.Length == 0)
                Destroy(itemsContainer.gameObject);
            for (int i = 0; i < floorSO.items.Length; i++)
            {
                var btn = Instantiate(itemUIPrefab, itemsContainer).GetComponentInChildren<Button>();
                var idx = i;
                btn.onClick.AddListener(() => selectItem(idx));
                btn.GetComponentInChildren<TextMeshProUGUI>().text = floorSO.items[i].boosterName;
                LeanTween.scale(btn.gameObject, 1.17f * Vector3.one, 1.5f).setEaseInOutSine().setLoopPingPong();
                LeanTween.rotateZ(btn.transform.parent.Find("LightEffect").gameObject, 180f, 20f).setLoopClamp();
                LeanTween.scale(btn.transform.parent.Find("LightEffect").gameObject, 1.2f * Vector3.one, 20f).setLoopPingPong();
            }
        }

        private void selectItem(int i)
        {
            selectedItem.text = floorSO.items[i].boosterName;
            floorSO.items[i].Activate();
            selectedItem.gameObject.SetActive(true);
            LeanTween.alphaCanvas(itemsContainer.GetComponent<CanvasGroup>(), 0, .5f).setOnComplete(() =>
            {
                Destroy(itemsContainer.gameObject);
            });
        }

        public void Upgrade()
        {
            rockRenderer.SetActive(true);
            woodRenderer.SetActive(false);
        }
    }
}
