using System;
using System.Collections.Generic;
using System.Linq;
using GunMods.Cyberpunk;
using UnityEngine;

namespace GunMods
{
    public class GunController : MonoBehaviour
    {
        public GunMod gunBase;
        private SpriteRenderer _spriteRenderer;
        private Texture2D _gunTexture = new Texture2D(100, 100);

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            GenerateGun();
        }


        public void GenerateGun()
        {
            var modTexture = gunBase.SmallSprite.texture;
            _gunTexture.SetPixels(0,0,modTexture.height, modTexture.width, modTexture.GetPixels());
            _gunTexture.Apply();
            _spriteRenderer.sprite = Sprite.Create(_gunTexture, new Rect(0,0, modTexture.height, modTexture.width), Vector2.zero, 16);
            
        }
        public void GenerateAttachments(GunMod gunMod)
        {
            if (gunMod.attachmentArea.Any())
            {
                foreach (var attachment in gunMod.attachmentArea)
                {
                    
                }
            }
            else
            {
                return;
            }
        }
    }


}
