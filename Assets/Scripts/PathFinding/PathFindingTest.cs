using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace PathFinding
{
    public class PathFindingTest : MonoBehaviour
    {
        private PathFindingGrid _pathFindingGrid;
        public float _cellSize;
        public int height;
        public int width;

        public Vector2 origin;

        public LayerMask collisionMask;

        public bool visulize;

        public GameObject target;

        private Node startNode;
        private Node endNode;
        private void Start()
        {
            _pathFindingGrid = new PathFindingGrid(collisionMask, width, height, _cellSize, origin);
            _pathFindingGrid.CreateGrid();
            
            _pathFindingGrid.UpdateNodes();
            
            
            endNode = _pathFindingGrid.NodeArray[9, 5];
            endNode.NodeState = Node.State.Goal;
            startNode = _pathFindingGrid.NodeArray[0, 5];
            startNode.NodeState = Node.State.Start;
            
            
            
        }
        

        private void Update()
        {
            _pathFindingGrid.Visualize(Time.deltaTime);
            
            _pathFindingGrid.UpdateNodes();
            var path = _pathFindingGrid.GeneratePath(startNode, endNode);
            path.Visualize(Time.deltaTime);
            
        }

        


        private void OnDrawGizmos()
        {
            
            if (_pathFindingGrid != null && visulize)
            {
                var nodeArray = _pathFindingGrid.NodeArray;
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
                            case Node.State.Start:
                                Gizmos.color = new Color(0, 0.5f, 0, 0.5f);
                                break;
                        }
                        Gizmos.DrawCube(currentNode.CenterWorldPos, new Vector3(_cellSize, _cellSize, 1 ));
                    }
                }
                
            }
            
        }
    }
}