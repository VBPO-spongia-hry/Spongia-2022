using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private new CinemachineVirtualCamera camera;
    [SerializeField] private float moveSpeed;
    internal static float speedBoost = 1;
    internal static float orthoSize = 10;
    public Animator animator;


    private void Start()
    {
        orthoSize = camera.m_Lens.OrthographicSize;
        _rb = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 300;
    }


    private void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var movement = new Vector2(horizontal, vertical);
        animator.Play("Movement");
        animator.SetFloat("SpeedX", movement.x);
        animator.SetFloat("SpeedY", movement.y);
        _rb.MovePosition(_rb.position + movement * moveSpeed * speedBoost * Time.deltaTime);
        if (Input.GetButtonDown("Fire1"))
        {
            Resource.activeResource?.Break();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            Resource.activeResource?.StopBreaking();
        }

        if (Resource.activeResource)
        {
            var dir = transform.position - Resource.activeResource.transform.position;
            animator.SetFloat("DirX", -dir.normalized.x);
            animator.SetFloat("DirY", -dir.normalized.y);
        }
        camera.m_Lens.OrthographicSize = orthoSize;
    }
}
