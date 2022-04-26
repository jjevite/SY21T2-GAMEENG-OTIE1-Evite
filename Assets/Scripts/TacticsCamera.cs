using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsCamera : MonoBehaviour
{
    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, 30, Space.Self);
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up, -30, Space.Self);
    }
    public void RotateDown()
    {
        transform.Rotate(Vector3.right, 30, Space.Self);
    }
    public void RotateUp()
    {
        transform.Rotate(Vector3.right, -30, Space.Self);
    }
}
