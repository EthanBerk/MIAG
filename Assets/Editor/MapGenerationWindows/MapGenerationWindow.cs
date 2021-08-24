using System;
using UnityEditor;
using UnityEngine;

namespace Editor.MapGenerationWindows
{
    public class MapGenerationWindow : EditorWindow
    {
        private MapGenerationCyberpunk m_MapGenerationCyberpunk;
        
        [MenuItem("Window/Map Generation/MapGen")] [SerializeField]
        private static void ShowWindow()
        {
            var window = GetWindow<MapGenerationWindow>();
            window.titleContent = new GUIContent("Map Generation");
            window.Show();
        }

        private void OnEnable()
        {
            if (!m_MapGenerationCyberpunk) MapGenerationCyberpunk.getMapGenrationCyberpunk();
        }

        private void OnGUI()
        {
            if (!m_MapGenerationCyberpunk) m_MapGenerationCyberpunk = MapGenerationCyberpunk.getMapGenrationCyberpunk();
            m_MapGenerationCyberpunk.publicFloat = EditorGUILayout.Slider(m_MapGenerationCyberpunk.publicFloat, 0, 10);
            m_MapGenerationCyberpunk.OnGui();
            if(GUI.changed) m_MapGenerationCyberpunk.Save();
        }
    }
}