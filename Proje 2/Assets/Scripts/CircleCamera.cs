using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CircleCamera : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private float _rotationSpeed;

    private void Awake()
    {
        _transform = transform;
        _transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void OnEnable()
    {
        PlayerController.OnTest += Testo;
    }
    private void OnDisable()
    {
        PlayerController.OnTest -= Testo;
    }

    private void LateUpdate()
    {
        _transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void Testo()
    {

    }
}
