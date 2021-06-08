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
                
            velocity.y = (endPoint.y - startPosition.y + 0.5f * Mathf.Abs(gravity_) * Mathf.Pow(time, 2)) /
                         time;

            velocity.x = (endPoint.x - startPosition.x) / time;
            return velocity;

        }
        public void DrawParabola(Vector2 start, Vector2 end, Vector2 mid, float iteration, float time)
        {
            var matrix = Matrix<float>.Build.DenseOfArray(new float[,]
            {
                {Mathf.Pow(start.x, 2f), start.x, 1},
                {Mathf.Pow(mid.x, 2f), mid.x, 1},
                {Mathf.Pow(end.x, 2f), end.x, 1}
            });
            var yValues = Matrix<float>.Build.DenseOfArray(new float[,]
            {
                {start.y},
                {mid.y},
                {end.y}
            });
            var finalValues = matrix.Inverse() * yValues;
            var a = finalValues[0, 0];
            var b = finalValues[1, 0];
            var c = finalValues[2, 0];


            for (var x = start.x; x < end.x; x += iteration)
            {
                var startPoint = new Vector2(x, (a * Mathf.Pow(x, 2f)) + (b * x) + c);
                var endPoint = new Vector2((x + iteration),
                    (a * Mathf.Pow((x + iteration), 2f)) + (b * (x + iteration)) + c);
                Debug.DrawLine(startPoint, endPoint, Color.black, time);
            }

            Debug.DrawLine(start, end, Color.blue, time);
            // print("a :" + a + " b :" + b + " c :" + c);
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