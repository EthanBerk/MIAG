using System;
using UnityEngine;
using PathFinding;
using UnityEngine.PlayerLoop;

namespace Enemies
{
    public class Drone : MonoBehaviour
    {
        private PathFindingGrid _pathFindingGrid;
        public float _cellSize;
        public int height;
        public int width;
        public LayerMask layerMask;

        public bool _visualize;

        public GameObject target;

        private void Start()
        {
            _pathFindingGrid = new PathFindingGrid(layerMask, width, height, _cellSize);
            _pathFindingGrid.CreateGrid();
        }

        private void Update()
        {
            if (_visualize)
            {
                _pathFindingGrid.Visualize(Time.deltaTime);
            }

            _pathFindingGrid.UpdateNodes();
            var targetNode = _pathFindingGrid.WorldPosToNode(target.transform.position);
            var startNode = _pathFindingGrid.WorldPosToNode(transform.position);
            if (targetNode != null && startNode != null)
            {
                var path = _pathFindingGrid.GeneratePath(startNode, targetNode);
                path.Visualize(Time.deltaTime);
            }

        }

        private void OnDrawGizmos()
        {

            if (_pathFindingGrid != null && _visualize)
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

                        Gizmos.DrawCube(currentNode.CenterWorldPos, new Vector3(_cellSize, _cellSize, 1));
                    }
                }

            }
        }
    }
}