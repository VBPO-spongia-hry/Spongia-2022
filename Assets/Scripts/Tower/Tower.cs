using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tower
{
    public class Tower : MonoBehaviour
    {
        public int level;
        public float levelHeight;
        public FloorSO[] floors;
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject map;
        public static Tower Instance;
        private TowerCamera _towerCamera => GetComponentInChildren<TowerCamera>();
        private void Awake()
        {
            Instance = this;
            Hide();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                StartCoroutine(NextFloorUnlocked());
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                Hide();
        }

        public IEnumerator NextFloorUnlocked()
        {
            level++;
            if (level > floors.Length)
            {
                level--;
                yield break;
            }

            var floor = Instantiate(floorPrefab, new Vector3(0, (level - 1) * levelHeight, 0), Quaternion.identity, transform).GetComponent<Floor>();
            _towerCamera.Show(level - 1);
            floor.transform.localScale = Vector3.zero;
            yield return new WaitForSeconds(_towerCamera.animationTime);

            floor.Init(level - 1, floors[level - 1]);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            map.SetActive(true);
        }

        public static void Show()
        {
            Instance.gameObject.SetActive(true);
            Instance.map.SetActive(false);
        }
    }
}
