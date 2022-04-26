using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuBar
{
    [MenuItem("Tools/Assign Tile Material")]
    public static void AssignMaterial()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        Material material = Resources.Load<Material>("Tile");

        foreach(GameObject i in tiles)
        {
            i.GetComponent<Renderer>().material = material;
        }
    }

    [MenuItem("Tools/Assign Tile Script")]
    public static void AssignTileScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        Material material = Resources.Load<Material>("Tile");

        foreach (GameObject i in tiles)
        {
            i.AddComponent<Tile>();
        }
    }
}
