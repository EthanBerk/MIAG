using UnityEditor;
using UnityEngine;

namespace Editor.MapGenerationWindows
{
    public class GenerationGroupEditorWindow : EditorWindow
    {
        private Sprite _sprite;
        [MenuItem("Window/Map Generation/Generation Group Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<GenerationGroupEditorWindow>();
            window.titleContent = new GUIContent("TITLE");
            window.Show();
        }

        private void OnGUI()
        {
            _sprite = EditorGUILayout.ObjectField(_sprite, typeof(Sprite), false) as Sprite;
            var rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));


            var spriteTexture = _sprite.texture;
            
            GUI.DrawTexture(rect, spriteTexture);
        }
    }
}