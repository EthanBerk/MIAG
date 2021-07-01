using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Utils
{
    public static class DrawingUtils
    {
        public static void DrawParabola(Vector2 start, Vector2 end, Vector2 mid, Color color, float iteration, float time)
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

            var direction = ((end.x > start.x)) ? 1f : -1f;
            for (var x = start.x; Mathf.Abs(x - start.x) < Mathf.Abs(end.x - start.x); x += iteration * direction)
            {
                var startPoint = new Vector2(x, (a * Mathf.Pow(x, 2f)) + (b * x) + c);
                var endPoint = new Vector2((x + iteration),
                    (a * Mathf.Pow((x + iteration), 2f)) + (b * (x + iteration)) + c);
                Debug.DrawLine(startPoint, endPoint, color, time);
            }

            // Debug.DrawLine(start, end, Color.blue, time);
            // print("a :" + a + " b :" + b + " c :" + c);
            
        }
    }
}