using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Source.MapGeneration
{
    [Serializable]
    public class GenerationArea
    {

        [SerializeField] private bool[] area = new bool[16];

        public int test { get; set; }

        public bool TestBool;

        private int Cols = 4;

        private int Rows = 4;
        
        private bool this[int row, int col]
        {
            get
            {
                if (row >= Rows || row < 0 || col >= Cols || col < 0)
                {
                    throw new IndexOutOfRangeException("Out Of Bounds");
                }

                return area[row * Rows + col];

            }
            set
            {
                if (row >= Rows || row < 0 || col >= Cols || col < 0)
                {
                    throw new IndexOutOfRangeException("Out Of Bounds");
                }

                area[row * Rows + col] = value;
            }
            
        }

        public void OnDrawGizmos(Vector2 origin, float cellSize)
        {
            for (var row = 0; row < Rows; row++)
            {
                for (var col = 0; col < Cols; col++)
                {
                    var color = this[row, col] ? Color.blue : Color.red;
                    var BottomLeft = origin + new Vector2(col * cellSize, (Rows  -1) - row * cellSize);
                    var center = BottomLeft + new Vector2(cellSize / 2, cellSize / 2);
                    Gizmos.color = color;
                    Gizmos.DrawCube(center, new Vector3(cellSize, cellSize, 0));
                }
            }
        }

        public static void  EditGenerationArea(SerializedProperty generationArea, int row, int col, bool value, int rowAmount)
        {
            generationArea.FindPropertyRelative(nameof(area)).GetArrayElementAtIndex(row * rowAmount + col).boolValue = value;
        }
        public static bool  GetValue(SerializedProperty generationArea, int row, int col, int rowAmount)
        {
            return generationArea.FindPropertyRelative(nameof(area)).GetArrayElementAtIndex(row * rowAmount + col)
                .boolValue;
        }

    }
}