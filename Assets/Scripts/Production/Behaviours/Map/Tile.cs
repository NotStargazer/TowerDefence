using UnityEngine;

/// <summary>
/// Stores tile data for dijkstra and maploader.
/// </summary>
public class Tile : MonoBehaviour
{
    public TileType Type;
    [System.NonSerialized] public float HCost;
    [System.NonSerialized] public Vector2Int Pos;
    [System.NonSerialized] public Tile Parent;

    /// <summary>
    /// Evaluates if you can walk on this type of tile.
    /// </summary>
    public bool CanWalk()
    {
        if (Type == TileType.Path)
        {
            return true;
        }

        return false;
    }
}
