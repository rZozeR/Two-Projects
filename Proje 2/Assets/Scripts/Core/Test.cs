using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform _reference;

    [SerializeField] private GameObject _fallingPiece;

    private MeshRenderer _referenceMesh;

    public Transform previousBlock;


    [ContextMenu("Divide")]
    public void TestDivide()
    {
        _referenceMesh = _reference.GetComponent<MeshRenderer>();
        float test = previousBlock.position.x - _reference.position.x;

        if (Mathf.Abs(test) > _reference.localScale.x)
            return;

        DivideObject(test);
    }

    private void SetPositionAndScale(Transform piece, float division, int direction)
    {
        Vector3 fallingSize = _reference.localScale;
        fallingSize.x = division;
        piece.localScale = fallingSize;

        Vector3 fallingPosition = _reference.position;
        fallingPosition.x += _referenceMesh.bounds.extents.x * direction;
        fallingPosition.x += (fallingSize.x / 2) * -direction;
        piece.position = fallingPosition;
    }

    private void DivideObject(float value)
    {
        Transform fallingPiece = Instantiate(_fallingPiece, transform.position, Quaternion.identity).transform;
        Transform standingPiece = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        standingPiece.name = "Standing Piece";

        int direction = (value > 0) ? 1 : -1;

        SetPositionAndScale(fallingPiece, Mathf.Abs(value), -direction);
        SetPositionAndScale(standingPiece, _reference.localScale.x - Mathf.Abs(value), direction);

        previousBlock = standingPiece;
        _reference.localScale = standingPiece.localScale;
        _reference.position = standingPiece.position;
        _reference.position += Vector3.forward * _reference.localScale.z;

        if (!Application.isPlaying) return;

        fallingPiece.GetComponent<MeshRenderer>().material = _referenceMesh.material;
        standingPiece.GetComponent<MeshRenderer>().material = _referenceMesh.material;
        Destroy(fallingPiece.gameObject, 3f);
    }

}
