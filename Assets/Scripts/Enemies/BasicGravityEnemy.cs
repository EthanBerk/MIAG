using System;
using UnityEngine;
using PathFinding;

namespace Enemies
{
    public class BasicGravityEnemy : MonoBehaviour, IPathfinder
    {
        public bool Gravity { get; set; }
        public Vector2 Size { get; set; }
        public int jumpHeight { get; set; }
        public int jumpWidth { get; set; }
        private PathFinder m_PathFinder;
        private Node currentNode;
        private Node goalNode;
        private Path Path;

        private void Update()
        {
            Path = m_PathFinder.GeneratePath(currentNode, goalNode, this);
            if (Path.NextNodeJump())
            {
                JumpToNode(Path.GetNextNode());
            }
            else
            {
                MoveToNode(Path.GetNextNode());
            }
        }

        private void JumpToNode(Node node)
        {
            //TODO
        }

        private void MoveToNode(Node node)
        {
            transform.Translate(node.WorldPos);
        }
    }
}