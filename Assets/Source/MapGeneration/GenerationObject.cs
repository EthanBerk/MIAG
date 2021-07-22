using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source.MapGeneration
{
    [Serializable]
    public class GenerationObject : MonoBehaviour
    {

        [SerializeField] private List<GenerationArea> GenerationAreas = new List<GenerationArea>();
        [SerializeField] private int AmountOfAreas;
        [SerializeField] public int test;



    }
}