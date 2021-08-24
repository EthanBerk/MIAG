using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source.MapGeneration
{
    [Serializable]
    public class GenerationObject : MonoBehaviour
    {


        [SerializeField] private int AmountOfAreas;
        [SerializeField] public GenerationArea test = new GenerationArea();
        public float cellSize = 1;
        

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            test.OnDrawGizmos(Vector2.zero, 1);
        }
#endif



    }
}