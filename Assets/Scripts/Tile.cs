using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool Walkable = true;
    public bool Current = false;
    public bool Target = false;
    public bool MoveSelectable = false;
    public bool InteractSelectable = false;

    public List<Tile> AdjacencyList = new List<Tile>();

    // Breadth First Search for Movement
    public bool Visited = false;
    public Tile Parent = null;
    public int Distance = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Current)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (Target)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (MoveSelectable)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (InteractSelectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }
    public void ResetGame()
    {
        AdjacencyList.Clear();

        Current = false;
        Target = false;
        MoveSelectable = false;
        InteractSelectable = false;

        Visited = false;
        Parent = null;
        Distance = 0;
    }

    public void FindNeighboors(float jumpHeight)
    {
        ResetGame();

        CheckTile(Vector3.forward, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
        CheckTile(-Vector3.forward, jumpHeight);
        CheckTile(-Vector3.right, jumpHeight);
    }

    public void CheckTile(Vector3 direction, float jumpHeight)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if(tile != null && tile.Walkable)
            {
                RaycastHit hit;

                if(!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    AdjacencyList.Add(tile);
                }
            }
        }
    }

    public void FindNeighboorsToTarget(float jumpHeight)
    {
        ResetGame();

        CheckTileIfTargetable(Vector3.forward, jumpHeight);
        CheckTileIfTargetable(Vector3.right, jumpHeight);
        CheckTileIfTargetable(-Vector3.forward, jumpHeight);
        CheckTileIfTargetable(-Vector3.right, jumpHeight);
    }

    public void CheckTileIfTargetable(Vector3 direction, float jumpHeight)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.Walkable)
            {
                AdjacencyList.Add(tile);
            }
        }
    }
}
