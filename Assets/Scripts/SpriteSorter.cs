using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField] private float offset;
    private Transform _player;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>().transform;
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_player.position.y < transform.position.y + offset)
            _renderer.sortingOrder = -1;
        else
            _renderer.sortingOrder = 1;
    }
}