using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSwitcher : MonoBehaviour
{
    private bool _isPlayerNear;
    private void Update()
    {
        if (_isPlayerNear && Input.GetButtonDown("Interact"))
        {
            ShowTower();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            _isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            _isPlayerNear = false;
    }

    private void ShowTower()
    {
        Tower.Tower.Show();
    }
}
