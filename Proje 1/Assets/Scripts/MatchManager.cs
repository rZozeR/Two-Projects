using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    private GridManager _gridManager;

    private List<Block> _blocks;


    public delegate void MatchEventHandler(int matchesCount);
    public static event MatchEventHandler OnValidMatch;


    private const int MATCH_COUNT = 3;

    private int _size, _matchCount = 0;


    private void Awake()
    {
        _gridManager = GetComponent<GridManager>();
        _size = _gridManager.Size;
        _blocks = _gridManager.GridBlocks;
    }

    private void OnEnable()
    {
        InputManager.OnValidInput += CheckMatches;
    }

    private void OnDisable()
    {
        InputManager.OnValidInput -= CheckMatches;
    }

    private void CheckMatches(Block block)
    {
        _size = _gridManager.Size;
        _blocks = _gridManager.GridBlocks;

        int indexSearch = _blocks.IndexOf(block);
        List<int> matches = new() { indexSearch };
        int row, col;

        void CheckNeighbor(int index)
        {
            if (!matches.Contains(index) && _blocks[index].Clicked)
                matches.Add(index);
        }

        for (int i = 0; i < matches.Count; i++)
        {
            row = matches[i] / _size;
            col = matches[i] % _size;

            if (col > 0)
                CheckNeighbor(matches[i] - 1);

            if (col < _size - 1)
                CheckNeighbor(matches[i] + 1);

            if (row > 0)
                CheckNeighbor(matches[i] - _size);

            if (row < _size - 1)
                CheckNeighbor(matches[i] + _size);
        }

        if (matches.Count >= MATCH_COUNT)
            ValidMatches(matches);
    }

    private void ValidMatches(List<int> matchIndexes)
    {
        for (int i = 0; i < matchIndexes.Count; i++)
            _blocks[matchIndexes[i]].OpenClick(false);

        _matchCount++;
        OnValidMatch?.Invoke(_matchCount);
    }
}
