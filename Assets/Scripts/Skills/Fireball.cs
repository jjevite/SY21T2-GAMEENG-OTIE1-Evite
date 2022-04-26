using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    public override void FindRange()
    {
        selectTileManager.FindSelectableTilesToInteract(owner.GetComponent<Unit>().GameStats.JumpHeight,
                                                        3,
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
                            if (hitUnitOnTop.collider.CompareTag("Unit"))
                            {
                                t.Target = true;
                                hitUnitOnTop.collider.GetComponent<Unit>().GameStats.Health.CurrentHP -= 50;
                                turnManager.OnSkillFinish();
                            }
                        }
                    }
                }
            }
        }
    }
}
