using System;
using System.Transactions;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GunMods.Cyberpunk
{
    public class CyberpunkGun : RaycastProjectileGunModBase, IRaycastProjectileGunMod
    {
        public ParticleSystem projectileParticleSystem;
        private ParticleSystem projectiles;
        public override void Start()
        {
            base.Start();
            projectiles = Instantiate(projectileParticleSystem, shootPoint, gunRotationPoint);
            projectiles.Play();
            var projectilesEmission = projectiles.emission;
            projectilesEmission.enabled = false;
            
            

        }

        public override void Update()
        {
            base.Update();
            projectiles.transform.position = shootPoint;
            projectiles.transform.localRotation = gunRotationPoint;
            
            if(Input.GetMouseButtonUp(0))
            {
                var projectilesEmission = projectiles.emission;
                projectilesEmission.enabled = false;

            }
        }






        public float range;
        public LayerMask GunLayerMask;
        
        public void OnShoot()
        {

            var projectilesEmission = projectiles.emission;
            projectilesEmission.enabled = true;





            // var Bullet = Physics2D.Raycast(shootPoint, gunRotationPoint * gameObject.transform.up, range, GunLayerMask);
            // if (Bullet)
            // {
            //     print("hit");
            //     OnHit(Bullet);
            // }

        }
            

        private void OnHit(RaycastHit2D hit2D)
        {
            
        }
    }
}