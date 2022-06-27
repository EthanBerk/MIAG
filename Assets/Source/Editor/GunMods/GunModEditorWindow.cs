﻿using System;
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

        private Vector2 scrollPos;
        
        
        private bool _hasInitialized = false;
        private int _currentAttachmentIndex = -1;
        private GunModAttachmentRail _tempCurrentAttachmentRail= new GunModAttachmentRail();

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

                AttachmentRails = GunMod.attachmentArea;
                
                Repaint();
                _hasInitialized = true;
            }
            
            
            var e = Event.current;
            UpdateColor();
            
            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandHeight(true), GUILayout.Width(150));
             _toolBar = EditorGUILayout.Popup(_toolBar, Enum.GetNames(typeof(GunModType)));
             GunMod.gunModType = (GunModType) _toolBar;
             GUILayout.BeginVertical("GroupBox", GUILayout.ExpandHeight(true), GUILayout.Width(150));
             scrollPos =
                 GUILayout.BeginScrollView(scrollPos);
             
             int acc = 0;
             for (var i = 0; i < AttachmentRails.Count; i++)
             {
                 var rail = AttachmentRails[i];
                 if (_currentAttachmentIndex == i)
                 {
                     GUILayout.BeginHorizontal("Box");
                 }
                 else
                 {
                     GUILayout.BeginHorizontal();
                 }
                 
                 ++acc;
                 
                 
                 //rail.AttachmentType = EditorGUILayout.Popup((int) rail.AttachmentType - 1, Enum.GetNames()) + 1;
                 if (GUILayout.Button("Edit"))
                 {
                     if (_currentAttachmentIndex > 0)
                     {
                         if (AttachmentRails[_currentAttachmentIndex].IsEmpty)
                             AttachmentRails.Remove(AttachmentRails[_currentAttachmentIndex]);
                     }
                     
                     
                     ResetPixelsOfRail(_tempCurrentAttachmentRail);
                     _currentAttachmentIndex = AttachmentRails.IndexOf(rail);
                     SetPixelsOfRail(rail, GetColorFromAttachmentType(rail.AttachmentType));
                     Repaint();
                     if (rail.IsEmpty)
                         _tempCurrentAttachmentRail = new GunModAttachmentRail();
                     else
                         _tempCurrentAttachmentRail = new GunModAttachmentRail(rail.LargeSpriteLine,
                             rail.SmallSpriteLine, rail.AttachmentType);
                     
                 }
                 if (GUILayout.Button("-"))
                 {
                     AttachmentRails.Remove(rail);
                     i--;
                 }

                 GUILayout.EndHorizontal();
             }

             GUILayout.EndScrollView();

             GUILayout.EndVertical();
             if (GUILayout.Button("Add"))
             {
                 AttachmentRails.Add(new GunModAttachmentRail());
             }
             if (GUILayout.Button("save"))
             {
                 AttachmentRails[_currentAttachmentIndex].LargeSpriteLine = _tempCurrentAttachmentRail.LargeSpriteLine;
                 AttachmentRails[_currentAttachmentIndex].SmallSpriteLine = _tempCurrentAttachmentRail.SmallSpriteLine;
                 AttachmentRails[_currentAttachmentIndex].IsEmpty = false;
             }
             if (GUILayout.Button("save rails"))
             {
                 GunMod.attachmentArea = AttachmentRails;
                 if (_currentAttachmentIndex > 0)
                 {
                     if (AttachmentRails[_currentAttachmentIndex].IsEmpty)
                         GunMod.attachmentArea.Remove(AttachmentRails[_currentAttachmentIndex]);
                 }
                 
             }
             if (GUILayout.Button("clear rails"))
             {
                 AttachmentRails = new List<GunModAttachmentRail>();
                 _currentAttachmentIndex = -1;
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




            if(_currentAttachmentIndex < 0) return;
            switch (e.type)
            {
                case EventType.MouseDown when (largeSpriteTexRect.Contains(e.mousePosition) || smallSpriteTexRect.Contains(e.mousePosition)):
                {
                    var currentRect = largeSpriteTexRect.Contains(e.mousePosition) ? largeSpriteTexRect : smallSpriteTexRect;
                    var currentTex = largeSpriteTexRect.Contains(e.mousePosition)
                        ? _largeWorkTexture2D
                        : _smallWorkTexture2D;
                    var row = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.y - currentRect.yMin) / (currentRect.height / currentTex.height)));
                    var col = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.x - currentRect.x) / (currentRect.width / currentTex.width)));
                    row = ((currentTex.height - 1) - row);
                    
                    
                    
                    _startPoint = new Vector2(col, row);
                   
                    break;
                }
                case EventType.MouseDrag when (largeSpriteTexRect.Contains(e.mousePosition) || smallSpriteTexRect.Contains(e.mousePosition)):
                {
                    var currentRect = largeSpriteTexRect.Contains(e.mousePosition) ? largeSpriteTexRect : smallSpriteTexRect;
                    var currentTex = largeSpriteTexRect.Contains(e.mousePosition)
                        ? _largeWorkTexture2D
                        : _smallWorkTexture2D;
                    var row = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.y - currentRect.yMin) / (currentRect.height / currentTex.height)));
                    var col = Mathf.Abs(Mathf.FloorToInt((e.mousePosition.x - currentRect.x) / (currentRect.width / currentTex.width)));
                    row = ((currentTex.height - 1) - row);
                    var currentOriginalTex = largeSpriteTexRect.Contains(e.mousePosition)
                        ? _largeOriginalTexture2D
                        : _smallOriginalTexture2D;



                    var currentLine = largeSpriteTexRect.Contains(e.mousePosition)
                        ? _tempCurrentAttachmentRail.LargeSpriteLine
                        : _tempCurrentAttachmentRail.SmallSpriteLine;
                    
                        SetPixelsOfLine(currentLine, ref currentTex, ref currentOriginalTex);
                     var up = Math.Abs(col - _startPoint.x) < Math.Abs(row - _startPoint.y);
                     var length = (int)(up ? row - _startPoint.y : (col - _startPoint.x));

                     if (largeSpriteTexRect.Contains(e.mousePosition) &&
                         _tempCurrentAttachmentRail.SmallSpriteLine.Length != 0) ;
                     {
                         var maxLength = _tempCurrentAttachmentRail.SmallSpriteLine.Length == 0? 20 : Mathf.Abs(_tempCurrentAttachmentRail.SmallSpriteLine.Length * 3);
                         length = Math.Abs(length) > maxLength ? Math.Sign(length) * maxLength : length;
                     }
                     if(!largeSpriteTexRect.Contains(e.mousePosition) && 
                        _tempCurrentAttachmentRail.LargeSpriteLine.Length != 0)
                     {
                         var minLength = (int)Mathf.Floor(Mathf.Abs(_tempCurrentAttachmentRail.LargeSpriteLine.Length / 3));
                         length = Math.Abs(length) < minLength ? Math.Sign(length) * minLength : length;
                     }
                     
                     currentLine = new GunModAttachmentLine(_startPoint, length, up);
                     if (largeSpriteTexRect.Contains(e.mousePosition))
                     {
                         _tempCurrentAttachmentRail.LargeSpriteLine = currentLine;
                     }
                     else
                     {
                         _tempCurrentAttachmentRail.SmallSpriteLine = currentLine;
                     }
                    SetPixelsOfLine(currentLine, ref currentTex, GetColorFromAttachmentType(AttachmentRails[_currentAttachmentIndex].AttachmentType));
                     
                     
                     Repaint();
                    break;
                }
            }
        }

        private void SetPixelsOfLine(GunModAttachmentLine gunModAttachmentLine, ref Texture2D texture2D, Color color)
        {
            var rect = gunModAttachmentLine.Rect;
            texture2D.SetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, PopulateColors(gunModAttachmentLine.Length, color));
            texture2D.Apply();
        }
        private void SetPixelsOfLine(GunModAttachmentLine gunModAttachmentLine, ref Texture2D texture2D, ref Texture2D originalTexture)
        {
            var rect = gunModAttachmentLine.Rect;
            Color[] pixels;
            try
            {
                pixels = originalTexture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            texture2D.SetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, pixels);
            texture2D.Apply();
        }

        private void SetPixelsOfRail(GunModAttachmentRail gunModAttachmentRail, Color color)
        {
            SetPixelsOfLine(gunModAttachmentRail.LargeSpriteLine, ref _largeWorkTexture2D, color);
            SetPixelsOfLine(gunModAttachmentRail.SmallSpriteLine, ref _smallWorkTexture2D, color);
        }
        private void ResetPixelsOfRail(GunModAttachmentRail gunModAttachmentRail)
        {
            SetPixelsOfLine(gunModAttachmentRail.LargeSpriteLine, ref _largeWorkTexture2D, ref _largeOriginalTexture2D);
            SetPixelsOfLine(gunModAttachmentRail.SmallSpriteLine, ref _smallWorkTexture2D, ref _smallOriginalTexture2D);
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
        private Color GetColorFromAttachmentType(GunModType type)
        {
            switch (type)
            {
                case GunModType.Barrel:
                    return Color.blue;
                case GunModType.Base:
                    return Color.red;
                    
                case GunModType.Stock:
                    return Color.yellow;
                default:
                    return Color.blue;
            }
        }

    }
}