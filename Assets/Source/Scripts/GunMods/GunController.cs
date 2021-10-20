using System;
using System.Collections.Generic;
using GunMods.Cyberpunk;
using UnityEngine;

namespace GunMods
{
    public class GunController : MonoBehaviour
    {
        public List<OperatingGunMod> gunMods;
        void Start()
        {
   
        }


        // Update is called once per frame
        void Update()
        {
            
            
        }
    }
    [Serializable]

    public class OperatingGunMod
    {
        public GunMod gunMod;
        [SerializeField] private Vector2 position;
    }
}
