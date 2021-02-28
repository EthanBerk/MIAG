using GunMods.Cyberpunk;
using UnityEngine;

namespace GunMods
{
    public class GunController : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public float bulletSpeed = 20;
        public GunManager GunManager;
        private CyberpunkGun _cyberpunkGun;
        
        void Start()
        {
            GunManager = new GunManager();
            _cyberpunkGun = gameObject.GetComponentInChildren<CyberpunkGun>();
        }


        // Update is called once per frame
        void Update()
        {
            
            if (Input.GetMouseButton(0))
            {
                _cyberpunkGun.OnShoot();
            }
        }
    }
}
