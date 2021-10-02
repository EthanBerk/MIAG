using System;
using MathNet.Numerics.LinearAlgebra;
using UnityEditor;
using UnityEngine;

namespace Enemies
{
    public abstract class GravityEnemy : Enemy
    {
        public int jumpHeight;
        public int jumpWidth;


        public float gravity_ { get; set; } = -90;
        public float dropHeight = 2;

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            applyGravity();
            base.Update();
        }

        public Vector2 jumpTo(Vector2 endPoint)
        {
            var velocity = new Vector2();
            
            var velY = gravity_ * (Mathf.Sqrt(Mathf.Abs(2 / gravity_)));
            var startPosition = transform.position;
            var jumpHeight = endPoint.y - startPosition.y;
            var dropTime = Mathf.Sqrt(Mathf.Abs(2 * dropHeight / gravity_));
            var upTime = Mathf.Sqrt(Mathf.Abs(2 * (jumpHeight + dropHeight) / gravity_));
            var time = dropTime + upTime;

            if (endPoint.y < startPosition.y)
            {
                velocity.y = ((0) - (0.5f * gravity_ * Mathf.Pow(time, 2))) /
                             time;
            }
            else
            {
                velocity.y = (jumpHeight - (0.5f * gravity_ * Mathf.Pow(time, 2))) /
                             time;
                
            }
            

            velocity.x = (endPoint.x - startPosition.x) / time;
            return velocity;

        }

        public void applyGravity()
        {
            base.velocity.y += gravity_ * Time.deltaTime;
            if (m_BoxCollisionController2D.Collisions.Below || m_BoxCollisionController2D.Collisions.Above)
            {
                velocity.y = 0;
                velocity.x = 0;
            }
        }
        
    }
}