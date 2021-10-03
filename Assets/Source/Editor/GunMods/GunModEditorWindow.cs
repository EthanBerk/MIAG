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

        public GunMod GunMod { get; set; }
        private Sprite _largeSprite, _smallSprite;
        private Texture2D _largeWorkTexture2D, _largeOriginalTexture2D, _smallWorkTexture2D, _smallOriginalTexture2D;
        private int _toolBar;
        private Color _currentColor;
        private Vector2 _startPoint;
        
        private GunModAttachmentLine _largeModAttachmentLine;
        private int _requiredLength = 2;
        private bool _hasInitialized = false;

        private List<GunModAttachmentRail> AttachmentRails = new List<GunModAttachmentRail>();

        
        


        private void OnGUI()
        {
            if(GunMod is null) return;
            if (!_hasInitialized)
            {
                _largeSprite = GunMod.LargeSprite;
                var texture = Instantiate(_largeSprite.texture);
                
                
                var spriteTexture = Sprite.Create(texture, _largeSprite.rect, Vector2.zero, 16).texture;
                var spriteColors = spriteTexture.GetPixels((int)_largeSprite.rect.x, (int)_largeSprite.rect.y, (int)_largeSprite.rect.width, (int)_largeSprite.rect.height);
                _largeWorkTexture2D = new Texture2D((int)_largeSprite.rect.width + 2, (int)_largeSprite.rect.height + 2) {filterMode = FilterMode.Point};
                EditorUtils.clear(ref _largeWorkTexture2D);
                _largeWorkTexture2D.SetPixels(1,1, (int)_largeSprite.rect.width, (int)_largeSprite.rect.height, spriteColors);
                _largeWorkTexture2D.Apply();
                _largeOriginalTexture2D = Instantiate(_largeWorkTexture2D);
                
                
                
                _smallSprite = GunMod.SmallSprite;
                
                spriteTexture = Sprite.Create(texture, _smallSprite.rect, Vector2.zero, 16).texture;
                spriteColors = spriteTexture.GetPixels((int)_smallSprite.rect.x, (int)_smallSprite.rect.y, (int)_smallSprite.rect.width, (int)_smallSprite.rect.height);
                _smallWorkTexture2D = new Texture2D((int)_smallSprite.rect.width + 2, (int)_smallSprite.rect.height + 2) {filterMode = FilterMode.Point};
                EditorUtils.clear(ref _smallWorkTexture2D);
                _smallWorkTexture2D.SetPixels(1,1, (int)_smallSprite.rect.width, (int)_smallSprite.rect.height, spriteColors);
                _smallWorkTexture2D.Apply();
                _smallOriginalTexture2D = Instantiate(_smallWorkTexture2D);
                
                Repaint();
                _hasInitialized = true;
            }
            
            
            var e = Event.current;
            UpdateColor();
            
            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandHeight(true), GUILayout.Width(150));
             _toolBar = EditorGUILayout.Popup(_toolBar, Enum.GetNames(typeof(GunModType)));
             GunMod.gunModType = (GunModType) _toolBar;
             GUILayout.BeginVertical("GroupBox", GUILayout.ExpandHeight(true), GUILayout.Width(120));
             
             int acc = 0;
             foreach (var rail in AttachmentRails)
             {
                 GUILayout.BeginHorizontal();
                 ++acc;
                 GUILayout.Label($"Rail :{acc}");
                 GUILayout.EndHorizontal();
             }

             GUILayout.EndVertical();
             if (GUILayout.Button("Add"))
             {
                 AttachmentRails.Add(new GunModAttachmentRail());
             }
             
            GUILayout.EndVertical();
 
            var spriteRect  = GUILayoutUtility.GetRect(GUIContent.none, "GroupBox", GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));
            
            GUILayout.EndHorizontal();
            
            

            var smallSpriteRect = new Rect(spriteRect.x, spriteRect.y + spriteRect.height / 2, spriteRect.width,
                spriteRect.height/2);
            var smallSpriteScale = Mathf.FloorToInt((smallSpriteRect.width / _smallWorkTexture2D.width < smallSpriteRect.height / _smallWorkTexture2D.height)? smallSpriteRect.width / _smallWorkTexture2D.width : smallSpriteRect.height / _smallWorkTexture2D.height);
            var smallSpriteTexRect = new Rect(smallSpriteRect.position, new Vector2(smallSpriteScale * _smallWorkTexture2D.width, smallSpriteScale * _smallWorkTexture2D.height));

            var largeSpriteRect = new Rect(spriteRect.x, spriteRect.y, spriteRect.width,
                     spriteRect.height/2);
            var largeSpriteScale = Mathf.FloorToInt((largeSpriteRect.width / _largeWorkTexture2D.width < largeSpriteRect.height / _largeWorkTexture2D.height)? largeSpriteRect.width / _largeWorkTexture2D.width : largeSpriteRect.height / _largeWorkTexture2D.height);
            var largeSpriteTexRect = new Rect(largeSpriteRect.position, new Vector2(largeSpriteScale * _largeWorkTexture2D.width, largeSpriteScale * _largeWorkTexture2D.height));

            if (Event.current.type == EventType.Repaint)
            {
                Graphics.DrawTexture(largeSpriteTexRect, _largeWorkTexture2D);
                Graphics.DrawTexture(smallSpriteTexRect, _smallWorkTexture2D);
            }





            // if (e.type == EventType.MouseDown && texRect.Contains(e.mousePosition))
            // {
            //     var row = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.y - texRect.yMin) / (texRect.height / _largeWorkTexture2D.height)));
            //     var col = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.x - texRect.x) / (texRect.width / _largeWorkTexture2D.width)));
            //     row = ((_largeWorkTexture2D.height - 1) - row);
            //
            //     _startPoint = new Vector2(col, row);
            //
            // }
            //
            // if (e.type == EventType.MouseDrag && texRect.Contains(e.mousePosition))
            // {
            //     
            //     var row = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.y - texRect.yMin) / (texRect.height / _largeWorkTexture2D.height)));
            //     var col = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.x - texRect.x) / (texRect.width / _largeWorkTexture2D.width)));
            //     row = ((_largeWorkTexture2D.height - 1) - row);
            //
            //
            //
            //
            //     if(_largeModAttachmentLine is {})
            //         SetPixelsOfLine(_largeModAttachmentLine, ref _largeWorkTexture2D, ref _largeOriginalTexture2D);
            //     var up = Math.Abs(col - _startPoint.x) < Math.Abs(row - _startPoint.y);
            //     var length = (int)(up ? row - _startPoint.y : col - _startPoint.x);
            //     length = Math.Abs(length)< _requiredLength ? Math.Sign(length) * 2 : length;
            //     _largeModAttachmentLine = new GunModAttachmentLine(_startPoint, length, up);
            //     SetPixelsOfLine(_largeModAttachmentLine, ref _largeWorkTexture2D, Color.blue);
            //     
            //     
            //     
            //     
            //
            //     
            //     
            //     
            //     //_workTexture2D.SetPixel(col, row, (e.button == 0? _currentColor : _originalTexture2D.GetPixel(col, row)));
            //     
            //     _largeWorkTexture2D.Apply();
            //     Repaint();
            //
            // }
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
            switch (GunMod.gunModType)
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