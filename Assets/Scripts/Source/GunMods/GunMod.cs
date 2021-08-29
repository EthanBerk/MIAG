using System.Collections.Generic;
using Attributes;
using UnityEngine;

namespace GunMods
{
    
    [CreateAssetMenu(fileName = "GunMod", menuName = "GunMods/GunMod", order = 10)]
    public class GunMod : ScriptableObject
    {
        [Expandable]
        public GunModController GunModController;
        
        [SerializeField] public Sprite LargeSprite;

        [SerializeField] public Sprite SmallSprite;

        [SerializeField] public List<StatEffect> StatEffects;

        
        public GunModType gunModType;


        public void SetGunModController(GunModController gunModController)
        {
            GunModController = gunModController;
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