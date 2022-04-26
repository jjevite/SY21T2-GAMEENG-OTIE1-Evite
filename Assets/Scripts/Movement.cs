using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Height of the Collider
    // So that it doesnt go underneath the tile
    private float halfHeight;

    // How fast the unit will walk across the tile
    public float MoveSpeed = 2;
    public float jumpVelocity = 4.5f;

    // How fast the player moves
    Vector3 velocity = new Vector3();
    // Direction the player is heading into
    Vector3 heading = new Vector3();

    // If true it means the unit found a tile to move to and can proceed to move to the tile
    // Will Turn False after reaching Destination
    public bool moving = false;

    // Jumping Variables/Flags
    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    Vector3 jumpTarget;

    protected SelectTileManager SelectTileManager { get; private set; }
    protected TurnManager TurnManager { get; private set; }
    private void Start()
    {
        SelectTileManager = SingletonManager.Get<SelectTileManager>();
        TurnManager = SingletonManager.Get<TurnManager>();

        halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    private void Update()
    {
        if(moving)
        {
            MoveUnitToTile();
        }
    }

    public void MoveUnitToTile()
    {
        Stack<Tile> path = SelectTileManager.path;
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            // Calculate the units position on top of tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jump = transform.position.y != target.y;

                if(jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }
        

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                // Tile Center Reached
                transform.position = target;
                // Maybe So No Jumping Required
                //transform.rotation = Quaternion.identity;
                //transform.forward = heading;
                path.Pop();
            }
        }
        else
        {
            if(gameObject.CompareTag("Unit"))
            {
                SelectTileManager.RemoveSelectableTiles();
                moving = false;
                // UI UI UI UI UI UI UI 
                TurnManager.UiAction.ActionPanel.SetActive(true);
            }
            else if(gameObject.CompareTag("Enemy"))
            {
                SelectTileManager.RemoveSelectableTiles();
                moving = false;
                if(TurnManager != null)
                {
                    TurnManager.EnemyAttack();
                }
                //TurnManager.UiAction.OnWaitButtonClicked();
            }
        
        }
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * MoveSpeed;
    }

    void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDownward(target);
        }
        else if(jumpingUp)
        {
            JumpingUpward(target);
        }
        else if(movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        CalculateHeading(target);
        if(transform.position.y > target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * MoveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if(transform.position.y <= target.y)
        {
            fallingDown = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new();
        }
    }

    void JumpingUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if(transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
            movingEdge = false;

        }
    }

    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= 4.0f;
            velocity.y = 1.5f;
        }
    }
}
