using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GunMods
{
    public abstract class RaycastProjectileGunModBase : MonoBehaviour
    {
        private GameObject _shootPointGameObject;
        public GameObject gunRotationPointGameObject;

        public virtual void Start()
        {
            _shootPointGameObject = GameObject.Find("FirePoint");
            gunRotationPointGameObject = GameObject.Find("Gun");
            
        }
        public Vector3 shootPoint;
        public Quaternion gunRotationPoint;

        public virtual void Update()
        {
            gunRotationPoint = Quaternion.Euler(0,0, gunRotationPointGameObject.GetComponent<GunRotationController>().realRotation);
            shootPoint = _shootPointGameObject.transform.position;
        }
    }
}