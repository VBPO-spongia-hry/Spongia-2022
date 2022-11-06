using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(EdgeCollider2D), typeof(Light2D))]
public class MapArea : MonoBehaviour
{
    public int unlockLevel;
    private Light2D _light;
    private EdgeCollider2D _collider;

    // Start is called before the first frame update
    void Awake()
    {
        _light = GetComponent<Light2D>();
        _light.enabled = false;
        _collider = GetComponent<EdgeCollider2D>();
        var points = new Vector2[_light.shapePath.Length + 1];
        for (int i = 0; i < _light.shapePath.Length; i++)
        {
            points[i] = new Vector2(_light.shapePath[i].x, _light.shapePath[i].y);
        }
        points[points.Length - 1] = points[0];
        _collider.points = points;
    }

    private void OnEnable()
    {
        if (_light.enabled) return;
        if (Tower.Tower.Instance == null)
        {
            if (unlockLevel == 0)
            {
                Unlock();
                return;
            }
            return;
        }
        if (Tower.Tower.Instance.level >= unlockLevel && _collider.enabled)
            Unlock();
    }

    private void Unlock()
    {
        _light.enabled = true;
        _collider.enabled = false;
    }
}
