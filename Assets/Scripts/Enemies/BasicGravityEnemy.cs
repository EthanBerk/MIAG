using System;
using System.Linq;
using Controllers;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using PathFinding;

namespace Enemies
{
    [RequireComponent(typeof(BoxCollisionController2D))]
    public class BasicGravityEnemy : GravityEnemy
    {
        public GameObject goal;
        public bool _visualize;
        public float speed;

        public override void Start()
        {
            base.Start();
            PathFindingGrid.UpdateNodes();
            startNode = PathFindingGrid.WorldPosToNode(transform.position);
            endNode = PathFindingGrid.WorldPosToNode(goal.transform.position);
            m_Path = PathFinder.GeneratePath(startNode, endNode, this);
            // if (!m_Path.IsEmpty)
            // {
            //     m_Path.Visualize(Mathf.Infinity, dropHeight * cellSize);
            // }
            // else
            // {
            //     print("empty");
            // }

        }

        private Path m_Path;
        private bool dooingPath;
        private bool jumping;
        private Node startNode;
        private Node endNode;


        public override void Update()
        {
            PathFindingGrid.UpdateNodes();
            
            
            startNode = PathFindingGrid.WorldPosToNode(transform.position);
            endNode = PathFindingGrid.WorldPosToNode(goal.transform.position);
            
            
            if (m_BoxCollisionController2D.Collisions.Below)
            {
                m_Path = PathFinder.GeneratePath(startNode, endNode, this);
            }
            
            if (!m_Path.IsEmpty)
            {
                m_Path.Visualize(Time.deltaTime, dropHeight * cellSize);
            }
            else
            {
                print("empty");
            }
            base.Update();
            
            
            if (m_Path.PathNodes.Count <= 1) return;
            if (!m_BoxCollisionController2D.Collisions.Below && velocity.y != 0) return;
            if (Mathf.Abs(m_Path.GetNextNode().col - startNode.col) > 1)
            {
                velocity = Math.Abs(transform.position.x - startNode.CenterWorldPos.x) < 0.1
                    ? jumpTo(m_Path.GetNextNode().CenterWorldPos)
                    : new Vector2((startNode.CenterWorldPos.x - transform.position.x > 0 ? 1 : -1) * speed, velocity.y);
            }
            else
            {
                        
                velocity = new Vector2((m_Path.GetNextNode().col - startNode.col) * speed, velocity.y);  
            }






        }
        private void OnDrawGizmos()
        {
        
            if (PathFindingGrid != null && _visualize)
            {
                var nodeArray = PathFindingGrid.NodeArray;
                for (int row = 0; row < nodeArray.GetLength(0); row++)
                {
                    for (int col = 0; col < nodeArray.GetLength(1); col++)
                    {
                        var currentNode = nodeArray[row, col];
                        switch (currentNode.NodeState)
                        {
                            case Node.State.Open:
                                Gizmos.color = new Color(1, 1, 0, 0.2f);
                                break;
                            case Node.State.Solid:
                                Gizmos.color = new Color(0, 0, 0, 0.5f);
                                break;
                            case Node.State.Goal:
                                Gizmos.color = new Color(0, 0, 1, 0.5f);
                                break;
                            case Node.State.Closed:
                                Gizmos.color = new Color(0.1f, 0.7f, 0.7f, 0.5f);
                                break;
                            case Node.State.Start:
                                Gizmos.color = new Color(0, 0.5f, 0, 0.5f);
                                break;
                        }
        
                        Gizmos.DrawCube(currentNode.CenterWorldPos, new Vector3(cellSize, cellSize, 1));
                    }
                }
        
            }
        }
    }
}