using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace PathFinding
{
    public class Path
    {
        public List<Node> PathNodes { get; set; } = new List<Node>();
        public bool IsEmpty { get; set; } 

        public Path(Node lastNode, Node startNode)
        {
            CreatePath(lastNode, startNode);
        }

        private Path()
        {
            
        }

        private void CreatePath(Node node, Node startNode)
        {

            PathNodes.Add(node);
            if (node == startNode)
            {
                return;
            }

            // ReSharper disable once TailRecursiveCall
            CreatePath(node.Parent, startNode);
        }

        public void Visualize(float time)
        {
            if (IsEmpty) return;
            for (var i = 0; i < PathNodes.Count -1; i++)
            {
                Debug.DrawLine(PathNodes[i].CenterWorldPos, PathNodes[i+1].CenterWorldPos, UnityEngine.Color.blue, time);
            }
        }
        public void Visualize(float time, float jumpHight)
        {
            if (IsEmpty) return;
            for (var i = 0; i < PathNodes.Count -1; i++)
            {
                var nextNode = PathNodes[i];
                var currentNode = PathNodes[i + 1];
                if (Mathf.Abs(PathNodes[i + 1].row - PathNodes[i].row) > 1)
                {
                    var centerPoint =
                        new Vector2(
                            currentNode.CenterWorldPos.x +
                            (nextNode.CenterWorldPos.x - currentNode.CenterWorldPos.x) / 2,
                            (nextNode.CenterWorldPos.y > currentNode.CenterWorldPos.y? nextNode.CenterWorldPos.y : currentNode.CenterWorldPos.y) + jumpHight);
                    DrawingUtils.DrawParabola(currentNode.CenterWorldPos, nextNode.CenterWorldPos, centerPoint, Color.blue,  0.01f, time);
                }
                else
                {
                    Debug.DrawLine(PathNodes[i].CenterWorldPos, PathNodes[i+1].CenterWorldPos, UnityEngine.Color.blue, time);  
                }
                
            }
        }

        public static Path Empty()
        {
            return new Path(){IsEmpty = true};
        }

        public Node GetNextNode()
        {
            return PathNodes[PathNodes.Count - 2];
        }

        public bool NextNodeJump()
        {
            return (PathNodes[1].col - PathNodes[0].col > 1) || (PathNodes[1].row - PathNodes[0].row > 1);
        }
    }
}