using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace AI
{
    //TODO: Implement IPathFinder using Dijsktra algorithm.
    public class Dijkstra : IPathFinder
    {
        public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
        {
            Tile[,] grid = MapLoader.Instance.grid;
            Tile startNode = grid[start.x, start.y];
            Tile endNode = grid[goal.x, goal.y];

            List<Tile> openSet = new List<Tile>();
            HashSet<Tile> closedSet = new HashSet<Tile>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Tile currentNode = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].hCost < currentNode.hCost) currentNode = openSet[i];
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == endNode)
                {
                    return TracePath(startNode, endNode);
                }

                foreach (Tile neighbor in GetNeighbors(currentNode))
                {
                    if (!(neighbor.type == TileType.Path || neighbor.type == TileType.End) || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    if (!openSet.Contains(neighbor))
                    {
                        neighbor.hCost = GetDistance(neighbor, endNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
            return TracePath(startNode, endNode);
        }

        List<Tile> GetNeighbors(Tile targetNode)
        {
            List<Tile> neighbors = new List<Tile>();
            Vector2Int[] moveDirections =
            {
                new Vector2Int(-1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(0, 1),
                new Vector2Int(1, 0)
            };

            foreach (var item in moveDirections)
            {
                int x = targetNode.pos.x + item.x;
                int y = targetNode.pos.y + item.y;
                if (x >= 0 && x < MapLoader.Instance.mapSize.x && y >= 0 && y < MapLoader.Instance.mapSize.y)
                {
                    neighbors.Add(MapLoader.Instance.grid[x, y]);
                }
            }

            return neighbors;
        }

        IEnumerable<Vector2Int> TracePath(Tile startNode, Tile endNode)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Tile currentNode = endNode;

            while (currentNode != startNode)
            {
                if (!(currentNode.type == TileType.Path || currentNode.type == TileType.End) || !currentNode.parent)
                {
                    return null;
                }
                Vector3 node = currentNode.transform.position + Vector3.one / 2;
                path.Add(Vector2Int.FloorToInt(new Vector2(node.x, node.z)));
                currentNode = currentNode.parent;
            }
            Vector3 start = startNode.transform.position + Vector3.one / 2;
            path.Add(Vector2Int.FloorToInt(new Vector2(start.x, start.z)));

            path.Reverse();

            return path;
        }

        int GetDistance(Tile startNode, Tile targetNode)
        {
            int disX = Mathf.Abs(startNode.pos.x - targetNode.pos.x);
            int disY = Mathf.Abs(startNode.pos.y - targetNode.pos.y);

            if (disX > disY)
            {
                return 14 * disY + 10 * (disX - disY);
            }
            return 14 * disX + 10 * (disY - disX);
        }
    }
}
