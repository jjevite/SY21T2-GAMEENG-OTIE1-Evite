using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Identifier;
    public string Name;
    // Add Unit Picture sprite 

    // Current Class this unit Has
    public TypeClass Class { get; set; }
    public GameStat GameStats { get; set; }
    public Health Health { get; set; }
    // You can also just call them ClassPrefab
    public GameObject ClassToEquipPrefab;


    private TurnManager turnManager;
    private void Awake()
    {
        // Instantiates the prefab into a gameobject Then  reference to the TypeClass var for Use
        // Because prefabs are just data/blueprint  not an instance
        Class = Instantiate(ClassToEquipPrefab.GetComponent<TypeClass>(), gameObject.transform);
        GameStats = GetComponent<GameStat>();
        Health = GetComponent<Health>();
        Health.OnDeath += RemoveFromQueue;
    }

    private void Start()
    {
        turnManager = SingletonManager.Get<TurnManager>();
    }

    public void RemoveFromQueue()
    {
        List<GameObject> gameobjectlist = new();
        foreach (GameObject go in turnManager.turnList)
        {
            gameobjectlist.Add(go);
        }

        gameobjectlist.Remove(gameObject);
        turnManager.Units.Remove(gameObject);


        turnManager.turnList.Clear();
        foreach (GameObject gg in gameobjectlist)
        {
            turnManager.turnList.Enqueue(gg);
        }

        bool stillHasUnits = false;
        bool stillHasEnemies = false;
        foreach(GameObject g in turnManager.Units)
        {
            if(g.CompareTag("Unit"))
            {
                stillHasUnits = true;
            }
            else if(g.CompareTag("Enemy"))
            {
                stillHasEnemies = true;
            }
        }

        if(stillHasEnemies == false)
        {
            turnManager.WinGame();
        }
        else if(stillHasUnits == false)
        {
            turnManager.LoseGame();
        }
        Destroy(gameObject);
    }
}
