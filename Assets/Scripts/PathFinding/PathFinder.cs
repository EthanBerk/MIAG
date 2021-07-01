using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using MathNet.Numerics.LinearAlgebra;
using Unity.Mathematics;

namespace PathFinding
{
    public class PathFinder
    {
        private PathFindingGrid m_PathFindingGrid;

        public PathFinder(PathFindingGrid pathFindingGrid)
        {
            m_PathFindingGrid = pathFindingGrid;
        }

        public Path GeneratePath(Node startNode, Node endNode, Enemy enemy)
        {
            var localNodeArray = (Node[,]) m_PathFindingGrid.NodeArray.Clone();
            var localStartNode = localNodeArray[startNode.row, startNode.col];
            var localEndNode = localNodeArray[endNode.row, endNode.col];

            var openNodes = new List<Node>();
            var closedNodes = new List<Node>();
            openNodes.Add(localStartNode);

            while (openNodes.Any())
            {
                var currentNode = openNodes.OrderBy(node => node.FCost).First();
                openNodes.Remove(currentNode);

                closedNodes.Add(currentNode);
                currentNode.NodeState = Node.State.Closed;
                if (currentNode == localEndNode)
                {
                    return new Path(currentNode, localStartNode);
                }

                foreach (var neighborNode in GetNeighbors(enemy, currentNode, localNodeArray))
                {
                    if (closedNodes.Contains(neighborNode)) continue;
                    var newGCostToNeighbor =
                        currentNode.GCost + Vector2.Distance(new Vector2(currentNode.col, currentNode.row),
                            new Vector2(neighborNode.col, neighborNode.row));
                    if (!(newGCostToNeighbor < neighborNode.GCost) && openNodes.Contains(neighborNode)) continue;
                    neighborNode.SetParentAndFCost(localStartNode, localEndNode, currentNode);
                    if (!openNodes.Contains(neighborNode))
                    {
                        openNodes.Add(neighborNode);
                    }
                }
            }

            return Path.Empty();
        }


        private IEnumerable<Node> GetNeighbors(Enemy enemy, Node node, Node[,] nodeArray)
        {
            var Neighbors = new List<Node>();
            if (!enemy.Gravity)
            {
                Neighbors.AddRange(node.GetDirectNeighbors().Where(neighbor => neighbor.NodeState != Node.State.Solid));
            }
            else
            {
                var gravityEnemy = (GravityEnemy) enemy;
                Neighbors.AddRange(node.GetDirectNeighbors()
                    .Where(neighbor => nodeArray[neighbor.row + 1, neighbor.col].NodeState ==
                        Node.State.Solid && neighbor.NodeState != Node.State.Solid));
                var possibleJumps = new List<Node>();

                for (var height = -gravityEnemy.jumpHeight; height <= gravityEnemy.jumpWidth; height++)
                {
                    for (var width = -gravityEnemy.jumpWidth; width <= gravityEnemy.jumpWidth ; width++)
                    {
                        if (width == 0 || width == 1 || width == -1 || height == 0 || height == 1 || height == -1) continue;
                        Node CurrentNode;
                        try
                        {
                            CurrentNode = nodeArray[node.row - height,
                                width + node.col];
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        if (CurrentNode.NodeState == Node.State.Solid) continue;
                        if (nodeArray[CurrentNode.row + 1, CurrentNode.col].NodeState !=
                            Node.State.Solid) continue;
                        if (nodeArray[CurrentNode.row + 1, CurrentNode.col + 1].NodeState ==
                            Node.State.Solid && nodeArray[CurrentNode.row + 1, CurrentNode.col - 1].NodeState ==
                            Node.State.Solid) continue;

                        possibleJumps.Add(CurrentNode);
                    }
                }


                Neighbors.AddRange(TestPossibleJumps(possibleJumps, node, gravityEnemy.Size,
                    (int) gravityEnemy.dropHeight, 0.25f, nodeArray));
            }


            return Neighbors;
        }


        private bool CheckBoxForCollision(double x, double y, Vector2 size)
        {
            var xMin = (int) (x - (size.x / 2));
            var xMax = (int) (x + (size.x / 2));
            var yMin = (int) (y - (size.y / 2));
            var yMax = (int) (y + (size.y / 2));
            for (var row = yMin; row <= yMax; row++)
            {
                for (var col = xMin; col <= xMax; col++)
                {
                    try
                    {
                        if (m_PathFindingGrid.NodeArray[row, col].NodeState != Node.State.Solid)
                        {
                            //m_PathFindingGrid.NodeArray[row, col].NodeState = Node.State.Goal;
                            continue;
                        }
                        else
                        {
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    return true;
                }
            }

            return false;
        }


        private List<Node> TestPossibleJumps(List<Node> possibleJumps, Node currentNode, Vector2 size,
            int dropHeight,
            float increment, Node[,] nodeArray)
        {
            var goodJumps = new List<Node>();
            if (!possibleJumps.Any()) return goodJumps;
            var startingPoint = new Vector2(currentNode.CenterBottomPos.x,
                currentNode.CenterBottomPos.y + (size.y / 2) * m_PathFindingGrid.CellSize);
            foreach (var jump in possibleJumps)
            {
                var endPoint = new Vector2(jump.CenterBottomPos.x,
                    jump.CenterBottomPos.y + (size.y / 2) * m_PathFindingGrid.CellSize);
                var apex = new Vector2(startingPoint.x + (endPoint.x - startingPoint.x) / 2,
                    (endPoint.y > startingPoint.y? endPoint.y : startingPoint.y) + dropHeight);
                if (m_PathFindingGrid.WorldPosToNode(apex).Equals(null)) break;


                var matrix = Matrix<float>.Build.DenseOfArray(new float[,]
                {
                    {Mathf.Pow(startingPoint.x, 2f), startingPoint.x, 1},
                    {Mathf.Pow(apex.x, 2f), apex.x, 1},
                    {Mathf.Pow(endPoint.x, 2f), endPoint.x, 1}
                });
                var yValues = Matrix<float>.Build.DenseOfArray(new float[,]
                {
                    {startingPoint.y},
                    {apex.y},
                    {endPoint.y}
                });
                var finalValues = matrix.Inverse() * yValues;
                var a = finalValues[0, 0];
                var b = finalValues[1, 0];
                var c = finalValues[2, 0];
                var adjIncrement = ((endPoint.x > startingPoint.x) ? 1f : -1f) * increment;
                for (var x = startingPoint.x + ((endPoint.x > startingPoint.x) ? 1f : -1f) * 0.01f;
                    Mathf.Abs(x - startingPoint.x) < Mathf.Abs(endPoint.x - startingPoint.x);
                    x += adjIncrement)
                {
                    var y = (a * Mathf.Pow(x, 2f)) + (b * x) + c;

                    // var startPoint = new Vector2(x, y);
                    // var endDot = new Vector2((x + adjIncrement),
                    //     (a * Mathf.Pow((x + adjIncrement), 2f)) + (b * (x + adjIncrement)) + c);
                    // Debug.DrawLine(startPoint, endDot, Color.red, Mathf.Infinity);
                    
                    var xNode = Mathf.Abs(m_PathFindingGrid._origin.x - x) / m_PathFindingGrid.CellSize;
                    var yNode = (Mathf.Abs(m_PathFindingGrid._origin.y - y) / m_PathFindingGrid.CellSize);
                    if (CheckBoxForCollision(xNode, yNode, size))
                    {
                        if (goodJumps.Contains(jump))
                        {
                            goodJumps.Remove(jump);
                        }

                        break;
                    }

                    if (!goodJumps.Contains(jump)) goodJumps.Add(jump);
                }


                // for (var x = startingPoint.x;
                //     Mathf.Abs(x - startingPoint.x) < Mathf.Abs(endPoint.x - startingPoint.x);
                //     x += adjIncrement)
                // {
                //     var y = (a * Mathf.Pow(x, 2f)) + (b * x) + c;
                //     var startPoint = new Vector2(x, y);
                //     var endDot = new Vector2((x + adjIncrement),
                //         (a * Mathf.Pow((x + adjIncrement), 2f)) + (b * (x + adjIncrement)) + c);
                //     Debug.DrawLine(startPoint, endDot, (goodJumps.Contains(jump)? Color.blue : Color.red), Mathf.Infinity);
                //     
                // }
            }

            return goodJumps;
        }
    }
}