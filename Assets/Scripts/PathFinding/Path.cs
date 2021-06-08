using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class Path
    {
        public List<Node> PathNodes { get; set; } = new List<Node>();
        public bool IsEmpty { get; set; } 

        public Path(Node lastNode)
        {
            CreatePath(lastNode);
        }

        private Path()
        {
            
        }

        private void CreatePath(Node node)
        {
            while (true)
            {
                PathNodes.Add(node);
                if (node.Parent == null)
                {
                    return;
                }

                if (node.NodeState == Node.State.Start)
                {
                    return;
                }

                node = node.Parent;
            }
        }

        public void Visualize(float time)
        {
            if (IsEmpty) return;
            for (var i = 0; i < PathNodes.Count -1; i++)
            {
                Debug.DrawLine(PathNodes[i].CenterWorldPos, PathNodes[i+1].CenterWorldPos, UnityEngine.Color.blue, time);
            }
        }

        public static Path Empty()
        {
            return new Path(){IsEmpty = true};
        }

        public Node GetNextNode()
        {
            return PathNodes[1];
        }

        public bool NextNodeJump()
        {
            return (PathNodes[1].col - PathNodes[0].col > 1) || (PathNodes[1].row - PathNodes[0].row > 1);
        }
    }
}