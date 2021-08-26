using System;
using UnityEngine;


namespace GunMods
{
    [Serializable]
    public enum stat
    {
        fireRate,
        stability
    }
    [Serializable]
    public class StatEffect
    {
        public stat Stat;
        public float LowerBound;
        public float UpperBound;
        [HideInInspector]
        public AnimationCurve Curve;
        
    }
}