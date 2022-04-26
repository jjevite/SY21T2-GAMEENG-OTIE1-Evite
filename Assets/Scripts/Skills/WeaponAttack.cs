using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : Skill
{
    public override void FindRange()
    {
        selectTileManager.FindSelectableTilesToInteract(owner.GetComponent<Unit>().GameStats.JumpHeight,
                                                        owner.GetComponent<Unit>().Class.Weapon.Range,
                                                         owner);
        checkingInput = true;
    }
    public override void Cast()
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
                    if (t.InteractSelectable)
                    {
                        RaycastHit hitUnitOnTop;
                        //Unit unit = null;
                        if (Physics.Raycast(hit.collider.gameObject.transform.position, Vector3.up, out hitUnitOnTop, 1))
                        {
                            if (hitUnitOnTop.collider.CompareTag("Enemy"))
                            {
                                t.Target = true;
                                hitUnitOnTop.collider.GetComponent<Health>().TakeDamage(owner.GetComponent<Unit>().Class.Weapon.AttackDamage);
                                
                                targetUnit = hitUnitOnTop.collider.gameObject;
                                StartCoroutine(LookAtEnemy());
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator LookAtEnemy()
    {
        owner.transform.LookAt(targetUnit.transform);
        owner.gameObject.transform.Find("MESH").GetComponent<Animator>().SetBool("isAttacking", true);
        turnManager.UiAction.SkillCancelPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        owner.gameObject.transform.Find("MESH").GetComponent<Animator>().SetBool("isAttacking", false);
        turnManager.OnSkillFinish();
    }
}
