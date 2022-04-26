using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(0, 2, 0));
    }
}
