using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Objects
{
    [Serializable]
    public class Serializable2DArray<T>
    {
        public T[] area { get; set; }
        
        public int Cols { get; set; }
        public int Rows { get; set; }

        public Serializable2DArray(int rows, int cols)
        {

            area = new T[rows * cols];
            
            Rows = rows;
            Cols = cols;
        }

        


        public T this[int row, int col]
        {
            get
            {
                if (row * Cols + col >= area.Length || row * Cols + col < 0)
                {
                    throw new IndexOutOfRangeException("Out Of Bounds");
                }

                try
                {
                    return area[Cols * row + col];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                

                
            }
            set
            {
                if (row * Cols + col >= area.Length || row * Cols + col < 0)
                {
                    throw new IndexOutOfRangeException("Out Of Bounds");
                }

                area[Cols * row + col] = value;
            }
            
        }
        
    }
}