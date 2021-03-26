using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace PathFinding
{
    public class PathFindingGrid
    {
        
        public Node[,] NodeArray { get; private set; }
        private float CellSize { get; set; }
        private readonly Vector2 _origin;

        private readonly LayerMask _collisionsMask;

        public PathFindingGrid(LayerMask collisionsMask, int width, int height, float cellSize)
        {
            _origin = new Vector2(0, height * cellSize);
            CellSize = cellSize;
            NodeArray = new Node[width, height];
            _collisionsMask = collisionsMask;

        }

        public PathFindingGrid(LayerMask collisionsMask, int width, int height, float cellSize, Vector2 origin)
        {
            _origin = new Vector2(origin.x, (height * cellSize) + origin.y); 
            CellSize = cellSize;
            NodeArray = new Node[width, height];
            _collisionsMask = collisionsMask;
        }

        public void CreateGrid()
        {
            for (int row = 0; row < NodeArray.GetLength(0); row++)
            {
                for (int col = 0; col < NodeArray.GetLength(1); col++)
                {
                    NodeArray[row, col] = new Node(CellSize);
                    var currentNode = NodeArray[row, col];
                    currentNode.WorldPos = new Vector2(_origin.x + (col * CellSize), _origin.y - (row * CellSize));
                    currentNode.CenterWorldPos = currentNode.WorldPos + new Vector2(CellSize / 2, CellSize / -2);
                    currentNode.GridPos = new Vector2(col, row);
                   
                }
            }
            
        }

        public void UpdateNodes()
        {
            for (int row = 0; row < NodeArray.GetLength(0); row++)
            {
                for (int col = 0; col < NodeArray.GetLength(1); col++)
                {
                    NodeArray[row, col].UpdateNodeState(_collisionsMask);
                }
            }
        }

        public void Visualize(float time)
        {
            for (int row = 0; row < NodeArray.GetLength(0); row++)
            {
                var startPoint = NodeArray[row, 0].WorldPos;
                var endPoint = new Vector2(NodeArray[row, NodeArray.GetLength(1)-1].WorldPos.x + CellSize, NodeArray[row, 0].WorldPos.y);
                Debug.DrawLine(startPoint, endPoint, Color.blue, time);
            }
            for (int col = 0; col < NodeArray.GetLength(1); col++)
            {
                var startPoint = NodeArray[0, col].WorldPos;
                var endPoint = new Vector2(startPoint.x, NodeArray[NodeArray.GetLength(0) - 1, col].WorldPos.y - CellSize);
                Debug.DrawLine(startPoint, endPoint, Color.red, time);
            }
            var startNode = NodeArray[NodeArray.GetLength(0) - 1, 0];
            var start = new Vector2(startNode.WorldPos.x, startNode.WorldPos.y - CellSize);
            var endNode = NodeArray[NodeArray.GetLength(0) - 1, NodeArray.GetLength(1) - 1];
            var end = new Vector2(endNode.WorldPos.x + CellSize, start.y);
            Debug.DrawLine(start, end, Color.blue, time);
            
            startNode = NodeArray[0, NodeArray.GetLength(1) - 1];
            start = new Vector2(startNode.WorldPos.x + CellSize, startNode.WorldPos.y);
            Debug.DrawLine(start, end, Color.red, time);

        }

        public Path GeneratePath(Node startNode, Node endNode)
        {
            var openNodes = new  List<Node>();
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
                var currentNeighborNodes = currentNode.Neighbors(NodeArray).Where(neighborNode =>
                    neighborNode.NodeState != Node.State.Solid && !closedNodes.Contains(neighborNode));

                foreach (var neighborNode in currentNeighborNodes)
                {
                    var newGCostToNeighbor =
                        currentNode.GCost + Vector2.Distance(currentNode.GridPos, neighborNode.GridPos);
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

        public Node WorldPosToNode(Vector2 pos)
        {
            for (int row = 0; row < NodeArray.GetLength(0); row++)
            {
                for (int col = 0; col < NodeArray.GetLength(1); col++)
                {
                    var currentNode = NodeArray[row, col];
                    var xPosInNode = pos.x > currentNode.WorldPos.x && pos.x < (currentNode.WorldPos.x + CellSize);
                    var yPosInNode = pos.y > (currentNode.WorldPos.y - CellSize) && pos.y < currentNode.WorldPos.y;
                    if (xPosInNode && yPosInNode)
                    {
                        return currentNode;
                    }
                   
                }
            }
            return null;
        }

    }
}