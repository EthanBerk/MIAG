using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using MathNet.Numerics.LinearAlgebra;

namespace PathFinding
{
    public class PathFinder
    {
        private readonly PathFindingGrid m_PathFindingGrid;

        public PathFinder(ref PathFindingGrid pathFindingGrid)
        {
            m_PathFindingGrid = pathFindingGrid;
        }

        public Path GeneratePath(Node startNode, Node endNode, IPathfinder pathfinder)
        {
            var openNodes = new List<Node>();
            var closedNodes = new List<Node>();
            openNodes.Add(startNode);
            while (openNodes.Any())
            {
                var currentNode = openNodes.OrderBy(node => node.FCost).First();
                openNodes.Remove(currentNode);

                closedNodes.Add(currentNode);
                if (currentNode == endNode)
                {
                    return new Path(currentNode);
                }

                
                

                foreach (var neighborNode in GetNeighbors(pathfinder, currentNode))
                {
                    var newGCostToNeighbor =
                        currentNode.GCost + Vector2.Distance(new Vector2(currentNode.col, currentNode.row), new Vector2(neighborNode.col, neighborNode.row));
                    if (!(newGCostToNeighbor < neighborNode.GCost) && openNodes.Contains(neighborNode)) continue;
                    neighborNode.SetParentAndFCost(startNode, endNode, currentNode);
                    if (!openNodes.Contains(neighborNode))
                    {
                        openNodes.Add(neighborNode);
                    }
                }
            }

            return Path.Empty();
        }


        private IEnumerable<Node> GetNeighbors(IPathfinder pathfinder, Node node)
        {
            var Neighbors = new List<Node>();
            if (!pathfinder.Gravity)
            {
            }
            else
            {
                Neighbors.AddRange(node.GetDirectNeighbors()
                    .Where(neighbor => m_PathFindingGrid.NodeArray[neighbor.row - 1, neighbor.col].NodeState ==
                                       Node.State.Solid));
                var possibleJumps = new List<Node>();
                for (var Direction = -1; Direction < 1; Direction += 2)
                {
                    for (var height = pathfinder.jumpHeight; height > 1; height--)
                    {
                        for (var width = pathfinder.jumpWidth; width > 1; width--)
                        {
                            var CurrentNode = m_PathFindingGrid.NodeArray[height * Direction + node.row,
                                width * Direction + node.row];
                            if (CurrentNode.NodeState == Node.State.Solid) continue;
                            if (m_PathFindingGrid.NodeArray[CurrentNode.row - 1, CurrentNode.col].NodeState !=
                                Node.State.Solid) continue;
                            possibleJumps.Add(CurrentNode);
                        }
                    }
                }
                TestPossibleJumps(ref possibleJumps, node, pathfinder.Size);
                Neighbors.AddRange(possibleJumps);
            }

            return Neighbors;
        }

        private void TestPossibleJumps(ref List<Node> possibleJumps, Node currentNode, Vector2 size)
        {
            foreach (var jump in possibleJumps)
            {
                var startingPoint = new Vector2(currentNode.col, currentNode.row);
                var apex = new Vector2((jump.col - currentNode.col) / 2 + currentNode.col, jump.row + 1);
                var endPoint = new Vector2(currentNode.row, currentNode.col);

                var matrix = Matrix<double>.Build.DenseOfArray(new double[3, 3]
                {
                    {Math.Pow(startingPoint.x, 2d), startingPoint.x, 1},
                    {Math.Pow(apex.x, 2d), apex.x, 1},
                    {Math.Pow(endPoint.x, 2d), endPoint.x, 1}
                });
                var yValues = Matrix<double>.Build.DenseOfArray(new double[3, 1]
                {
                    {startingPoint.y},
                    {apex.y},
                    {endPoint.y}
                });
                var finalValues = matrix.Inverse() * yValues;
                var a = finalValues[0, 0];
                var b = finalValues[1, 0];
                var c = finalValues[2, 0];
                for (var yLevel = currentNode.row; yLevel <= apex.y; yLevel++)
                {
                    var val = (-b - Math.Sqrt(Math.Pow(b, 2d) - (4d * a * (c - yLevel)))) / 2d * a;
                    var val2 = (-b + Math.Sqrt(Math.Pow(b, 2d) - (4d * a * (c - yLevel)))) / 2d * a;

                    if (jump.col > startingPoint.x)
                    {
                        if (val < jump.col)
                        {
                            if (CheckBoxForCollisionFromBottomLeft(val, yLevel, size))
                            {
                                possibleJumps.Remove(jump);
                                break;
                            }
                        }

                        if (!(val2 < jump.col)) continue;
                        if (!CheckBoxForCollisionFromBottomLeft(val2, yLevel, size)) continue;
                        possibleJumps.Remove(jump);
                        break;
                    }

                    if (val > jump.col)
                    {
                        if (CheckBoxForCollisionFromBottomLeft(val, yLevel, size))
                        {
                            possibleJumps.Remove(jump);
                            break;
                        }
                    }

                    if (!(val2 > jump.col)) continue;
                    if (!CheckBoxForCollisionFromBottomLeft(val2, yLevel, size)) continue;
                    possibleJumps.Remove(jump);
                    break;
                }
            }
        }

        private bool CheckBoxForCollisionFromBottomLeft(double x, int y, Vector2 size)
        {
            for (var i = (int) Math.Ceiling(x + size.x); i > x; i--)
            {
                if (m_PathFindingGrid.NodeArray[y, i].NodeState == Node.State.Solid)
                {
                    return true;
                }
            }

            for (var i = y + (int) size.y; i > y; i--)
            {
                if (m_PathFindingGrid.NodeArray[i, (int) Math.Ceiling(x)].NodeState == Node.State.Solid)
                {
                    return true;
                }
            }

            return false;
        }
    }
}