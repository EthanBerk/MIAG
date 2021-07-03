using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace MapGeneration.Office
{
    [Serializable]
    public class Office : MonoBehaviour
    {
        public int seed;
        public List<IUpdateGui> UpdateGuis;
        

        public void OnGenerate()
        {
            UpdateGuis = new List<IUpdateGui>();
            GenerateTable(Random.value);
        }
        //prefabs 
        public void RandomizeSeed()
        {
            seed = Random.Range(0, int.MaxValue);
            Random.InitState(seed);
        }
        
        
        //table

        public GameObject tablePrefab{ get; set; }

        [HideInInspector] public AnimationCurve LengthOfTableCurve { get; set; } = new AnimationCurve();
        public int MaxLengthOfTable { get; set; }
        private Table tableClass;


        private void GenerateTable(float random)
        {
            if (tableClass != null)
            {
                tableClass.destroyTable();
            }
            var table = Instantiate(tablePrefab, Vector3.zero, Quaternion.identity);
            tableClass = table.GetComponent<Table>();
            tableClass.InitializeTable(this, random);
            UpdateGuis.Add(tableClass);
        }
        
        //items 
        
        
        public GameObject itemPrefab{ get; set; }
        [HideInInspector]public AnimationCurve AmountOfItemsCurve { get; set; } = new AnimationCurve();
        public int MaxItemsPerTable{ get; set; }

        public List<Color> ListOfColors { get; set; } = new List<Color>();
        public List<float> ListOfProbs { get; set; } = new List<float>();



    }
}