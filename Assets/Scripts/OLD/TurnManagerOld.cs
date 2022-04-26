using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerOld : MonoBehaviour
{
    public List<Unit> Units = new();
    public List<Tile> Tiles = new();

    private Queue<TacticsMove> turnList = new Queue<TacticsMove>();

    private void Awake()
    {
        SingletonManager.Register(this);    
    }

    private void Start()
    {
        if(turnList.Count <= 0)
        {
            InitQueue();
        }
    }

    private void InitQueue()
    {
        //GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        // Getcomponent Unit Stat Speed then sort from fastest to slowerst
        foreach (Unit unit in Units)
        {
            turnList.Enqueue(unit.GetComponent<TacticsMove>());
        }
        StartGame();
    }

    private void StartGame()
    {
        if (turnList.Count > 0)
        {
            turnList.Peek().BeginTurn();
        }
    }



    public void EndTurn()
    {
        TacticsMove unit = turnList.Dequeue();
        unit.EndTurn();

        if(turnList.Count > 0)
        {
            StartGame();
        }
        else
        {
            InitQueue();
        }
    }
}
