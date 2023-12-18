using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidBody;

    private Transform _transform;

    [SerializeField] private float _movementSpeed = 5f; // Adjust this value to change speed

    private void Awake()
    {
        _transform = transform;
        _rigidBody = GetComponent<Rigidbody>();

        InvokeRepeating(nameof(CheckLoseCondition), 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        _rigidBody.MovePosition(_rigidBody.position + _transform.forward * _movementSpeed * Time.deltaTime);
    }

    private void CheckLoseCondition()
    {
        if (_transform.position.y < 5f) return;

        Debug.Log("LOSE");
    }
}
