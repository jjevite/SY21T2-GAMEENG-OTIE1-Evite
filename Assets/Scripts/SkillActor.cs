using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActor : MonoBehaviour
{
    public Skill BasicAttack { get; set; }
    public Skill ActiveSkill1 { get; set; }
    public Skill ActiveSkill2 { get; set; }
    public Skill PassiveSkill { get; set; }

    public GameObject BasicAttackPrefab;
    public GameObject ActiveSkill1ToEquipPrefab;
    public GameObject ActiveSkill2ToEquipPrefab;
    public GameObject PassiveSkillToEquipPrefab;

    private void Awake()
    {
        // Instantiates the prefab into a gameobject Then  reference to the Skill var for Use
        BasicAttack = Instantiate(BasicAttackPrefab.GetComponent<Skill>(), gameObject.transform);
        ActiveSkill1 = Instantiate(ActiveSkill1ToEquipPrefab.GetComponent<Skill>(), gameObject.transform);
        ActiveSkill2 = Instantiate(ActiveSkill2ToEquipPrefab.GetComponent<Skill>(), gameObject.transform);
        PassiveSkill = Instantiate(PassiveSkillToEquipPrefab.GetComponent<Skill>(), gameObject.transform);
    }

    private void Start()
    {
        StartCoroutine(LateStart(5));
    }

    // Scuffed! the start referencing wont kick in before the passives casts
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PassiveSkill.Cast();
    }
}
