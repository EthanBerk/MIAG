using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace PathFinding
{
    public class Node
    {
        public int row { get; set; }
        public int col { get; set; }
        public Vector2 WorldPos { get; set; }
        public Vector2 CenterWorldPos { get; set; }
        public float FCost { get; private set; }
        public float GCost { get; private set; } = 0;
        private float HCost { get; set; } = 0;
        public Node Parent { get; private set; }
        
        public Node[,] nodeArray { get; set; }

        public enum State
        {
            Open,
            Solid,
            Goal,
            Start
        }
        public State NodeState = State.Open;
        public float _cellSize { get; set; }
        public void UpdateNodeState( LayerMask colissionMask)
        {
            var hit = Physics2D.OverlapBox(CenterWorldPos, new Vector2(_cellSize - 0.1f, _cellSize - 0.1f), 0, colissionMask);
            if (NodeState == State.Goal || NodeState == State.Start)
            {
            }
            else if (hit)
            {
                NodeState = State.Solid;
            }
            else
            {
                NodeState = State.Open;
            }
        }
        public List<Node> GetDirectNeighbors()
        {
            
            var nodeCol = col;
            var nodeRow = row;
            var neighborsList = new List<Node>();
            for (var col = -1; col <= 1; col++)
            {
                for (var row = -1; row <= 1; row++)
                {
                    var neighborRow = nodeRow + row;
                    var neighborCol = nodeCol + col;
                    
                    if ((neighborCol >= 0 && neighborCol <= nodeArray.GetLength(1) - 1) && neighborRow >= 0 && neighborRow <= nodeArray.GetLength(0) - 1)
                    {
                        neighborsList.Add(nodeArray[neighborRow, neighborCol]);
                        
                    }
                }
            }
            return neighborsList;
        }

        

        public void SetParentAndFCost(Node startingNode, Node endNode, Node parentNode)
        {
            HCost = Vector2.Distance(new Vector2(col , row), new Vector2(endNode.col, endNode.row));
            GCost = parentNode.GCost + Vector2.Distance(new Vector2(col , row), new Vector2(parentNode.col, row));
            Parent = parentNode;
            
            FCost = HCost + GCost;
        }
    }
}