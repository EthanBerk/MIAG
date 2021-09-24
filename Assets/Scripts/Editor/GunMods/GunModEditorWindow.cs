using System;
using System.Collections.Generic;
using System.Timers;
using GunMods;
using Objects;
using Unity.Mathematics;
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
        private Sprite _largeSprite, _smallSprite;
        private Texture2D _largeWorkTexture2D, _largeOriginalTexture2D, _smallWorkTexture2D, _smallOriginalTexture2D;
        private int _toolBar;
        private Color _currentColor;
        private Vector2 _startPoint;
        
        private GunModAttachmentLine _largeModAttachmentLine;
        private int _requiredLength = 2;
        
        
        
        
        private void OnGUI()
        {
            
            EditorGUI.BeginChangeCheck();
            _gunMod = EditorGUILayout.ObjectField(_gunMod, typeof(GunMod), false) as GunMod;
            if (EditorGUI.EndChangeCheck())
            {
                _largeSprite = _gunMod.LargeSprite;
                var texture = Instantiate(_largeSprite.texture);
                var spriteTexture = Sprite.Create(texture, _largeSprite.textureRect, Vector2.zero, 16).texture;
                var spriteColors = spriteTexture.GetPixels(0,0, (int)_largeSprite.textureRect.width, (int)_largeSprite.textureRect.height);
                _largeWorkTexture2D = new Texture2D(spriteTexture.width + 2, spriteTexture.height + 2) {filterMode = FilterMode.Point};
                EditorUtils.clear(ref _largeWorkTexture2D);
                _largeWorkTexture2D.SetPixels(1,1, (int)_largeSprite.textureRect.width, (int)_largeSprite.textureRect.height, spriteColors);
                _largeWorkTexture2D.Apply();
                _largeOriginalTexture2D = Instantiate(_largeWorkTexture2D);
                _largeWorkTexture2D.Apply();
                
                _smallSprite = _gunMod.SmallSprite;
                texture = Instantiate(_smallSprite.texture);
                spriteTexture = Sprite.Create(texture, _smallSprite.textureRect, Vector2.zero, 16).texture;
                spriteColors = spriteTexture.GetPixels(0,0, (int)_smallSprite.textureRect.width, (int)_smallSprite.textureRect.height);
                _smallWorkTexture2D = new Texture2D(spriteTexture.width + 2, spriteTexture.height + 2) {filterMode = FilterMode.Point};
                EditorUtils.clear(ref _smallWorkTexture2D);
                _smallWorkTexture2D.SetPixels(1,1, (int)_smallSprite.textureRect.width, (int)_smallSprite.textureRect.height, spriteColors);
                _smallWorkTexture2D.Apply();
                _smallOriginalTexture2D = Instantiate(_smallWorkTexture2D);
                _smallWorkTexture2D.Apply();
                Repaint();
            }
            
            if(_gunMod is null) return;
            var e = Event.current;
            UpdateColor();
            
 
            
 
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandHeight(true), GUILayout.Width(100));
             _toolBar = EditorGUILayout.Popup(_toolBar, Enum.GetNames(typeof(GunModType)));
             
             _gunMod.gunModType = (GunModType) _toolBar;
             
            GUILayout.EndVertical();
 
            
            var rect = GUILayoutUtility.GetRect(GUIContent.none, "GroupBox", GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));
            GUILayout.EndHorizontal();
            
            
            
            var smallerDistance = Mathf.FloorToInt((rect.width / _largeWorkTexture2D.width < rect.height / _largeWorkTexture2D.height)? rect.width / _largeWorkTexture2D.width : rect.height / _largeWorkTexture2D.height);
            var texRect = new Rect(rect.position, new Vector2(smallerDistance * _largeWorkTexture2D.width, smallerDistance * _largeWorkTexture2D.height));


            if (Event.current.type == EventType.Repaint)
            {
                Graphics.DrawTexture(texRect, _largeWorkTexture2D);
            }





            if (e.type == EventType.MouseDown && texRect.Contains(e.mousePosition))
            {
                var row = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.y - texRect.yMin) / (texRect.height / _largeWorkTexture2D.height)));
                var col = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.x - texRect.x) / (texRect.width / _largeWorkTexture2D.width)));
                row = ((_largeWorkTexture2D.height - 1) - row);

                _startPoint = new Vector2(col, row);

            }

            if (e.type == EventType.MouseDrag && texRect.Contains(e.mousePosition))
            {
                
                var row = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.y - texRect.yMin) / (texRect.height / _largeWorkTexture2D.height)));
                var col = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.x - texRect.x) / (texRect.width / _largeWorkTexture2D.width)));
                row = ((_largeWorkTexture2D.height - 1) - row);




                if(_largeModAttachmentLine is {})
                    SetPixelsOfLine(_largeModAttachmentLine, ref _largeWorkTexture2D, ref _largeOriginalTexture2D);
                var up = Math.Abs(col - _startPoint.x) < Math.Abs(row - _startPoint.y);
                var length = (int)(up ? row - _startPoint.y : col - _startPoint.x);
                length = Math.Abs(length)< _requiredLength ? Math.Sign(length) * 2 : length;
                _largeModAttachmentLine = new GunModAttachmentLine(_startPoint, length, up);
                SetPixelsOfLine(_largeModAttachmentLine, ref _largeWorkTexture2D, Color.blue);
                
                
                
                

                
                
                
                //_workTexture2D.SetPixel(col, row, (e.button == 0? _currentColor : _originalTexture2D.GetPixel(col, row)));
                
                _largeWorkTexture2D.Apply();
                Repaint();

            }
        }

        private void SetPixelsOfLine(GunModAttachmentLine gunModAttachmentLine, ref Texture2D texture2D, Color color)
        {
            var rect = gunModAttachmentLine.Rect;
            texture2D.SetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, PopulateColors(gunModAttachmentLine.Length, color));
        }
        private void SetPixelsOfLine(GunModAttachmentLine gunModAttachmentLine, ref Texture2D texture2D, ref Texture2D originalTexture)
        {
            var rect = gunModAttachmentLine.Rect;
            Color[] pixels;
            try
            {
                pixels = _largeOriginalTexture2D.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            texture2D.SetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, pixels);
        }


        

        
        

        private static void UpdatePixels(Serializable2DArray<bool> states, ref Texture2D texture2D, Color color)
        {
            for (var col = 0; col < states.Cols; col++)
            {
                for (var row = 0; row < states.Rows; row++)
                {
                    try
                    {
                        if (states[row, col]) texture2D.SetPixel(col, row, color);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }
            }
        }

        private void UpdateColor()
        {
            switch (_gunMod.gunModType)
            {
                case GunModType.Base:
                    _currentColor = Color.blue;
                    break;
                case GunModType.Stock:
                    _currentColor = Color.red;
                    break;
                case GunModType.Barrel:
                    _currentColor = Color.yellow;
                    break;
                case GunModType.Mod:
                    _currentColor = Color.green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Color[] PopulateColors(int length, Color color)
        {
            var colors = new Color[Math.Abs(length)];
            for (var i = 0; i < Math.Abs(length); ++i)
                colors[i] = color;
            return colors;
        }

    }
}