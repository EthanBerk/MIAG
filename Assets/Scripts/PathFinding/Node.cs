using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace PathFinding
{
    public class Node
    {
        public Vector2 GridPos{ get; set; }
        public Vector2 WorldPos { get; set; }
        public Vector2 CenterWorldPos { get; set; }
        public float FCost { get; set; }
        public float GCost { get; set; } = 0;
        public float HCost { get; set; } = 0;
        public Node Parent { get; set; }

        public enum State
        {
            Open,
            Solid,
            Goal,
            Start
            
        }
        public State NodeState = State.Open;

        

        private readonly float _cellSize;

        public Node(float cellSize)
        {
            _cellSize = cellSize;
        }
        public void UpdateNodeState( LayerMask colissionMask)
        {
            var hit = Physics2D.OverlapBox(CenterWorldPos, new Vector2(_cellSize, _cellSize), 0, colissionMask);
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

        public List<Node> Neighbors(Node[,] nodeArray)
        {
            var nodeCol = (int)GridPos.x;
            var nodeRow = (int)GridPos.y;
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
            HCost = Vector2.Distance(GridPos, endNode.GridPos);
            GCost = parentNode.GCost + Vector2.Distance(GridPos, parentNode.GridPos);
            Parent = parentNode;
            
            FCost = HCost + GCost;
        }
    }
}