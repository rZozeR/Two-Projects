using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private Camera _camera;
    private PlayerInput _playerInput;
    private InputAction _actionClick, _actionPosition;

    public delegate void InputEventHandler(Block block);
    public static event InputEventHandler OnValidInput;

    [Tooltip("Interactable object's Layer")]
    [SerializeField] private LayerMask _interactableLayer;

    [Tooltip("Cooldown between each player input")]
    [SerializeField, Range(0f, 2f)] private float _inputCooldownTimer = 0.5f;

    private bool _inputCooldown;

    private void Awake()
    {
        _camera = Camera.main;
        _playerInput = GetComponent<PlayerInput>();

        _actionClick = _playerInput.actions["Click"];
        _actionPosition = _playerInput.actions["Click Position"];
    }

    private void OnEnable()
    {
        _actionClick.started += InputStart;
    }

    private void OnDisable()
    {
        _actionClick.started -= InputStart;
    }

    //Event method that detects input and invokes an event if an object is found at the input position.
    private void InputStart(InputAction.CallbackContext context)
    {
        if (_inputCooldown) return;
        _ = StartCoroutine(ResetCooldown());

        Vector2 inputPosition = _camera.ScreenToWorldPoint(_actionPosition.ReadValue<Vector2>());
        RaycastHit2D rayOut = Physics2D.Raycast(inputPosition, Vector2.zero, float.PositiveInfinity, _interactableLayer);

        if (!rayOut.collider) return;
        Block _clickedBlock = rayOut.collider.gameObject.GetComponent<Block>();

        if (_clickedBlock == null) return;
        _clickedBlock.OpenClick(!_clickedBlock.Clicked);

        OnValidInput?.Invoke(_clickedBlock);
    }

    private IEnumerator ResetCooldown()
    {
        _inputCooldown = true;
        yield return new WaitForSeconds(_inputCooldownTimer);
        _inputCooldown = false;
    }
}
