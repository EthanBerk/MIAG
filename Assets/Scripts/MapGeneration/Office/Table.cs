
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapGeneration.Office
{
    public class Table : MonoBehaviour, IUpdateGui
    {
        private Office m_Office;
        private float random;
        public void InitializeTable(Office office, float iRandom)
        {
            m_Office = office;
            random = iRandom;
            transform.localScale = new Vector2(m_Office.LengthOfTableCurve.Evaluate(random), transform.localScale.y);
            GenerateItems();
        }

        

        //items
        public float[] itemRandoms { get; set; } = new float[50];
        public List<Item> Items { get; set; }
        public int itemCount { get; set; }


        private void GenerateItems()
        {
            
            Items = new List<Item>();
            for (var i = 0; i < itemRandoms.Length; i++)
            {
                itemRandoms[i] = Random.value;
            }
            itemCount = Mathf.RoundToInt(m_Office.AmountOfItemsCurve.Evaluate(random));
            for (var i = 0; i < itemCount; i++)
            {
                var item = Instantiate(m_Office.itemPrefab, Vector3.zero, Quaternion.identity);
                var itemClass = item.GetComponent<Item>();
                itemClass.InitializeItem(m_Office, this, i);
                Items.Add(itemClass);
                
            }
        
        }
        public void updateItem()
        {
            var newItemCount = Mathf.RoundToInt(m_Office.AmountOfItemsCurve.Evaluate(random));
            for (int i = 0; i < ((newItemCount > itemCount)? newItemCount : itemCount); i++)
            {
                if (i < newItemCount && i >= itemCount)
                {
                    var item = Instantiate(m_Office.itemPrefab, Vector3.zero, Quaternion.identity);
                    var itemClass = item.GetComponent<Item>();
                    itemClass.InitializeItem(m_Office, this, i);
                    Items.Add(itemClass);
                    itemCount++;
                }
                else if(i >= newItemCount && i < itemCount)
                {
                    itemCount--;
                    var item = Items[i];
                    Items.Remove(item);
                    m_Office.UpdateGuis.Remove(item);
                    DestroyImmediate(item.gameObject);
                }
                else
                {
                    Items[i].updateItem();
                }

            }

            
            
            transform.localScale = new Vector2(m_Office.LengthOfTableCurve.Evaluate(random), transform.localScale.y);
        }

        private void destroyItems()
        {
            if (Items == null) return;
            foreach (var item in Items)
            {
                DestroyImmediate(item.gameObject);
            }
        }

        public void destroyTable()
        {
            destroyItems();
            DestroyImmediate(gameObject);
        }
    }
}