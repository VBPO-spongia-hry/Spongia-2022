using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Tower
{
    public class Tower : MonoBehaviour
    {
        public static bool TowerActive;
        public static event Action<int> OnTowerUpdated;
        public int level;
        public float levelHeight;
        public FloorSO[] floors;
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject map;
        [SerializeField] private PlayableDirector cutsceneEnterController;
        [SerializeField] private PlayableDirector cutsceneExitController;
        [SerializeField] private AudioClip upgradeClip;
        public static Tower Instance;
        private TowerCamera _towerCamera => GetComponentInChildren<TowerCamera>();
        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
            // Hide();  
        }

        private void Start()
        {
            Crafting.CraftingRecipe[] unlockRecipes = new Crafting.CraftingRecipe[floors.Length];
            for (int i = 0; i < floors.Length; i++)
            {
                unlockRecipes[i] = floors[i].unlockRecipe;
            }
            GetComponentInChildren<Crafting.Crafter>().InitFloorCrafter(unlockRecipes);
        }

        void Update()
        {
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
            OnTowerUpdated?.Invoke(level);
            var floor = Instantiate(floorPrefab, new Vector3(0, (level - 1) * levelHeight, 0), Quaternion.identity, transform).GetComponent<Floor>();
            _towerCamera.Show(level - 1);
            floor.transform.localScale = Vector3.zero;
            yield return new WaitForSeconds(_towerCamera.animationTime);
            LeanAudio.play(upgradeClip);

            floor.Init(level - 1, floors[level - 2]);
        }

        public static void Hide()
        {
            TowerActive = false;
            FindObjectOfType<CutsceneController>().Skip();
            Instance.cutsceneExitController.Play();
        }

        public static void Show()
        {
            TowerActive = true;
            FindObjectOfType<CutsceneController>().Skip();
            Instance.cutsceneEnterController.Play();
        }

        public static void UnlockNextFloor()
        {
            Instance.StartCoroutine(Instance.NextFloorUnlocked());
        }
    }
}
