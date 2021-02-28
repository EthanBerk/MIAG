using System;
using System.Transactions;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GunMods.Cyberpunk
{
    public class CyberpunkGun : RaycastProjectileGunModBase, IRaycastProjectileGunMod
    {
        public ParticleSystem projectileParticleSystem;
        public override void Start()
        {
            base.Start();
            projectiles = null;
            var projectile = Instantiate(projectileParticleSystem, shootPoint, gunRotationPoint);
            projectiles = projectile;

        }

        public override void Update()
        {
            projectileParticleSystem.Play();
            base.Update();
            if(Input.GetMouseButtonUp(0))
            {
                var projectilesEmission = projectiles.emission;
                projectilesEmission.enabled = false;

            }
        }






        public float range;
        public LayerMask GunLayerMask;
        private ParticleSystem projectiles;
        public void OnShoot()
        {
            projectiles.transform.position = shootPoint;
            projectiles.transform.rotation = gunRotationPoint;
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