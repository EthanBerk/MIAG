using System;
using UnityEditor;
using UnityEngine;

namespace Editor.MapGenerationWindows
{
    public class MapGenerationWindow : EditorWindow
    {
        private MapGenerationCyberpunk m_MapGenerationCyberpunk;
        
        [MenuItem("Window/Map Generation")] [SerializeField]
        private static void ShowWindow()
        {
            var window = GetWindow<MapGenerationWindow>();
            window.titleContent = new GUIContent("Map Generation");
            window.Show();
        }

        private void OnEnable()
        {
            if (m_MapGenerationCyberpunk == null)
                m_MapGenerationCyberpunk = new MapGenerationCyberpunk();
        }

        private void OnGUI()
        {
            m_MapGenerationCyberpunk.publicFloat = EditorGUILayout.Slider(m_MapGenerationCyberpunk.publicFloat, 0, 10);
            m_MapGenerationCyberpunk.OnGui();
        }
    }
}