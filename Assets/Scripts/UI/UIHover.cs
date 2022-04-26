using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class UIHover : MonoBehaviour
{
    [Header("UI Hover References")]
    public GameObject HoverPanel;
    public GameObject UnitNameTextReference;
    public GameObject HealthBarMaskReference;
    public GameObject ManaBarMaskReference;
    public GameObject JumpHeightTextReference;
    public GameObject MovementRangeTextReference;
    public GameObject SpeedTextReference;
    public GameObject AttackDamageTextReference;
    public GameObject MagicDamageTextReference;

    public TextMeshProUGUI UnitNameText { get; private set; }
    public Image HealthBarMask { get; private set; }
    public Image ManaBarMask { get; private set; }
    public TextMeshProUGUI JumpHeightText { get; private set; }
    public TextMeshProUGUI MovementRangeText { get; private set; }
    public TextMeshProUGUI SpeedText { get; private set; }
    public TextMeshProUGUI AttackDamageText { get; private set; }
    public TextMeshProUGUI MagicDamageText { get; private set; }



    private void Start()
    {
        UnitNameText = UnitNameTextReference.GetComponent<TextMeshProUGUI>();
        HealthBarMask = HealthBarMaskReference.GetComponent<Image>();
        ManaBarMask = ManaBarMaskReference.GetComponent<Image>();
        JumpHeightText = JumpHeightTextReference.GetComponent<TextMeshProUGUI>();
        MovementRangeText = MovementRangeTextReference.GetComponent<TextMeshProUGUI>();
        SpeedText = SpeedTextReference.GetComponent<TextMeshProUGUI>();
        AttackDamageText = AttackDamageTextReference.GetComponent<TextMeshProUGUI>();
        MagicDamageText = MagicDamageTextReference.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Unit")
                {
                    HoverPanel.SetActive(true);

                    Unit unitPtr = hit.collider.gameObject.GetComponent<Unit>();
                    GameStat gameStatPtr = hit.collider.gameObject.GetComponent<GameStat>();
                    UnitNameText.text = unitPtr.Name;
                    HealthBarMask.fillAmount = gameStatPtr.Health.CurrentHP / gameStatPtr.Health.MaxHP;
                    ManaBarMask.fillAmount = gameStatPtr.CurrentMP / gameStatPtr.MaxMP;
                    JumpHeightText.text = gameStatPtr.JumpHeight.ToString();
                    MovementRangeText.text = gameStatPtr.MovementRange.ToString();
                    SpeedText.text = gameStatPtr.Speed.ToString();
                    AttackDamageText.text = gameStatPtr.AttackDamage.ToString();
                    MagicDamageText.text = gameStatPtr.MagicDamage.ToString();
                }
                else if (hit.collider.tag == "Enemy")
                {
                    HoverPanel.SetActive(true);

                    Unit unitPtr = hit.collider.gameObject.GetComponent<Unit>();
                    GameStat gameStatPtr = hit.collider.gameObject.GetComponent<GameStat>();
                    UnitNameText.text = unitPtr.Name;
                    HealthBarMask.fillAmount = gameStatPtr.Health.CurrentHP / gameStatPtr.Health.MaxHP;
                    ManaBarMask.fillAmount = gameStatPtr.CurrentMP / gameStatPtr.MaxMP;
                    JumpHeightText.text = gameStatPtr.JumpHeight.ToString();
                    MovementRangeText.text = gameStatPtr.MovementRange.ToString();
                    SpeedText.text = gameStatPtr.Speed.ToString();
                    AttackDamageText.text = gameStatPtr.AttackDamage.ToString();
                    MagicDamageText.text = gameStatPtr.MagicDamage.ToString();
                }
                else
                {
                    HoverPanel.SetActive(false);
                }
            }
        }
    }
}
