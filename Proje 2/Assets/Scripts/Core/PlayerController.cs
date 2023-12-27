using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidBody;
    private Animator _animator;


    public static Action OnGameLose;

    public static Action<Vector3> OnGameWin;

    [Tooltip("Speed of Forward movement, Moves constantly forward at said speed")]
    [SerializeField, Range(0, 5f)] private float _forwardSpeed = 1f;

    [Tooltip("Speed of Left-Right movement, Moves to the center of the next block")]
    [SerializeField, Range(0, 5f)] private float _strafeSpeed = 2f;

    private readonly float _fallThreshold = -5f;

    private Vector3 xMovePosition, zMovePosition;

    private float _middleBlockXPos, _xMoveAmount;

    private bool _isStopped;


    private void Awake()
    {
        _transform = transform;
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

        InvokeRepeating(nameof(CheckLoseCondition), 0f, 0.5f);
    }

    private void CheckLoseCondition()
    {
        if (_isStopped) return;

        if (_transform.position.y < _fallThreshold)
        {
            _isStopped = true;
            OnGameLose?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (!_isStopped)
            Movement();
    }

    private void Movement()
    {
        _xMoveAmount = Mathf.Min(Mathf.Abs(_middleBlockXPos - _rigidBody.position.x), _strafeSpeed * Time.fixedDeltaTime);

        xMovePosition = new(Mathf.Sign(_middleBlockXPos - _rigidBody.position.x) * _xMoveAmount, 0, 0);
        zMovePosition = _forwardSpeed * Time.fixedDeltaTime * _transform.forward.normalized;

        _rigidBody.MovePosition(_rigidBody.position + xMovePosition + zMovePosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            _isStopped = true;
            _animator.Play("Dance");
            OnGameWin?.Invoke(_transform.position);
        }
    }

    private void OnEnable()
    {
        BlockController.MiddleBlockChanged += UpdateMiddleBlock;
        GameManager.OnGameContinue += RestartPlayer;
    }

    private void OnDisable()
    {
        BlockController.MiddleBlockChanged -= UpdateMiddleBlock;
        GameManager.OnGameContinue -= RestartPlayer;
    }

    private void UpdateMiddleBlock(float newPos)
    {
        _middleBlockXPos = newPos;
    }

    private void RestartPlayer()
    {
        _isStopped = false;
        _animator.Play("Run");
    }
}
