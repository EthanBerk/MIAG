using System;
using System.Collections.Generic;
using Attributes;
using Objects;
using UnityEngine;

namespace GunMods
{
    
    [CreateAssetMenu(fileName = "GunMod", menuName = "GunMods/GunMod", order = 10)]
    public class GunMod : ScriptableObject
    {
        [Expandable]
        public GunModController gunModController;
        
         public Sprite LargeSprite;

         public Sprite SmallSprite;
         [HideInInspector]public List<Sprite> sprites;

        
        [SerializeField] public List<StatEffect> StatEffects;
        
        
        
        [SerializeField] public List<GunModAttachmentRail> attachmentArea = new List<GunModAttachmentRail>();
        
        
        

        
        public GunModType gunModType;


        public void SetGunModController(GunModController gunModController)
        {
            this.gunModController = gunModController;
        }

    }
    public enum GunModType
    {
        Base,
        Stock,
        Barrel,
        Mod
    }
}