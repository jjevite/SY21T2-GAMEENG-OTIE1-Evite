using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    // Make a manager that handles where units can move
    // Let the Unit do the moving after the manager tells them where they can move

    protected bool turn = false;

    List<Tile> selectableTiles = new List<Tile>();
    //GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    // If its moving, dont look for selectable tiles
    protected bool moving = false;
    // Movement Range of the Unit
    public int Move = 5;
    // Jump Height
    public float JumpHeight = 2;
    // How fast the unit will walk across the tile
    public float MoveSpeed = 2;


    // How fast the player moves
    Vector3 velocity = new Vector3();
    // Direction the player is heading into
    Vector3 heading = new Vector3();

    // How tall half the tile is
    float halfHeight = 0;

    protected TurnManagerOld TurnManager { get; private set; }
    // private List<Tile> Tiles => TurnManager.Tiles;

    private void Start()
    {
        TurnManager = SingletonManager.Get<TurnManagerOld>();

        // Cache all the tile in a single array
        // Doesnt work with dynamic levels
        //tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    protected virtual void Init() { }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.Current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if(Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }

    public void ComputeAdjacencyList()
    {
        foreach(Tile tile in TurnManager.Tiles)
        {
            //Tile t = tile.GetComponent<Tile>();
            tile.FindNeighboors(JumpHeight);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyList();
        GetCurrentTile();

        // BFS
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.Visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.MoveSelectable = true;

            if (t.Distance < Move)
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

    public void CreatePathToTarget(Tile tile)
    {
        path.Clear();
        tile.Target = true;
        moving = true;

        Tile next = tile;
        while(next!= null)
        {
            path.Push(next);
            next = next.Parent;
        }
    }

    public void MoveUnitToTile()
    {
        if(path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            // Calculate the units position on top of tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                // Tile Center Reached
                transform.position = target;
                // Maybe So No Jumping Required
                transform.rotation = Quaternion.identity;
                path.Pop();
            }


        }
        else
        {
            RemoveSelectableTiles();
            moving = false;

            TurnManager.EndTurn();
        }
    }

    // Remove Selectable Tiles and Resets Vars
    protected void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.Current = false;
            currentTile = null;
        }
        foreach(Tile tile in selectableTiles)
        {
            tile.ResetGame();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * MoveSpeed;
    }

    public void BeginTurn()
    {
        turn = true;
    }

    public void EndTurn()
    {
        turn = false;
    }
}
