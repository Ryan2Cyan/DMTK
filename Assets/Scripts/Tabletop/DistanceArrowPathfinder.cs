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
            
            var openSet = new List<PathfindingNode> { startNode };
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

                if (current.Coordinate == endNode.Coordinate)
                {
                    // return retraced path:
                    var path = new List<Vector2Int>();
                    endNode.Parent = current.Parent;
                    
                    var currentNode = endNode;
                    while (currentNode.Coordinate != startNode.Coordinate)
                    {
                        path.Add(currentNode.Coordinate);
                        currentNode = currentNode.Parent;
                    }
                    path.Add(start);
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
                    var newCostToNeighbour = current.GCost + CalculateNodeDistance(current.Coordinate, node.Coordinate);

                    var openSetContainsNeighbour = openSet.Contains(node);
                    if (newCostToNeighbour >= node.GCost && openSetContainsNeighbour) continue;
                    node.GCost = newCostToNeighbour;
                    node.HCost = CalculateNodeDistance(node.Coordinate, endNode.Coordinate);
                    node.Parent = current;
                    if(!openSetContainsNeighbour) openSet.Add(node);
                }
            }
            return null;
        }

        /// <summary>
        /// Calculates the distance between two node coordinates. Horizontal and vertical movements are 10 per node,
        /// whereas diagonal movements are 10 * sqrt(2).
        /// </summary>
        /// <param name="node1">First node coordinates.</param>
        /// <param name="node2">Second node coordinates.</param>
        /// <returns>Distance between the two nodes.</returns>
        private static int CalculateNodeDistance(Vector2Int node1, Vector2Int node2)
        {
            var difference = new Vector2Int(Mathf.Abs(node1.x - node2.x), Mathf.Abs(node1.y - node2.y));
            if (difference.x != 0 && difference.y != 0) return (int)(difference.x * Mathf.Sqrt(2) * 10 + difference.y * Mathf.Sqrt(2) * 10);
            return difference.x * 10 + difference.y * 10;
        }
    }
}
