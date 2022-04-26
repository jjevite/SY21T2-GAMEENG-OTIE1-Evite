using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTileManager : MonoBehaviour
{
    // A collection of all tiles available to be selected
    List<Tile> selectableTiles = new List<Tile>();
    // Path the Unit will move through to destiation
    public Stack<Tile> path = new Stack<Tile>();
    // Tile the unit is currently sitting on
    Tile currentTile;

    protected TurnManager TurnManager { get; private set; }

    private void Awake()
    {
        SingletonManager.Register(this);
    }

    private void Start()
    {
        TurnManager = SingletonManager.Get<TurnManager>();
    }

    public void GetCurrentTile(GameObject CurrentUnit)
    {
        currentTile = GetTargetTile(CurrentUnit);
        currentTile.Current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }

    public void ComputeAdjacencyList(float JumpHeight)
    {
        foreach (Tile tile in TurnManager.Tiles)
        {
            //Tile t = tile.GetComponent<Tile>();
            tile.FindNeighboors(JumpHeight);
        }
    }

    public void ComputeAdjacencyListForTargeting(float JumpHeight)
    {
        foreach (Tile tile in TurnManager.Tiles)
        {
            //Tile t = tile.GetComponent<Tile>();
            tile.FindNeighboorsToTarget(JumpHeight);
        }
    }

    public void FindSelectableTilesToMove(float JumpHeight, int MovementRange, GameObject CurrentUnit)
    {
        ComputeAdjacencyList(JumpHeight);
        GetCurrentTile(CurrentUnit);

        // BFS
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.Visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.MoveSelectable = true;

            if (t.Distance < MovementRange)
            {
                foreach (Tile tile in t.AdjacencyList)
                {
                    if (!tile.Visited)
                    {
                        tile.Parent = t;
                        tile.Visited = true;
                        tile.Distance = 1 + t.Distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
        currentTile.MoveSelectable = false;
    }

    public void FindSelectableTilesToInteract(float JumpHeight, int AttackRange, GameObject CurrentUnit)
    {
        ComputeAdjacencyListForTargeting(JumpHeight);
        GetCurrentTile(CurrentUnit);

        // BFS
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.Visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.InteractSelectable = true;

            if (t.Distance < AttackRange)
            {
                foreach (Tile tile in t.AdjacencyList)
                {
                    if (!tile.Visited)
                    {
                        tile.Parent = t;
                        tile.Visited = true;
                        tile.Distance = 1 + t.Distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void RemoveSelectableTilesToInteract()
    {
        if (currentTile != null)
        {
            currentTile.Current = false;
            currentTile = null;
        }
        foreach (Tile tile in selectableTiles)
        {
            tile.ResetGame();
        }

        selectableTiles.Clear();
    }

    public void CreatePathToTarget(Tile tile)
    {
        path.Clear();
        tile.Target = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.Parent;
        }
    }

    public void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.Current = false;
            currentTile = null;
        }
        foreach (Tile tile in selectableTiles)
        {
            tile.ResetGame();
        }

        selectableTiles.Clear();
    }
}
