using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _finish, _blockController, _platform;

    [SerializeField] private GameObject _winPanel, _losePanel;
    
    [SerializeField] private CinemachineVirtualCamera _mainCamera, _rotatingCamera;


    public static Action OnGameContinue;


    [SerializeField, Range(1, 20)] private int _platformCount = 1;

    private Vector3 _lastPosition;

    private float _zDistance;


    private void Awake()
    {
        _zDistance = _platform.transform.localScale.z;
        AddPlatforms(_platform.transform.position + (Vector3.forward * _zDistance));
    }

    private void OnEnable()
    {
        PlayerController.OnGameWin += GameWon;
        PlayerController.OnGameLose += GameLose;
    }

    private void OnDisable()
    {
        PlayerController.OnGameWin -= GameWon;
        PlayerController.OnGameLose -= GameLose;
    }

    private void AddPlatforms(Vector3 spawnPos)
    {
        Transform previous = Instantiate(_platform, spawnPos, Quaternion.identity).transform;

        spawnPos.z += _zDistance;
        BlockController blockController = Instantiate(_blockController, spawnPos, Quaternion.identity).GetComponent<BlockController>();
        blockController.previousBlock = previous;
        blockController.maxDistance = _platformCount;

        spawnPos.z += _zDistance * _platformCount;
        spawnPos.z += _finish.transform.position.z;
        spawnPos.y -= _finish.transform.position.y;

        _lastPosition = Instantiate(_finish, spawnPos, Quaternion.identity).transform.position;
    }

    public void ContinueGame()
    {
        _lastPosition.z += _finish.transform.position.z;
        _lastPosition.y += _finish.transform.position.y;
        _lastPosition.z += _zDistance;

        _winPanel.SetActive(false);

        _mainCamera.Priority = 1;
        _rotatingCamera.Priority = 0;

        AddPlatforms(_lastPosition);
        OnGameContinue?.Invoke();
    }

    private void GameWon(Vector3 empty)
    {
        _winPanel.SetActive(true);
        _mainCamera.Priority = 0;
        _rotatingCamera.Priority = 1;
    }

    private void GameLose()
    {
        Time.timeScale = 0;
        _losePanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
