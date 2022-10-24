using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float moveSpeed;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var movement = new Vector2(horizontal, vertical);

        _rb.MovePosition(_rb.position + movement * moveSpeed * Time.deltaTime);
        if (Input.GetButtonDown("Fire1"))
        {
            Resource.activeResource?.Break();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            Resource.activeResource?.StopBreaking();
        }
    }
}
