using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    [HideInInspector] public List<Block> GridBlocks;

    [SerializeField] private GameObject _block;

    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    private Transform _gridParent, _blocksParent;


    [SerializeField, Range(2, 9)] private int _size = 2;

    public int Size
    {
        get { return _size; }
        private set { _size = value; }
    }

    private float _gridWidth;


    private void Awake()
    {
        StartGrid();
    }

    private void StartGrid()
    {
        _camera = Camera.main;
        _gridParent = transform.GetChild(0);
        _blocksParent = transform.GetChild(1);
        _spriteRenderer = _gridParent.GetComponent<SpriteRenderer>();

        SetGridBackground();
    }

    private void SetGridBackground()
    {
        float cameraWidth = _camera.orthographicSize * 2.0f * _camera.aspect;
        float currentWidth = _spriteRenderer.sprite.bounds.size.x;

        _gridWidth = cameraWidth / currentWidth;

        _gridParent.localScale = new Vector2(_gridWidth, _gridWidth);
        _gridParent.position = new Vector2(_gridParent.position.x, _camera.orthographicSize - (_gridWidth / 2f));

        ClearBlocks();
        SpawnBlocks();
    }

    private void SpawnBlocks()
    {
        Transform blockTransform;
        Vector2 blockSize = Vector2.one * (_gridWidth / _size);
        Vector2 startOffset = blockSize.x / 2f * (_size - 1) * Vector2.one;
        Vector2 startPos = (Vector2)_gridParent.position - startOffset;
        GridBlocks = new List<Block>();

        for (int h = 0; h < _size; h++)
        {
            for (int w = 0; w < _size; w++)
            {
                startOffset = startPos + (new Vector2(w, h) * blockSize.x);
                blockTransform = Instantiate(_block, startOffset, Quaternion.identity, _blocksParent).transform;
                blockTransform.localScale = blockSize;
                blockTransform.name = $"Block {w}_{h}";
                GridBlocks.Add(blockTransform.GetComponent<Block>());
            }
        }
    }

    private void ClearBlocks()
    {
        for (int i = _blocksParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(_blocksParent.GetChild(i).gameObject);
        GridBlocks.Clear();
    }

    private void OnEnable()
    {
        UIManager.OnSizeChange += SizeChanged;
    }

    private void OnDisable()
    {
        UIManager.OnSizeChange -= SizeChanged;
    }

    private void SizeChanged(int newSize)
    {
        if (newSize == _size)
        {
            Debug.Log("Same Size");
            return;
        }

        _size = newSize;

        if (_gridParent == null)
        {
            StartGrid();
        }
        else
        {
            ClearBlocks();
            SpawnBlocks();
        }
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += DelayOnValidate;
    }

    private void DelayOnValidate()
    {
        if (this == null || _block == null)
            return;

        StartGrid();
    }

#endif
}
