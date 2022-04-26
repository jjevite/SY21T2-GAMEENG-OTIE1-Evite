using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeClass : MonoBehaviour
{
    // Editor Referenced
    [TextArea]
    public string Description;
    // Add Class Icon Here
    public string Name;
    public ClassStat Stats;
    public SkillActor SkillActor;
    public Weapon Weapon { get; set; }

    public GameObject WeaponToEquipPrefab;

    private void Awake()
    {
        // Instantiates the prefab into a gameobject Then  reference to the Weapon var for Use
        Weapon = Instantiate(WeaponToEquipPrefab.GetComponent<Weapon>(), gameObject.transform);
    }
}
