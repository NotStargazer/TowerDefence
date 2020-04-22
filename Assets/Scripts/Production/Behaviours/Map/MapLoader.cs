using AI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools;
using System;

[MonoSingletonConfiguration("Map")]
public class MapLoader : MonoSingleton<MapLoader>
{
    public MapSettings mapData;
    [System.NonSerialized] public Tile[,] grid;
    [System.NonSerialized] public Vector2Int mapSize;
    [System.NonSerialized] public Vector2Int endTile;
    [System.NonSerialized] public Vector2Int startTile;
    [System.NonSerialized] public IEnumerable<Vector2Int> AIpath;

    private void Start()
    {
        CreateMap();
        CreatePath();
        CreateUnitFactory();
    }

    private void CreateUnitFactory()
    {
        GameObject factoryGo = new GameObject("Unit Factory");
        factoryGo.transform.position = grid[startTile.x, startTile.y].transform.position;
        factoryGo.ForceComponent<Factory>();
        string waveData = mapData.map.text.Substring(mapData.map.text.IndexOf('#') + 2);
        factoryGo.GetComponent<Factory>().CreateWaves(waveData);
    }

    private void CreatePath()
    {
        var pathFinder = new Dijkstra();
        AIpath = pathFinder.FindPath(startTile, endTile);
    }

    private void CreateMap()
    {
        GameObject mapGo = new GameObject("Map");
        mapSize.x = mapData.map.text.Substring(0, mapData.map.text.IndexOf('#')).Count(x => x == '\n');
        mapSize.y = mapData.map.text.Substring(0, mapData.map.text.IndexOf('\n')).Length;
        grid = new Tile[mapSize.x, mapSize.y];

        short currentColumn = 0;
        short currentRow = 0;

        foreach (char ch in mapData.map.text)
        {
            if (ch == '\n')
            {
                currentRow++;
                currentColumn = 0;
                continue;
            }

            if (ch == '#') 
                break;

            if (int.TryParse(ch.ToString(), out int tile))
            {
                grid[currentRow, currentColumn] = Instantiate(GetTile(tile), new Vector3(currentRow * mapData.tileSize, 0, currentColumn * mapData.tileSize), Quaternion.identity);
                grid[currentRow, currentColumn].transform.localScale.SetX(mapData.tileSize);
                grid[currentRow, currentColumn].transform.localScale.SetZ(mapData.tileSize);


                if (tile == 2 || tile == 3) grid[currentRow, currentColumn].type = (TileType)tile;

                grid[currentRow, currentColumn].transform.SetParent(mapGo.transform);
                grid[currentRow, currentColumn].pos = new Vector2Int(currentRow, currentColumn);

                if (tile == 9) endTile = grid[currentRow, currentColumn].pos;
                if (tile == 8) startTile = grid[currentRow, currentColumn].pos;

                currentColumn++;
            }
        }
    }

    private Tile GetTile(int tile)
    {
        switch (tile)
        {
            case 0:
                return mapData.path;
            case 1:
                return mapData.obstacle;
            case 2:
            case 3:
                return mapData.tower;
            case 8:
                return mapData.start;
            case 9:
                return mapData.end;
        }

        return new Tile();
    }
}
