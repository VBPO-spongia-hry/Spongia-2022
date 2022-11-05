using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCutscene : MonoBehaviour
{
    [SerializeField] private int levelToTrigger;
    [SerializeField] private string cutsceneName;
    private void Start()
    {
        Tower.Tower.OnTowerUpdated += OnTowerUpgraded;
    }

    private void OnTowerUpgraded(int level)
    {
        if (level == levelToTrigger)
            StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(1);
        Tower.Tower.Hide();
        yield return new WaitForSeconds(1);
        CutsceneController.PlayCutscene(cutsceneName);
    }
}
