using System;
using Controllers;
using PathFinding;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public bool Gravity { get; set; } = true;
        public Vector2 Size = new Vector2(1,1);

        public BoxCollisionController2D m_BoxCollisionController2D;

        [HideInInspector] public Vector2 velocity = new Vector2();

        public PathFinder PathFinder { get; set; }
        public PathFindingGrid PathFindingGrid { get; set; }
        public LayerMask LayerMask;
        public int width;
        public int height;
        public float cellSize;
        public Vector2 origin;

        public virtual void Start()
        {
            m_BoxCollisionController2D = gameObject.GetComponent<BoxCollisionController2D>();
            PathFindingGrid = new PathFindingGrid(LayerMask, width, height, cellSize, origin);
            PathFindingGrid.UpdateNodes();
            PathFinder = new PathFinder(PathFindingGrid);
        }

        public virtual void Update()
        {
            applyVelocity();
        }


        public void applyVelocity()
        {
            m_BoxCollisionController2D.Move(velocity * Time.deltaTime);
        }

        

    }
}