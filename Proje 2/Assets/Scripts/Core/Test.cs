
using UnityEngine;

public class Test : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private GameObject _fallingPiece;

    private MeshRenderer _referenceMesh;

    private BlockStack _blockStack;

    private Material _blockMaterial;

    [SerializeField] private float tolerans;

    public Transform previousBlock;

    public bool _isForward;
    public float speed, limit;

    public int perfectScore = 0;
    private PlayerController _playerController;

    private void Awake()
    {
        _transform = transform;
        _blockStack = GetComponent<BlockStack>();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        InputManager.OnValidInput += TestDivide;
    }

    private void OnDisable()
    {
        InputManager.OnValidInput -= TestDivide;
    }

    private void LateUpdate()
    {
        var position = transform.position;
        var direction = _isForward ? 1 : -1;
        var move = speed * Time.deltaTime * direction;

        position.x += move;

        if (position.x < previousBlock.position.x - limit || position.x > previousBlock.position.x + limit)
        {
            position.x = Mathf.Clamp(position.x, -limit, limit);
            _isForward = !_isForward;
        }

        transform.position = position;
    }

    [ContextMenu("Divide")]
    public void TestDivide()
    {
        _referenceMesh = _transform.GetComponent<MeshRenderer>();
        float test = previousBlock.position.x - _transform.position.x;

        if (Mathf.Abs(test) > _transform.localScale.x) return;

        if (Mathf.Abs(test) < tolerans)
        {
            perfectScore++;
            AudioManager.singleton.PlayAudio(perfectScore);
            PerfectDivision();
            //_blockStack.PerfectDivision();
        }
        else
        {
            perfectScore = 0;
            AudioManager.singleton.PlayAudio(0);
            DivideObject(test);
            //_blockStack.DivideObject(test);
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

        if (!Application.isPlaying) return;

        standingPiece.GetComponent<MeshRenderer>().material = _referenceMesh.material;
    }

    private void DivideObject(float value)
    {
        Transform fallingPiece = Instantiate(_fallingPiece, transform.position, Quaternion.identity).transform;
        Transform standingPiece = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        standingPiece.name = "Standing Piece";

        int direction = (value > 0) ? 1 : -1;

        SetPositionAndScale(fallingPiece, Mathf.Abs(value), -direction);
        SetPositionAndScale(standingPiece, _transform.localScale.x - Mathf.Abs(value), direction);

        previousBlock = standingPiece;
        _transform.localScale = standingPiece.localScale;
        _transform.position = standingPiece.position;
        _transform.position += Vector3.forward * _transform.localScale.z;

        _playerController.middleXPos = previousBlock.position.x;


        limit = _transform.localScale.x * 1.6f;
        direction = Random.Range(0, 2) == 0 ? 1 : -1;
        _isForward = direction == 1;
        _transform.position += -direction * limit * Vector3.right;

        if (!Application.isPlaying) return;

        fallingPiece.GetComponent<MeshRenderer>().material = _referenceMesh.material;
        standingPiece.GetComponent<MeshRenderer>().material = _referenceMesh.material;
        Destroy(fallingPiece.gameObject, 3f);
    }

}
