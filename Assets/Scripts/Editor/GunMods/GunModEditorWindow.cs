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
        private Texture2D _workTexture2D;
        [MenuItem("Window/GunModEditor")]
        private static void ShowWindow()
        {
            var window = GetWindow<GunModEditorWindow>();
            window.titleContent = new GUIContent("Gun Mod Editor");
            window.Show();
            
        }

        private void OnEnable()
        {
            
        }


        private GunMod _gunMod;
        private Sprite _sprite;
        
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
                _workTexture2D = new Texture2D(spriteTexture.width + 2, spriteTexture.height + 2)
                {
                    filterMode = FilterMode.Point
                };

            }
            if (_gunMod is null) return;
            
            //EditorUtils.clear(ref _workTexture2D);
            //_workTexture2D.SetPixels(1,1, (int)_sprite.textureRect.width, (int)_sprite.textureRect.height, _spriteColors);
            _workTexture2D.SetPixels(updatePixels(_gunMod.attachmentArea, ref _workTexture2D, Color.red));
            _workTexture2D.Apply();
            

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

        private Color[] updatePixels(Serializable2DArray<bool> states, ref Texture2D texture2D, Color color)
        {
            var colors = new Color[texture2D.height * texture2D.width];
            for (var acc = 0; acc < _gunMod.attachmentArea.area.Length; acc++)
            {
                var isColor = _gunMod.attachmentArea.area[acc];
                if (isColor)
                    colors[acc] = color;
                else
                    colors[acc] = Color.clear;
            }

            return colors;
        }
        
    }
}