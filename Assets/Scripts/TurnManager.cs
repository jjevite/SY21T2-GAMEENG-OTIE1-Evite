using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    // For Debugging 
    [SerializeField] Tile nooo;
    GameObject nearestUnit;

    // LINE 223 LINE 111 LINE 87 has UI
    public UIAction UiAction;

    // Turns true if currently displaying moveable tiles, turns false when you click on a valid tile
    public bool checkingMovement = false;

    // Editor Referenced, all Units in the current Scene
    public List<GameObject> Units = new();
    // Editor Referenced, all Tiles in the current Scene
    public List<Tile> Tiles = new();

    // All units that are currently in queue for turn
    public Queue<GameObject> turnList = new Queue<GameObject>();

    // Manages selecting tiles avialbe to the current Unit in turn
    public SelectTileManager SelectTileManager { get; private set; }

    #region Unity Functions
    private void Awake()
    {
        SingletonManager.Register(this);
    }

    private void Start()
    {
        SelectTileManager = SingletonManager.Get<SelectTileManager>();
        if (turnList.Count <= 0)
        {
            InitQueue();
        }
    }

    private void Update()
    {
         if(checkingMovement)
         {
            CheckMouse();
         }
    }

    #endregion

    #region Private Functions
    public void InitQueue()
    {
        //GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        // Getcomponent Unit Stat Speed then sort from fastest to slowerst
        foreach (GameObject unit in Units)
        {
            turnList.Enqueue(unit);
        }
        StartGame();
    }

    public void StartGame()
    {
        if (turnList.Count > 0)
        {
            // UI UI UI UI UI UI
            UiAction.ResetBattleHandlerCanvasAndElements();
            turnList.Peek().transform.Find("TurnIndicator").gameObject.SetActive(true);
            if (turnList.Peek().gameObject.CompareTag("Enemy"))
            {
                UiAction.ActionCanvas.SetActive(false);
                EnemyMove();
            }
        }
    }

    private void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.MoveSelectable)
                    {
                        // Target To Move to
                        SelectTileManager.CreatePathToTarget(t);
                        checkingMovement = false;
                        turnList.Peek().GetComponent<Movement>().moving = true;

                        // UI UI UI UI UI UI UI UI UI UI UI
                        UiAction.MoveButton.SetActive(false);
                        UiAction.ActionPanel.SetActive(false);
                        UiAction.MoveButtonCancelPanel.SetActive(false);
                    }
                }
            }
        }
    }

    #endregion

    #region public Functions
    public void OnSkillFinish()
    {
        turnList.Peek().GetComponent<Unit>().Class.SkillActor.BasicAttack.Cancel();
        turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill1.Cancel();
        turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill2.Cancel();

        // UI UI UI UI UI UI UI UI
        UiAction.ActionPanel.SetActive(true);
        UiAction.SkillCancelPanel.SetActive(false);
        UiAction.ActButton.SetActive(false);
    }

    public void WinGame()
    {
        UiAction.WinCanvas.SetActive(true);
        Destroy(gameObject);
    }

    public void LoseGame()
    {
        UiAction.LoseCanvas.SetActive(true);
        Destroy(gameObject);
     
    }

    #endregion

    #region EnemyAI
    private void EnemyMove()
    {
        SelectTileManager.FindSelectableTilesToMove(turnList.Peek().GetComponent<Unit>().GameStats.JumpHeight,
                                            turnList.Peek().GetComponent<Unit>().GameStats.MovementRange,
                                            turnList.Peek());

        if(FindNearestUnit() == null)
        {
            Destroy(gameObject);
        }
        nearestUnit =  FindNearestUnit();
        nooo = FindNearestTileToNearestUnit(nearestUnit);
        SelectTileManager.CreatePathToTarget(nooo);
        turnList.Peek().GetComponent<Movement>().moving = true;

        //UiAction.OnWaitButtonClicked();
    }

    public void EnemyAttack()
    {
        turnList.Peek().GetComponent<Unit>().Class.SkillActor.BasicAttack.FindRange();
        turnList.Peek().GetComponent<Unit>().Class.SkillActor.BasicAttack.checkingInput = false;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        Tile t = FindNearestTileToUnitToAttack(nearestUnit);
        t.Target = true;

        yield return new WaitForSeconds(1);
   
        if (t.InteractSelectable)
        {
            RaycastHit hitUnitOnTop;
            if (Physics.Raycast(t.gameObject.transform.position, Vector3.up, out hitUnitOnTop, 1))
            {
                if (hitUnitOnTop.collider.CompareTag("Unit"))
                {
                    t.Target = true;
                    turnList.Peek().transform.LookAt(hitUnitOnTop.collider.gameObject.transform);
                    turnList.Peek().transform.Find("MESH").GetComponent<Animator>().SetBool("isAttacking", true);
                    yield return new WaitForSeconds(1f);
                    turnList.Peek().transform.Find("MESH").GetComponent<Animator>().SetBool("isAttacking", false);
                    hitUnitOnTop.collider.GetComponent<Health>().TakeDamage(turnList.Peek().GetComponent<Unit>().Class.Weapon.AttackDamage);
                }
            }
        }
        SelectTileManager.RemoveSelectableTilesToInteract();
        UiAction.OnWaitButtonClicked();
    }

    GameObject FindNearestUnit()
    {
        //List<GameObject> target = new();
        //foreach(GameObject go in Units)
        //{
        //    if(go.CompareTag("Unit"))
        //    {
        //        target.Add(go);
        //    }
        //}

        GameObject[] target = GameObject.FindGameObjectsWithTag("Unit");
        GameObject nearest = null;
        float distance = Mathf.Infinity;
        foreach(GameObject go in target)
        {
            float dist = Vector3.Distance(turnList.Peek().transform.position, go.transform.position);

            if(dist < distance)
            {
                distance = dist;
                nearest = go;
            }
        }
        //target.Clear();
        //target = null;
        return nearest;
    }

    Tile FindNearestTileToNearestUnit(GameObject nearest)
    {
        Tile closestTile = null;
        float distance = Mathf.Infinity;
        foreach(Tile t in Tiles)
        {
            if(t.MoveSelectable)
            {
                float d = Vector3.Distance(nearest.transform.position, t.transform.position);
                if(d < distance)
                {
                    distance = d;
                    closestTile = t;
                }
            }
        }
        return closestTile;
    }

    Tile FindNearestTileToUnitToAttack(GameObject nearest)
    {
        Tile closestTile = null;
        float distance = Mathf.Infinity;
        foreach (Tile t in Tiles)
        {
            if (t.InteractSelectable)
            {
                float d = Vector3.Distance(nearest.transform.position, t.transform.position);
                if (d < distance)
                {
                    distance = d;
                    closestTile = t;
                }
            }
        }
        return closestTile;
    }
    #endregion
}
