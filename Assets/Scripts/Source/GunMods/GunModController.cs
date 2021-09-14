using System;
using Attributes;
using UnityEngine;

namespace GunMods
{
    
    
    [CreateAssetMenu(fileName = "GunModController", menuName = "GunMods/GunModController", order = 10)]
    public class GunModController : ScriptableObject
    {
        
        public new string name;
        public string description;
        public GameObject enemyModel;
        public int health = 20;
        public float speed = 2f;
        public float detectRange = 10f;
        public int damage = 1;
    }
}