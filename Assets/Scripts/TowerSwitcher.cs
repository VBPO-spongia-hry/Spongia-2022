using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HintObject))]
public class TowerSwitcher : MonoBehaviour
{
    private HintObject _hint;
    private void Start()
    {
        _hint = GetComponent<HintObject>();
    }
    private void Update()
    {
        if (_hint.PlayerNear && Input.GetButtonDown("Interact"))
        {
            ShowTower();
        }
    }

    private void ShowTower()
    {
        Tower.Tower.Show();
    }
}
