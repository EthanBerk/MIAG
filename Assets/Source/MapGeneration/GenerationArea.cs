using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.MapGeneration
{
    [Serializable]
    public class GenerationArea : ScriptableObject
    {
        [SerializeField]        
        private List<bool> area = new List<bool>(16);
        
        private int Cols
        {
            get => Cols;
            set
            {
                Cols = value;
                area = new List<bool>(Rows * Cols);
            } 
        }
        
        private int Rows
        {
            get => Rows;
            set
            {
                Rows = value;
                area = new List<bool>(Rows * Cols);
            } 
            
        }
        
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

    }
}