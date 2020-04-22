using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType type;
    [System.NonSerialized] public float hCost;
    [System.NonSerialized] public Vector2Int pos;
    [System.NonSerialized] public Tile parent;

    public bool CanWalk()
    {
        if (type == TileType.Path) return true;

        return false;
    }
}
