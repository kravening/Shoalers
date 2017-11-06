using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int _startWidth;
    [SerializeField] private int _startHeight;

    private GameObject[,] _grid;

    public GameObject[,] Array
    {
        get { return _grid; }
    }
    public int Width
    {
        get { return _grid.GetLength(0); }
    }
    public int Height
    {
        get { return _grid.GetLength(1); }
    }

    private void Awake()
    {
        _grid = new GameObject[_startWidth, _startHeight];
        ResetGrid();
    }

    public GameObject GetNode(Vector2 node)
    {
        return GetNode((int)node.x, (int)node.y);
    }
    public GameObject GetNode(int x, int y)
    {
        if (x > Width-1 || y > Height-1) { return default(GameObject); }
        return _grid[x, y];
    }

    public void SetNode(GameObject newNode, Vector2 node)
    {
        SetNode(newNode, (int)node.x, (int)node.y);
    }
    public void SetNode(GameObject newNode, int x, int y)
    {
        if (x > Width-1 || y > Height-1) { return; }
        _grid[x, y] = newNode;
    }

    public void ForEachNode(Func<GameObject, int, int, GameObject> doWithNode)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _grid[x, y] = doWithNode(_grid[x, y], x, y);
            }
        }
    }

    public void ResetGrid(GameObject node = default(GameObject))
    {
        _grid = ResetGrid(_grid, node);
    }

    public void ResizeGrid(int width, int height, GameObject newNodes = default(GameObject), bool keepOldValues = true)
    {
        var grid = new GameObject[width, height];
        grid = ResetGrid(grid, newNodes);
        if (!keepOldValues)
        {
            _grid = grid;
            return;
        }

        var resetX = width > _grid.GetLength(0) ? _grid.GetLength(0) : width;
        var resetY = height > _grid.GetLength(1) ? _grid.GetLength(1) : height;

        for (int fx = 0; fx < resetX; fx++)
        {
            for (int fy = 0; fy < resetY; fy++)
            {
                grid[fx, fy] = _grid[fx, fy];
            }
        }
        _grid = grid;
    }

    public Vector2[] GetNeighboursPositions(int x, int y, bool cross = true)
    {
        return GetNeighboursPositions(new Vector2(x, y), cross);
    }
    public Vector2[] GetNeighboursPositions(Vector2 nodePosition, bool cross = true)
    {
        List<Vector2> neighbours = new List<Vector2>();
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {

                if ((!cross && (y == 1 || y == -1) && (x == 1 || x == -1)) ||
                    x == 0 && y == 0)
                    continue;

                var position = new Vector2(x, y) + nodePosition;

                if (position.x > Width-1 || position.y > Height-1 || position.x < 0 || position.y < 0)
                    continue;

                neighbours.Add(position);
            }
        }
        return neighbours.ToArray();

    }

    public GameObject[] GetNeighbours(int x, int y, bool cross = true)
    {
        return GetNeighbours(new Vector2(x, y), cross);
    }
    public GameObject[] GetNeighbours(Vector2 nodePosition, bool cross = true)
    {
        List<GameObject> neighbours = new List<GameObject>();
        var positions = GetNeighboursPositions(nodePosition, cross);
        for (int i = 0; i < positions.Length; i++)
        {
            neighbours.Add(GetNode(positions[i]));
        }
        return neighbours.ToArray();

    }

    public GameObject[] GetReletives(int x, int y, Vector2[] reletivesPosition)
    {
        return GetReletives(new Vector2(x, y), reletivesPosition);
    }
    public GameObject[] GetReletives(Vector2 nodePosition, Vector2[] reletivesPosition)
    {
        var nodeX = (int)nodePosition.x;
        var nodeY = (int)nodePosition.y;

        var reletives = new List<GameObject>();

        for (int i = 0; i < reletivesPosition.Length; i++)
        {
            reletives.Add(GetNode(nodeX + (int)reletivesPosition[i].x, nodeY + (int)reletivesPosition[i].y));
        }

        return reletives.ToArray();
    }

    private GameObject[,] ResetGrid(GameObject[,] grid, GameObject node = default(GameObject))
    {

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = node;
            }
        }
        return grid;
    }

}