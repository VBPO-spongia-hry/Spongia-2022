using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTowerLevel : MonoBehaviour
{
    [SerializeField] private int levelToDestroy;

    private void Start()
    {
        Tower.Tower.OnTowerUpdated += OnTowerUpdate;
    }

    private void OnTowerUpdate(int level)
    {
        if (level >= levelToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
