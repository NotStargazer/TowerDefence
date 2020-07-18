using UnityEngine;

/// <summary>
/// Data store for maps and units.
/// </summary>
[CreateAssetMenu(fileName = "Map Settings", menuName = "Tower Defence/Map Settings", order = 1)]
public class MapSettings : ScriptableObject
{
    [Header("Map Settings")]
    public TextAsset Map;
    public float TileSize = 2;
    public Tile Path;
    public Tile Obstacle;
    public Tile BombTower;
    public Tile FreezeTower;
    public Tile Start;
    public Tile End;
    public GameObject Unit;

    public float NextWaveInterval = 2f;
    public float StandardSpawnInterval = 0.5f;
    public float LargeSpawnInterval = 0.5f;
}
