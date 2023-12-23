using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidBody;

    private Animator _animator;

    private Transform _transform;

    [SerializeField] private float _movementSpeed = 5f; // Adjust this value to change speed

    private bool _isStopped;

    public static Action OnTest;

    public float middleXPos = 0;

    [SerializeField] private float _movementSmoothness;

    private void Awake()
    {
        _transform = transform;
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

        //InvokeRepeating(nameof(CheckLoseCondition), 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        if (_isStopped) return;

        float moveXDistance = middleXPos - _rigidBody.position.x;
        float xMoveAmount = Mathf.Min(Mathf.Abs(moveXDistance), _movementSmoothness * Time.deltaTime);
        Vector3 xMove = new(Mathf.Sign(moveXDistance) * xMoveAmount, 0, 0);

        Vector3 zMove = _movementSpeed * Time.deltaTime * _transform.forward;
        _rigidBody.MovePosition(_rigidBody.position + xMove + zMove);
    }

    private void CheckLoseCondition()
    {
        if (_transform.position.y < -2f) return;

        Debug.Log("LOSE");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            _animator.Play("Dance");
            _isStopped = true;
        }
    }
}
