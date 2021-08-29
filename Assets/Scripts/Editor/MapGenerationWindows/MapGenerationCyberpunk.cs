using System;
using UnityEditor;
using UnityEngine;

namespace Editor.MapGenerationWindows
{
    [CreateAssetMenu(fileName = "MapGenerationCyberpunk", menuName = "MapGeneration", order = 10)]
    [Serializable]
    public class MapGenerationCyberpunk : ScriptableObject
    {
        
        [SerializeField]
        private float coolFloat;
        public float publicFloat;
        public void OnGui()
        {
            coolFloat = EditorGUILayout.Slider(coolFloat, 0, 10);
        }

        
    }
}