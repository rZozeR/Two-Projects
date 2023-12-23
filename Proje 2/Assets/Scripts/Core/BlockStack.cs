using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStack : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private GameObject _fallingPiece;

    private MeshRenderer _referenceMesh, _previousMesh;

    [SerializeField] private Transform _previousBlock;

    public bool _isForward;
    public float limit;

    private PlayerController _playerController;

    private void Awake()
    {
        _transform = transform;
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _referenceMesh = GetComponent<MeshRenderer>();
        _previousMesh = _previousBlock.GetComponent<MeshRenderer>();
    }

    public void PerfectDivision()
    {
        Transform standingPiece = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        standingPiece.name = "Standing Piece";

        Vector3 position = _transform.position;
        position.x = _previousBlock.position.x;

        standingPiece.position = position;
        standingPiece.localScale = _transform.localScale;

        _transform.localScale = standingPiece.localScale;
        _transform.position = standingPiece.position;
        _transform.position += Vector3.forward * _transform.localScale.z;

        standingPiece.GetComponent<MeshRenderer>().material = _previousMesh.material;
    }

    public void DivideObject(float value)
    {
        Transform fallingPiece = Instantiate(_fallingPiece, transform.position, Quaternion.identity).transform;
        Transform standingPiece = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        standingPiece.name = "Standing Piece";

        int direction = (value > 0) ? 1 : -1;

        SetPositionAndScale(fallingPiece, Mathf.Abs(value), -direction);
        SetPositionAndScale(standingPiece, _transform.localScale.x - Mathf.Abs(value), direction);

        _previousBlock = standingPiece;
        _transform.localScale = standingPiece.localScale;
        _transform.position = standingPiece.position;
        _transform.position += Vector3.forward * _transform.localScale.z;

        _playerController.middleXPos = _previousBlock.position.x;

        //limit = _reference.localScale.x * 1.6f;
        //direction = Random.Range(0, 2) == 0 ? 1 : -1;
        //_isForward = direction == 1;
        //_reference.position += -direction * limit * Vector3.right;

        fallingPiece.GetComponent<MeshRenderer>().material = _previousMesh.material;
        standingPiece.GetComponent<MeshRenderer>().material = _previousMesh.material;
        Destroy(fallingPiece.gameObject, 3f);
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
}
