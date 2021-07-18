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

        public static MapGenerationCyberpunk getMapGenrationCyberpunk()
        {
            var mapGenerationCyberpunk = AssetDatabase.LoadAssetAtPath<MapGenerationCyberpunk>("Assets/Source/MapGeneration/Office/objects/mapGenerationCyberpunk.asset");
            if (mapGenerationCyberpunk) return mapGenerationCyberpunk;
            

            mapGenerationCyberpunk = ScriptableObject.CreateInstance<MapGenerationCyberpunk>();
            AssetDatabase.CreateAsset(mapGenerationCyberpunk, "Assets/Source/MapGeneration/Office/objects/mapGenerationCyberpunk.asset");
            
            mapGenerationCyberpunk.Save();
            return mapGenerationCyberpunk;
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        
    }
}