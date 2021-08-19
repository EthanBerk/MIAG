using Codice.Client.BaseCommands;
using Source.MapGeneration;
using UnityEditor;
using UnityEngine;

namespace Editor.MapGenerationWindows
{
    
    [CustomPropertyDrawer(typeof(GenerationArea))]
    public class GenerationAreaPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUILayout.LabelField("Test");
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}