using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace MapGeneration.Office
{
    public class Item : MonoBehaviour, IUpdateGui
    {
        private Office m_Office;
        private Table m_Table;
        private int m_Index;
        private SpriteRenderer SpriteRenderer;
        private float m_random;
        

        public void InitializeItem(Office office, Table table, int index)
        {
            m_Office = office;
            m_Table = table;
            m_Index = index;
            m_random = table.itemRandoms[index];
            SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            
               
            
            
            updateItem();

        }

        public void updateItem()
        {
            var tableLocalScale = m_Table.transform.localScale;
            // ReSharper disable once Unity.InefficientPropertyAccess
            var tablePosition = m_Table.transform.position;
            var x = tablePosition.x - tableLocalScale.x/2 +
                    tableLocalScale.x / (m_Table.itemCount + 1) * (m_Index + 1);
            var y = tablePosition.y + tableLocalScale.y / 2 + transform.localScale.y / 2;
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.position = new Vector2(x, y);
            var choice = GenerationUtils.ChoseItmGivenProbs(m_random, m_Office.ListOfProbs.ToArray());
            if (choice == -1)
            {
                SpriteRenderer.color = Color.black;
            }
            else
            {
                SpriteRenderer.color = m_Office.ListOfColors[choice];
            }
             
              
  
        }
        
    }
}