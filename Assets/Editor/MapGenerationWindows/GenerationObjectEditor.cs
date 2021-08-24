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
        private SerializedProperty generationArea;
        
        private SerializedProperty cellSize;





        private Sprite sprite;
        
        private string tag;
        private bool enabeld;
        
        private void OnEnable()
        {
            Debug.Log(EditorUtility.IsPersistent(target));
            generationArea = serializedObject.FindProperty(nameof(GenerationObject.test));
            cellSize = serializedObject.FindProperty(nameof(GenerationObject.cellSize));
            
        }

        public override void OnInspectorGUI()
        {
            
        }
        

        private void OnSceneGUI()
        {
            serializedObject.Update();
            if (Event.current.type == EventType.MouseDown)
            {
                
                var mouseDown = Event.current.button == 0;
                var mousePos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                
               
                if (!(mousePos.x < 0) && !(mousePos.y < 0) && !(mousePos.x > 4 * 1) &&
                    !(mousePos.y > 4 * 1))
                {
                    var row = Mathf.FloorToInt(4 - mousePos.y);
                    var col = Mathf.FloorToInt(mousePos.x);
                    if (GenerationArea.GetValue(generationArea, row, col, 4) != mouseDown)
                    {
                        GenerationArea.EditGenerationArea(generationArea, row, col, mouseDown, 4);
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
            
        }
    }
}