using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
    // Update is called once per frame
    private void Update()
    {
        if(!turn)
        {
            return;
        }

        if(!moving)
        {
            FindSelectableTiles();
            CheckMouse();
        }
        else
        {
            MoveUnitToTile();
        }
    }

    private void CheckMouse()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if(t.MoveSelectable)
                    {
                        // Target To Move to
                        CreatePathToTarget(t);
                    }
                }
            }
        }
    }
}
