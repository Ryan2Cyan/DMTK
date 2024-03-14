using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tabletop
{
    public class PathfindingNode
    {
        public PathfindingNode(Vector2Int coordinate, bool occupied)
        {
            Coordinate = coordinate;
            GCost = 0;
            HCost = 0;
            FCost = 0;
            Occupied = occupied;
        }
        
        public PathfindingNode Parent;
        public Vector2Int Coordinate;
        public int GCost;
        public int HCost;
        public int FCost;
        public readonly bool Occupied;
    }
    
    public static class DistanceArrowPathfinder
    {
        private const int _diagonalMoveValue = 14;
        private const int _nonDiagonalMoveValue = 10;

        public static List<TabletopCell> AStarPathfinder(TabletopCell start, TabletopCell end, Vector2Int gridSize, List<List<TabletopCell>> grid)
        {
            var startNode = new PathfindingNode(start.Coordinate, start.IsOccupied);
            var endNode = new PathfindingNode(end.Coordinate, end.IsOccupied);
            var openSet = new List<PathfindingNode> { startNode };
            var closedSet = new HashSet<PathfindingNode>();

            while (openSet.Count > 0)
            {
                if (openSet.Count > 1000) return new List<TabletopCell>();
                var current = openSet.First();
                
                // Check all open set nodes for the node with the lowest F-Cost and H-Cost: 
                foreach (var node in openSet)
                {
                    if (node.FCost > current.FCost) continue;
                    if(node.HCost < current.HCost) current = node;
                }
                openSet.Remove(current);
                closedSet.Add(current);

                // If the current node is the end node return the path:
                if (current.Coordinate == endNode.Coordinate)
                {
                    // return retraced path:
                    var path = new List<TabletopCell>();
                    endNode.Parent = current.Parent;
                    
                    var currentNode = endNode;
                    while (currentNode.Coordinate != startNode.Coordinate)
                    {
                        path.Add(grid[currentNode.Coordinate.x][currentNode.Coordinate.y]);
                        currentNode = currentNode.Parent;
                    }
                    path.Add(start);
                    path.Reverse();
                    return path;
                }
                
                // Get neighbors for the current cell:
                var neighbours = new List<PathfindingNode>();
                for (var x = -1; x <= 1; x++) {
                    for (var y = -1; y <= 1; y++) {
                        if (x == 0 && y == 0)
                            continue;

                        var check = new Vector2Int(current.Coordinate.x + x, current.Coordinate.y + y);
                        
                        if (check.x < 0 || check.x >= gridSize.x || check.y < 0 || check.y >= gridSize.y) continue;
                        if (grid[check.x][check.y].IsOccupied) continue;
                        neighbours.Add(new PathfindingNode(check, false));
                    }
                }

                // Calculate the GCost, HCost, and FCost for each neighbour:
                foreach (var node in neighbours)
                {
                    if (node.Occupied || closedSet.Contains(node)) continue;
                    var newCostToNeighbour = current.GCost + CalculateNodeDistance(current.Coordinate, node.Coordinate);

                    var openSetContainsNeighbour = openSet.Contains(node);
                    if (newCostToNeighbour >= node.GCost && openSetContainsNeighbour) continue;
                    node.GCost = newCostToNeighbour;
                    node.HCost = CalculateNodeDistance(node.Coordinate, endNode.Coordinate);
                    node.FCost = node.GCost + node.HCost;
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
            if (difference.x > difference.y) return difference.y * _diagonalMoveValue + (difference.x - difference.y) * _nonDiagonalMoveValue;
            return difference.x * _diagonalMoveValue + (difference.y - difference.x) * _nonDiagonalMoveValue;
        }
    }
}
