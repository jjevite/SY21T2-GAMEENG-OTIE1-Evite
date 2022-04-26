using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public int Identifier;
    public string Name;
    [TextArea]
    public string Description;

    protected GameObject owner;
    protected GameObject targetUnit;
    protected TurnManager turnManager;
    protected SelectTileManager selectTileManager;
    public bool checkingInput = false;

    public abstract void FindRange();
    public abstract void Cast();
    public void Cancel()
    {
        selectTileManager.RemoveSelectableTilesToInteract();
        checkingInput = false;
    }

    private void Start()
    {
        owner = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        turnManager = SingletonManager.Get<TurnManager>();
        selectTileManager = SingletonManager.Get<SelectTileManager>();
    }

    private void Update()
    {
        if(checkingInput)
        {
            Cast();
        }
    }
}
