using UnityEngine;

[CreateAssetMenu(fileName = "Map Settings", menuName = "Tower Defence/Map Settings", order = 1)]
public class MapSettings : ScriptableObject
{
    [Header("Map Settings")]
    public TextAsset map;
    public float tileSize = 2;
    public Tile path;
    public Tile obstacle;
    public Tile tower;
    public Tile start;
    public Tile end;

    [Header("Unit Settings")]
    public GameObject unit;
    public float standardUnitHealth = 100;
    public float standardUnitSpeed = 2;
    public float standardSpawnInterval = 0.5f;
    public float largeUnitHealth = 500;
    public float largeUnitSpeed = 1;
    public float largeSpawnInterval = 0.5f;

    public float nextWaveInterval = 2f;

    [Header("Tower Settings")]
    public float bombTowerCooldownTime = 0.5f;
    public float bombTowerExplosionRadius = 2;
    public float bombTowerExplosionDamage = 10;
    public float freezeTowerCooldownTime = 0.5f;
    public float freezeTowerDamage = 10;
    public float freezeTowerSlowDuration = 2f;
    [Range(0,1)] public float freezeTowerSlowAmount = 0.5f;
}
