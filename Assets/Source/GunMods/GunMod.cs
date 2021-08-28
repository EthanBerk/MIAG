using System.Collections.Generic;
using UnityEngine;

namespace GunMods
{
    
    [CreateAssetMenu(fileName = "GunMod", menuName = "GunMod", order = 0)]
    public class GunMod : ScriptableObject
    {
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