using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tabletop
{
    public class PathfindingNode
    {
        public PathfindingNode(Vector2Int coordinate)
        {
            Coordinate = coordinate;
            GCost = 0;
            HCost = 0;
            FCost = 0;
        }
        
        public Vector2Int Coordinate;
        public int GCost;
        public int HCost;
        public int FCost;
        public PathfindingNode Parent;
    }
    
    public static class DistanceArrowPathfinder
    {
        public static List<Vector2Int> AStarPathfinder(Vector2Int start, Vector2Int end, Vector2Int gridSize)
        {
            var startNode = new PathfindingNode(start);
            var endNode = new PathfindingNode(end);
            
            var openSet = new List<PathfindingNode> { new (start) };
            var closedSet = new HashSet<PathfindingNode>();

            while (openSet.Count > 0)
            {
                var current = openSet.First();
                foreach (var node in openSet)
                {
                    if (node.FCost > current.FCost) continue;
                    if(node.HCost < current.HCost) current = node;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                if (current == endNode)
                {
                    // return retraced path:
                    var path = new List<Vector2Int>();
                    var currentNode = endNode;
                    while (currentNode != startNode)
                    {
                        path.Add(currentNode.Coordinate);
                        currentNode = currentNode.Parent;
                    }
                    path.Reverse();
                    return path;
                }
                
                // Get neighbors:
                var neighbours = new List<PathfindingNode>();
                for (var x = -1; x <= 1; x++) {
                    for (var y = -1; y <= 1; y++) {
                        if (x == 0 && y == 0)
                            continue;

                        var check = new Vector2Int(current.Coordinate.x + x, current.Coordinate.y + y);

                        if (check.x >= 0 && check.x < gridSize.x && check.y >= 0 && check.y < gridSize.y) 
                        {
                            neighbours.Add(new PathfindingNode(check));
                        }
                    }
                }

                foreach (var node in neighbours)
                {
                    if (closedSet.Contains(node)) continue;
                    var newCostToNeighbour = current.GCost + (int)Vector2Int.Distance(current.Coordinate, node.Coordinate);

                    var openSetContainsNeighbour = openSet.Contains(node);
                    if (newCostToNeighbour >= node.GCost && openSetContainsNeighbour) continue;
                    node.GCost = newCostToNeighbour;
                    node.HCost = (int)Vector2Int.Distance(node.Coordinate, end);
                    node.Parent = current;
                    if(!openSetContainsNeighbour) openSet.Add(node);
                }
            }
            return null;
        }
    }
}
