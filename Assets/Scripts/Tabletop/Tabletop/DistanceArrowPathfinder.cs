using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tabletop.Tabletop
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
        
        public PathfindingNode Parent;
        public Vector2Int Coordinate;
        public int GCost;
        public int HCost;
        public int FCost;
    }
    
    public static class DistanceArrowPathfinder
    {
        /// <summary>Finds shortest path between two tabletop cells./// </summary>
        /// <param name="start">Starting cell.</param>
        /// <param name="end">Destination cell.</param>
        /// <param name="grid">2D grid of tabletop cells.</param>
        /// <returns>Path of tabletop cells from specified start to end cells.</returns>
        /// <remarks>Sources: https://github.com/SebLague/Pathfinding/tree/master/Episode%2003%20-%20astar/Assets/Scripts</remarks>
        public static List<TabletopCell> AStarPathfinder(TabletopCell start, TabletopCell end, List<List<TabletopCell>> grid)
        {
            var startNode = new PathfindingNode(start.Coordinate);
            var endNode = new PathfindingNode(end.Coordinate);
            var openSet = new List<PathfindingNode> { startNode };
            var closedSet = new HashSet<PathfindingNode>();

            while (openSet.Count > 0)
            {
                var current = openSet.First();
                
                // Check open set for the node with the lowest F-Cost and H-Cost: 
                foreach (var node in openSet)
                {
                    if (node.FCost > current.FCost) continue;
                    if(node.HCost < current.HCost) current = node;
                }
                openSet.Remove(current);
                closedSet.Add(current);

                // If current node is the end node return the path:
                if (current.Coordinate == endNode.Coordinate)
                {
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
                foreach (var neighbourCoordinate in Tabletop.GetNeighbouringCellCoordinates(current.Coordinate, -1, 1, -1, 1 ))
                {
                    // If occupied, skip:
                    var neighbourCell = grid[neighbourCoordinate.x][neighbourCoordinate.y];
                    if(neighbourCell.IsOccupied) continue;
                    
                    var neighbourPathfinderNode = new PathfindingNode(neighbourCoordinate);
                    if (closedSet.Contains(neighbourPathfinderNode)) continue;
                    
                    // Calculate GCost (distance from start), check if neighbour is already in open list:
                    var neighbourGCost = current.GCost + Tabletop.CalculateCellDistance(current.Coordinate, neighbourPathfinderNode.Coordinate);
                    var openSetContainsNeighbour = openSet.Contains(neighbourPathfinderNode);
                    if (neighbourGCost >= neighbourPathfinderNode.GCost && openSetContainsNeighbour) continue;
                    
                    // Assign GCost, HCost (distance from end), and FCost:
                    neighbourPathfinderNode.GCost = neighbourGCost;
                    neighbourPathfinderNode.HCost = Tabletop.CalculateCellDistance(neighbourPathfinderNode.Coordinate, endNode.Coordinate);
                    neighbourPathfinderNode.FCost = neighbourPathfinderNode.GCost + neighbourPathfinderNode.HCost;
                    neighbourPathfinderNode.Parent = current;
                    
                    // Add to open set:
                    openSet.Add(neighbourPathfinderNode);
                }
            }
            return null;
        }
    }
}
