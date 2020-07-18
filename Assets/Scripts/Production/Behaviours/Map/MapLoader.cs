using AI;
using System.Linq;
using UnityEngine;
using Tools;
using System.Collections.Generic;

/// <summary>
/// Class responsible for generating the map in the scene.
/// </summary>
public class MapLoader : MonoBehaviour
{
    public MapSettings MapData;
    public Player Player;
    [System.NonSerialized] public Tile[,] Grid;
    [System.NonSerialized] public Vector2Int MapSize;
    private Vector2Int m_EndTile;
    private Vector2Int m_StartTile;

    private void Awake()
    {
        CreateMap();
        CreatePath();
        CreateUnitFactory();
    }

    /// <summary>
    /// Creates the Factory where enemies spawn from.
    /// </summary>
    private void CreateUnitFactory()
    {
        GameObject factoryGo = new GameObject("Unit Factory");
        factoryGo.transform.position = Grid[m_StartTile.x, m_StartTile.y].transform.position;
        Factory factory = factoryGo.ForceComponent<Factory>();
        string waveData = MapData.Map.text.Substring(MapData.Map.text.IndexOf('#') + 2);
        factory.CreateWaves(waveData);
        factory.Construct(MapData, Player, CreatePath());
    }

    /// <summary>
    /// Creates the path that enemies will take to reach the players base.
    /// </summary>
    private IEnumerable<Vector2Int> CreatePath()
    {
        var pathFinder = new Dijkstra(this);
        return pathFinder.FindPath(m_StartTile, m_EndTile);
    }

    /// <summary>
    /// Creates all the tiles listed in the map. Path, Obsticle, Bases and Towers.
    /// </summary>
    private void CreateMap()
    {
        GameObject mapGo = new GameObject("Map");
        MapSize.x = MapData.Map.text.Substring(0, MapData.Map.text.IndexOf('#')).Count(x => x == '\n');
        MapSize.y = MapData.Map.text.Substring(0, MapData.Map.text.IndexOf('\n')).Length;
        Grid = new Tile[MapSize.x, MapSize.y];

        short currentColumn = 0;
        short currentRow = 0;

        foreach (char ch in MapData.Map.text)
        {
            if (ch == '\n')
            {
                currentRow++;
                currentColumn = 0;
                continue;
            }

            if (ch == '#')
            {
                break;
            }

            if (int.TryParse(ch.ToString(), out int tile))
            {
                TileType tileT = (TileType)tile;

                Grid[currentRow, currentColumn] = Instantiate(GetTile(tileT), new Vector3(currentRow * MapData.TileSize, 0, currentColumn * MapData.TileSize), Quaternion.identity);
                Grid[currentRow, currentColumn].transform.localScale.SetX(MapData.TileSize);
                Grid[currentRow, currentColumn].transform.localScale.SetZ(MapData.TileSize);         
                Grid[currentRow, currentColumn].transform.SetParent(mapGo.transform);
                Grid[currentRow, currentColumn].Pos = new Vector2Int(currentRow, currentColumn);

                if (tileT == TileType.End)
                {
                    m_EndTile = Grid[currentRow, currentColumn].Pos;
                }
                if (tileT == TileType.Start)
                {
                    m_StartTile = Grid[currentRow, currentColumn].Pos;
                }

                currentColumn++;
            }
        }
    }

    /// <summary>
    /// Get the tile type.
    /// </summary>
    /// <param name="tile">Tile number ID</param>
    /// <returns></returns>
    private Tile GetTile(TileType tile)
    {
        switch (tile)
        {
            case TileType.Path:
                return MapData.Path;
            case TileType.Obstacle:
                return MapData.Obstacle;
            case TileType.TowerOne:
                return MapData.BombTower;
            case TileType.TowerTwo:
                return MapData.FreezeTower;
            case TileType.Start:
                return MapData.Start;
            case TileType.End:
                return MapData.End;
        }

        return new Tile();
    }
}
