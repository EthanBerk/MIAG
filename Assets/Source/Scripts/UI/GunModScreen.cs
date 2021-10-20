using System;
using GunMods;
using Source.Scripts.Player;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class GunModScreen : MonoBehaviour
    {
        private GameObject player;
        private GunModController playerGun;
        private InventoryManger playerInv;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerInv = player.GetComponent<InventoryManger>();
            playerGun = player.GetComponent<GunModController>();
            foreach (var gunMod in playerInv.gunMods)
            {
                
            }
        }
    }
}