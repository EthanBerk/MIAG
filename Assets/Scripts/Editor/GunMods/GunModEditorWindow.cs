using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.GunMods
{
    public class GunModEditorWindow : EditorWindow
    {
        [MenuItem("Window/GunModEditor")]
        private static void ShowWindow()
        {
            var window = GetWindow<GunModEditorWindow>();
            window.titleContent = new GUIContent("Gun Mod Editor");
            window.Show();
        }

        private Sprite _sprite;
        private Texture2D _workTexture2D;

        private void OnGUI()
        {
            _sprite = EditorGUILayout.ObjectField(_sprite, typeof(Sprite), false) as Sprite;
            _workTexture2D = Instantiate(_sprite.texture);
            var rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));
            var smallerDistance = Mathf.FloorToInt((rect.width / _sprite.textureRect.width < rect.height / _sprite.textureRect.height)? rect.width / _sprite.textureRect.width : rect.height / _sprite.textureRect.height);
            
            var adjRect = new Rect(rect.position, new Vector2(smallerDistance * _sprite.textureRect.width, smallerDistance * _sprite.textureRect.height));
            var spriteTexture = Sprite.Create(_workTexture2D, _sprite.textureRect, Vector2.zero, 16).texture;
            spriteTexture.Resize(spriteTexture.width + 2, spriteTexture.height + 2);
            spriteTexture.Apply();
            
            spriteTexture.SetPixel(0,0, Color.red);
            spriteTexture.SetPixel(spriteTexture.width,0, Color.red);
            spriteTexture.SetPixel(0,spriteTexture.height, Color.red);
            spriteTexture.SetPixel(spriteTexture.width,spriteTexture.height, Color.red);
            spriteTexture.Apply();
            GUI.DrawTexture(adjRect, spriteTexture);
        }
    }
}