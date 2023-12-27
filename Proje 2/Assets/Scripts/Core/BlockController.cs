using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockController : MonoBehaviour
{
    [SerializeField] private GameObject _fallingPiece;

    public Transform previousBlock;

    private Transform _transform;

    private MeshRenderer _referenceMesh, _previousMesh;


    public static event Action<float> MiddleBlockChanged;


    [HideInInspector] public int maxDistance = 100;

    private readonly float _limitOffset = 1.25f;
    private readonly float _speedMultiplier = 1.2f;
    private readonly float _scaleTolerance = 0.1f;

    private float _moveLimit, _moveSpeed, _tolerance;

    private int _direction = 1, _perfectScore = 0, _distancePlaced = 0;


    private void Awake()
    {
        _transform = transform;
        _referenceMesh = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _previousMesh = previousBlock.GetComponent<MeshRenderer>();

        _moveLimit = _transform.localScale.x * _limitOffset;
        _moveSpeed = transform.localScale.x * _speedMultiplier;
        _tolerance = _transform.localScale.x / 10f;

        _direction = Random.Range(0, 2) == 0 ? 1 : -1;
        _transform.position += -_direction * _moveLimit * Vector3.right;
    }

    private void LateUpdate()
    {
        Movement();
    }

    private void OnEnable()
    {
        InputManager.OnValidInput += DivideBlock;
    }

    private void OnDisable()
    {
        InputManager.OnValidInput -= DivideBlock;
    }

    private void Movement()
    {
        if (_transform.position.x > previousBlock.position.x + _moveLimit)
            _direction = -1;
        else if (_transform.position.x < previousBlock.position.x - _moveLimit)
            _direction = 1;

        _transform.Translate(_direction * _moveSpeed * Time.deltaTime * Vector3.right);
    }

    private void DivideBlock()
    {
        float distance = previousBlock.position.x - _transform.position.x;

        if (Mathf.Abs(distance) > _transform.localScale.x) return;

        if (Mathf.Abs(distance) <= _tolerance)
        {
            _perfectScore++;
            AudioManager.singleton.PlayAudio(_perfectScore);
            PerfectDivision();
        }
        else
        {
            _perfectScore = 0;
            AudioManager.singleton.PlayAudio(0);
            DivideObject(distance);
        }

        _distancePlaced++;
        if (_distancePlaced >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void SetPositionAndScale(Transform piece, float division, int direction)
    {
        Vector3 fallingSize = _transform.localScale;
        fallingSize.x = division;
        piece.localScale = fallingSize;

        Vector3 fallingPosition = _transform.position;
        fallingPosition.x += _referenceMesh.bounds.extents.x * direction;
        fallingPosition.x += (fallingSize.x / 2) * -direction;
        piece.position = fallingPosition;
    }

    private void PerfectDivision()
    {
        Transform standingPiece = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        standingPiece.name = "Standing Piece";

        Vector3 position = _transform.position;
        position.x = previousBlock.position.x;

        standingPiece.position = position;
        standingPiece.localScale = _transform.localScale;

        _transform.localScale = standingPiece.localScale;
        _transform.position = standingPiece.position;
        _transform.position += Vector3.forward * _transform.localScale.z;

        _direction = Random.Range(0, 2) == 0 ? 1 : -1;
        _transform.position += -_direction * _moveLimit * Vector3.right;

        standingPiece.GetComponent<MeshRenderer>().material = _previousMesh.material;
    }

    private void DivideObject(float value)
    {
        Transform fallingPiece = Instantiate(_fallingPiece, transform.position, Quaternion.identity).transform;
        Transform standingPiece = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        standingPiece.name = "Standing Piece";

        _direction = (value > 0) ? 1 : -1;

        SetPositionAndScale(fallingPiece, Mathf.Abs(value), -_direction);
        SetPositionAndScale(standingPiece, _transform.localScale.x - Mathf.Abs(value), _direction);

        previousBlock = standingPiece;
        AdjustObjectAndVariables(fallingPiece, standingPiece);
    }

    private void AdjustObjectAndVariables(Transform fallingPiece, Transform standingPiece)
    {
        _transform.localScale = previousBlock.localScale;
        _transform.position = previousBlock.position;
        _transform.position += Vector3.forward * _transform.localScale.z;

        MiddleBlockChanged?.Invoke(previousBlock.position.x);

        _moveLimit = transform.localScale.x * _limitOffset;
        _moveSpeed = transform.localScale.x * _speedMultiplier;
        _tolerance = _transform.localScale.x / 10f;

        _direction = Random.Range(0, 2) == 0 ? 1 : -1;
        _transform.position += -_direction * _moveLimit * Vector3.right;

        fallingPiece.GetComponent<MeshRenderer>().material = _previousMesh.material;
        standingPiece.GetComponent<MeshRenderer>().material = _previousMesh.material;
        Destroy(fallingPiece.gameObject, 3f);

        if (_transform.localScale.x <= _scaleTolerance)
        {
            Destroy(gameObject);
        }
    }
}
