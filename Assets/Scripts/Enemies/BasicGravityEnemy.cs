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

        public override void Start()
        {
            base.Start();
            var goalPosition = goal.transform.position;
            var midPosition = new Vector2(transform.position.x + (goalPosition.x - transform.position.x) / 2,
                goalPosition.y + dropHeight);
            DrawParabola(gameObject.transform.position,goalPosition, midPosition, 0.3f, Mathf.Infinity);
            print(midPosition);
            var startNode = PathFindingGrid.WorldPosToNode(transform.position);
            var endNode = PathFindingGrid.WorldPosToNode(goal.transform.position);
            startNode.NodeState = Node.State.Start;
            endNode.NodeState = Node.State.Goal;
            print("(" +startNode.col + " ," + startNode.row +")");
            print("(" +endNode.col + " ," + endNode.row +")");
            PathFindingGrid.UpdateNodes();
            
            Path = PathFinder.GeneratePath(startNode, endNode, this);
            if (!Path.IsEmpty)
            {
                Path.Visualize(Mathf.Infinity);
            }
            else
            {
                print("empty");
            }
            PathFindingGrid.UpdateNodes();

        }

        private Path Path;
        private bool dooingPath;


        public override void Update()
        {
            
            // var startNode = PathFindingGrid.WorldPosToNode(transform.position);
            // var endNode = PathFindingGrid.WorldPosToNode(goal.transform.position);
            // startNode.NodeState = Node.State.Start;
            // endNode.NodeState = Node.State.Goal;
            // print( "start: "+ "(" +startNode.col + " ," + startNode.row +")");
            // print("end: "+ "(" +endNode.col + " ," + endNode.row +")");
            //
            //
            // Path = PathFinder.GeneratePath(startNode, endNode, this);
            // if (!Path.IsEmpty)
            // {
            //     Path.Visualize(Time.deltaTime);
            // }
            // else
            // {
            //     print("empty");
            // }







            // if (Path.PathNodes.Count > 1)
            // {
            //     velocity = new Vector2(Path.GetNextNode().CenterWorldPos.x, 0);
            //     
            // }
            
            
            


            base.Update();
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