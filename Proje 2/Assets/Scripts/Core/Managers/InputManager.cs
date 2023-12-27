using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _actionClick;

    public delegate void InputEventHandler();
    public static event InputEventHandler OnValidInput;

    //[Tooltip("Interactable object's Layer")]
    //[SerializeField] private LayerMask _interactableLayer;

    [Tooltip("Cooldown between each player input")]
    [SerializeField, Range(0f, 2f)] private float _inputCooldownTimer = 0.5f;

    private bool _inputCooldown;


    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _actionClick = _playerInput.actions["Click"];
        //_actionPosition = _playerInput.actions["Click Position"];
    }

    private void OnEnable()
    {
        _actionClick.started += InputStart;
    }

    private void OnDisable()
    {
        _actionClick.started -= InputStart;
    }

    private void InputStart(InputAction.CallbackContext _context)
    {
        if (_inputCooldown) return;

        _ = StartCoroutine(ResetCooldown());

        OnValidInput?.Invoke();
    }

    private IEnumerator ResetCooldown()
    {
        _inputCooldown = true;
        yield return new WaitForSeconds(_inputCooldownTimer);
        _inputCooldown = false;
    }
}
