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

        private void OnGUI()
        {
            _sprite = EditorGUILayout.ObjectField(_sprite, typeof(Sprite), false) as Sprite;
            var rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));
            var smallerDistance = (rect.width < rect.height)? rect.width : rect.height;
            var adjRect = new Rect(rect.position, new Vector2(smallerDistance, smallerDistance));
            var spriteTexture = Sprite.Create(_sprite.texture, _sprite.textureRect, Vector2.zero, 16).texture;
            var spriteSize  = _sprite.bounds.size;
            var finalTexture = new Texture2D(Mathf.RoundToInt(spriteSize.x) + 2, Mathf.RoundToInt(spriteSize.y) + 2);
            // var pixels = spriteTexture.GetPixels(  
            //     (int)_sprite.textureRect.x, 
            //     (int)_sprite.textureRect.y, 
            //     (int)_sprite.textureRect.width, 
            //     (int)_sprite.textureRect.height );
            // finalTexture.SetPixels(pixels);
            // finalTexture.Apply();
            
            GUI.DrawTexture(_sprite.textureRect, spriteTexture);
        }
    }
}