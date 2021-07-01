using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapGeneration.Office
{
    [CustomEditor(typeof(Office))]
    [CanEditMultipleObjects]
    public class OfficeEditor : Editor
    {
        private int tab = 0;
        private int currentTab = 0;
        
         
        private Office office;

        private void OnEnable()
        { 
            office = (Office) target; 
            
           
        }

        public override void OnInspectorGUI()
        {
            
            
            


            if (GUILayout.Button("Generate random table"))
            {
                
                office.RandomizeSeed();
                office.OnGenerate();
            }

            tab = GUILayout.Toolbar(tab, new[] {"table", "item"});

            switch (tab)
            {
                case 0:
                    office.tablePrefab = (GameObject) EditorGUILayout.ObjectField("Table prefab", office.tablePrefab, typeof(GameObject), true);
                    office.MaxLengthOfTable = EditorGUILayout.IntField("Max length of table", office.MaxLengthOfTable);
                    office.LengthOfTableCurve = EditorGUILayout.CurveField("Length of table curve", office.LengthOfTableCurve, Color.blue,
                    new Rect(0, 0, 1, office.MaxLengthOfTable));
                    
                    break;
                case 1:
                    
                    office.itemPrefab = (GameObject) EditorGUILayout.ObjectField("item prefab", office.itemPrefab, typeof(GameObject), true);
                    office.MaxItemsPerTable = EditorGUILayout.IntField("Max items per table", office.MaxItemsPerTable);
                    office.AmountOfItemsCurve = EditorGUILayout.CurveField("Amount of items curve", office.AmountOfItemsCurve, Color.blue,
                        new Rect(0, 0, 1, office.MaxItemsPerTable));
                    if (GUILayout.Button("Add Color"))
                    {
                        office.ListOfProbs.Add(0);
                        office.ListOfColors.Add(Color.black);
                    }
                    for (int i = 0; i < office.ListOfProbs.Count; i++)
                    {
                        
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Color: " + i, GUILayout.ExpandWidth(false));
                        office.ListOfProbs[i] = EditorGUILayout.Slider( office.ListOfProbs[i], 0, 1, GUILayout.ExpandWidth(false));
                        office.ListOfColors[i] = EditorGUILayout.ColorField(office.ListOfColors[i], GUILayout.Width(90));
                        if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
                        {
                            office.ListOfProbs.RemoveAt(i);
                            office.ListOfColors.RemoveAt(i);
                            i--;
                            continue;
                        }
                        GUILayout.EndHorizontal();

                    }
                    break;
            }
            if (tab != currentTab)
            {
                GUI.FocusControl(null);
                currentTab = tab;
            }
            if (office.UpdateGuis != null)
            {
                foreach (var updateGui in office.UpdateGuis)
                {
                    updateGui.updateItem();
                }
            }
        }
    }
}