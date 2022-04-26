using UnityEngine;
using TMPro;

public class UIAction : MonoBehaviour
{
    // Add UI Action Manager
    [Header("UI References")]
    public GameObject ActionCanvas;
    public GameObject ActionPanel;
    public GameObject MoveButton;
    public GameObject ActButton;
    public GameObject MoveButtonCancelPanel;
    public GameObject ActPanel;
    public GameObject SkillCancelPanel;
    public TextMeshProUGUI ActiveSkill1;
    public TextMeshProUGUI ActiveSkill2;

    [Header("InfoPanelUIReference")]
    public GameObject InfoPanel;
    public GameObject InfoBackButton;
    public TextMeshProUGUI UnitName;
    public TextMeshProUGUI UnitClass;
    public TextMeshProUGUI UnitHP;
    public TextMeshProUGUI UnitMP;
    public TextMeshProUGUI ActiveSkill1Info;
    public TextMeshProUGUI ActiveSkill2Info;

    [Header("WinLoseConditionsPanel")]
    public GameObject WinCanvas;
    public GameObject LoseCanvas;

    TurnManager turnManager;
    private void Start()
    {
        turnManager = SingletonManager.Get<TurnManager>();
    }

    #region Public Methods
    public void ResetBattleHandlerCanvasAndElements()
    {
        ActionCanvas.SetActive(true);
        ActionPanel.SetActive(true);
        MoveButton.SetActive(true);
        ActButton.SetActive(true);
    }
    #endregion


    public void OnMoveButtonClicked()
    {
        turnManager.SelectTileManager.FindSelectableTilesToMove(turnManager.turnList.Peek().GetComponent<Unit>().GameStats.JumpHeight,
                                            turnManager.turnList.Peek().GetComponent<Unit>().GameStats.MovementRange,
                                            turnManager.turnList.Peek());

        ActionPanel.SetActive(false);
        MoveButtonCancelPanel.SetActive(true);
        turnManager.checkingMovement = true;
    }

    public void OnMoveCancelButtonClicked()
    {
        ActionPanel.SetActive(true);
        MoveButtonCancelPanel.SetActive(false);
        turnManager.checkingMovement = false;
        turnManager.SelectTileManager.RemoveSelectableTiles();
    }

    public void OnWaitButtonClicked()
    {
        turnManager.turnList.Peek().transform.Find("TurnIndicator").gameObject.SetActive(false);
        turnManager.turnList.Dequeue();

        if (turnManager.turnList.Count > 0)
        {
            turnManager.StartGame();
        }
        else
        {
            turnManager.InitQueue();
        }
    }

    public void OnActButtonClicked()
    {
        ActPanel.SetActive(true);
        ActionPanel.SetActive(false);
        ActiveSkill1.text = turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill1.Name;
        ActiveSkill2.text = turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill2.Name;
    }

    public void OnActButtonCancelClicked()
    {
        ActPanel.SetActive(false);
        ActionPanel.SetActive(true);
    }

    public void OnBasicAttackButtonClicked()
    {
        turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.BasicAttack.FindRange();
        ActPanel.SetActive(false);
        SkillCancelPanel.SetActive(true);
    }
    public void OnActiveSkill1ButtonClicked()
    {
        turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill1.FindRange();
        ActPanel.SetActive(false);
        SkillCancelPanel.SetActive(true);
    }
    public void OnActiveSkill2ButtonClicked()
    {
        turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill2.FindRange();
        ActPanel.SetActive(false);
        SkillCancelPanel.SetActive(true);
    }
    public void OnSkillCanceledButtonClicked()
    {
        turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.BasicAttack.Cancel();
        turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill1.Cancel();
        turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill2.Cancel();
        ActPanel.SetActive(true);
        SkillCancelPanel.SetActive(false);
    }

    public void OnInfoPanelButtonClicked()
    {
        UnitName.text = turnManager.turnList.Peek().GetComponent<Unit>().Name;
        UnitClass.text = turnManager.turnList.Peek().GetComponent<Unit>().Class.Name;
        UnitHP.text = turnManager.turnList.Peek().GetComponent<Unit>().GameStats.Health.CurrentHP.ToString();
        UnitMP.text = turnManager.turnList.Peek().GetComponent<Unit>().GameStats.CurrentMP.ToString();
        ActiveSkill1Info.text = turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill1.Name;
        ActiveSkill2Info.text = turnManager.turnList.Peek().GetComponent<Unit>().Class.SkillActor.ActiveSkill2.Name;
        InfoPanel.SetActive(true);
        ActionPanel.SetActive(false);
    }

    public void OnInfoPanelBackButtonClicked()
    {
        InfoPanel.SetActive(false);
        ActionPanel.SetActive(true);
    }
}
