using System.Collections.Generic;
using UnityEngine;

namespace GunMods
{
    [CreateAssetMenu(fileName = "GunMod", menuName = "GunMod", order = 0)]
    public class GunMod : ScriptableObject
    {
        [SerializeField] public Sprite LargeSprite;

        [SerializeField] public Sprite SmallSprite;

        [SerializeField] public List<StatEffect> StatEffects;

    }
}