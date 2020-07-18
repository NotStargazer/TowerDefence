using UnityEngine;
using System.Collections.Generic;

namespace AI
{
    //TODO: Implement IPathFinder using Dijsktra algorithm.
    public class Dijkstra : IPathFinder
    {
        private MapLoader m_MapData;

        public Dijkstra(MapLoader mapData)
        {
            m_MapData = mapData;
        }

        public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
        {
            Tile[,] grid = m_MapData.Grid;
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
                    if (openSet[i].HCost < currentNode.HCost) currentNode = openSet[i];
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == endNode)
                {
                    return TracePath(startNode, endNode);
                }

                foreach (Tile neighbor in GetNeighbors(currentNode))
                {
                    if (!(neighbor.Type == TileType.Path || neighbor.Type == TileType.End) || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    if (!openSet.Contains(neighbor))
                    {
                        neighbor.HCost = GetDistance(neighbor, endNode);
                        neighbor.Parent = currentNode;

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
                int x = targetNode.Pos.x + item.x;
                int y = targetNode.Pos.y + item.y;
                if (x >= 0 && x < m_MapData.MapSize.x && y >= 0 && y < m_MapData.MapSize.y)
                {
                    neighbors.Add(m_MapData.Grid[x, y]);
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
                if (!(currentNode.Type == TileType.Path || currentNode.Type == TileType.End) || !currentNode.Parent)
                {
                    return null;
                }
                Vector3 node = currentNode.transform.position + Vector3.one / 2;
                path.Add(Vector2Int.FloorToInt(new Vector2(node.x, node.z)));
                currentNode = currentNode.Parent;
            }
            Vector3 start = startNode.transform.position + Vector3.one / 2;
            path.Add(Vector2Int.FloorToInt(new Vector2(start.x, start.z)));

            path.Reverse();

            return path;
        }

        int GetDistance(Tile startNode, Tile targetNode)
        {
            int disX = Mathf.Abs(startNode.Pos.x - targetNode.Pos.x);
            int disY = Mathf.Abs(startNode.Pos.y - targetNode.Pos.y);

            if (disX > disY)
            {
                return 14 * disY + 10 * (disX - disY);
            }
            return 14 * disX + 10 * (disY - disX);
        }
    }
}
