using System;
using GunMods;
using Objects;
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
        

        private GunMod _gunMod;
        private Sprite _sprite;
        private Texture2D _workTexture2D;
        private Color[] _spriteColors;

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            _gunMod = EditorGUILayout.ObjectField(_gunMod, typeof(GunMod), false) as GunMod;
            if (EditorGUI.EndChangeCheck())
            {
                _sprite = _gunMod.LargeSprite;
                var texture = Instantiate(_sprite.texture);
                var spriteTexture = Sprite.Create(texture, _sprite.textureRect, Vector2.zero, 16).texture;
                _spriteColors = spriteTexture.GetPixels(0,0, (int)_sprite.textureRect.width, (int)_sprite.textureRect.height);
                _workTexture2D = new Texture2D(spriteTexture.width + 2, spriteTexture.height + 2);
            }
            if (_gunMod is null) return;
            
            EditorUtils.clear(ref _workTexture2D);
            _workTexture2D.SetPixels(1,1, (int)_sprite.textureRect.width, (int)_sprite.textureRect.height, _spriteColors);
            _workTexture2D.Apply();
            updatePixels(_gunMod.attachmentArea, ref _workTexture2D, Color.red);

            var rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));
            var smallerDistance = Mathf.FloorToInt((rect.width / _sprite.textureRect.width < rect.height / _sprite.textureRect.height)? rect.width / _sprite.textureRect.width : rect.height / _sprite.textureRect.height);
            var adjRect = new Rect(rect.position, new Vector2(smallerDistance * _sprite.textureRect.width, smallerDistance * _sprite.textureRect.height));
            GUI.DrawTexture(adjRect, _workTexture2D);
            
            
            var mouseRect = new Rect(adjRect.x, adjRect.y, adjRect.width + 2 * smallerDistance, adjRect.height + 2 * smallerDistance);
            var m_Event = Event.current;
            if (m_Event.type == EventType.MouseDown && mouseRect.Contains(m_Event.mousePosition))
            {
                var row = Mathf.Abs(Mathf.RoundToInt((adjRect.y - m_Event.mousePosition.y) / smallerDistance));
                var col = Mathf.Abs(Mathf.RoundToInt((m_Event.mousePosition.x - adjRect.x) / smallerDistance));
                _gunMod.attachmentArea[row, col] = m_Event.button == 0;
            }
        }

        private void updatePixels(Serializable2DArray<bool> states, ref Texture2D texture2D, Color color)
        {
            for (var col = 0; col < states.Cols; col++)
            {
                for (var row = 0; row < states.Rows; row++)
                {
                    try
                    {
                        if(states[row - 1, col -1])texture2D.SetPixel(col, row, color);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    
                }
            }
            texture2D.Apply();
        }
    }
}