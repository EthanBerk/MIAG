using System;
using System.Collections.Generic;
using Source.MapGeneration;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.MapGenerationWindows
{
    [CustomEditor(typeof(GenerationObject))]
    public class GenerationObjectEditor : UnityEditor.Editor
    {
        private SerializedProperty m_GenerationAreas;
        private SerializedProperty m_GenerationAreasLength;
        
        
        private GenerationObject m_GenerationObject;

        private Sprite sprite;
        private SerializedProperty test;
        private GenerationArea genArea;
        private string tag;
        
        private void OnEnable()
        {
            m_GenerationObject = target as GenerationObject;
            sprite = m_GenerationObject.gameObject.GetComponent<SpriteRenderer>().sprite;
            
            
            

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            m_GenerationAreas = serializedObject.FindProperty("GenerationAreas");
            m_GenerationAreasLength = serializedObject.FindProperty("AmountOfAreas");
            test = serializedObject.FindProperty("test");
            if (m_GenerationAreas.arraySize != m_GenerationAreasLength.intValue)
            {
                var isAdd = m_GenerationAreas.arraySize < m_GenerationAreasLength.intValue;
            
            
                for (int i = 0; i < (isAdd ? m_GenerationAreasLength.intValue : m_GenerationAreas.arraySize); i++)
                {
                    if (i >= m_GenerationAreas.arraySize && isAdd)
                    
                    {
                        m_GenerationAreas.InsertArrayElementAtIndex(i);
                    }
                    if (i >= m_GenerationAreasLength.intValue && !isAdd)
                    {
                        m_GenerationAreas.DeleteArrayElementAtIndex(i);
                    }
                }
            }
            
            EditorGUILayout.PropertyField(m_GenerationAreasLength);
            EditorGUILayout.PropertyField(test);
            
            
            
            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            
        }
    }
}