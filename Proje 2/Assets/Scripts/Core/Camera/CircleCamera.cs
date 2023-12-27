using UnityEngine;

public class CircleCamera : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private float _rotationSpeed = -30;

    private bool _isStarted;

    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        if (_isStarted)
            _transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnEnable()
    {
        PlayerController.OnGameWin += EnableCamera;
        GameManager.OnGameContinue += DisableCamera;
    }
    private void OnDisable()
    {
        PlayerController.OnGameWin -= EnableCamera;
        GameManager.OnGameContinue -= DisableCamera;
    }

    private void EnableCamera(Vector3 position)
    {
        _isStarted = true;
        _transform.position = position;
    }

    private void DisableCamera()
    {
        _isStarted = false;
    }
}
