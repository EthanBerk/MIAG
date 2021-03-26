using Objects;
using UnityEngine;

namespace GunMods
{
    public class GunRotationController : MonoBehaviour
    {
        private Camera _mainCamera;
        public GameObject _player;
        public PlayerScript _PlayerScript;
        private Bounds _playerBounds;
        private Animator _gunArmAnimator;
        private Animator _otherArmAnimator;
        private Animator _playerAnimator;
    
        void Start()
        {
            _player = gameObject.transform.parent.gameObject;
            _PlayerScript = _player.GetComponent<PlayerScript>();
            _playerAnimator = _player.GetComponent<Animator>();
            _playerBounds = _player.GetComponent<BoxCollider2D>().bounds;
            _gunArmAnimator = GameObject.Find("OtherArm").GetComponent<Animator>();
            _otherArmAnimator = GameObject.Find("GunArm").GetComponent<Animator>();
            _mainCamera = Camera.main;

        }
        public float damping = 0.1f;
        public float reflectidRotation;
        public float realRotation;
        public float Rotation;
    
        public float tolorance;
    
    

        // Update is called once per frame
        void Update()
        {
            
            _playerBounds = _player.GetComponent<BoxCollider2D>().bounds;
            var _flipAxis = _playerBounds.center.x;
        
            var worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var lookPos = worldPosition - transform.position;
            var leftFacing = false;
            _mainCamera.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, _mainCamera.transform.position.z);
        
            lookPos.Normalize();
        
        
            realRotation = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;

            if (worldPosition.x < _flipAxis)
            {
                _player.transform.localScale = new Vector3(-1,_player.transform.localScale.y);
                transform.rotation = Quaternion.Euler(0,0,(((realRotation < 0)? 180 : -180) + realRotation) );
                reflectidRotation = (((realRotation < 0)? 180 : -180) + realRotation) * -1;
                leftFacing = true;


            }
            else
            {
                
                
                _player.transform.localScale = new Vector3(1,_player.transform.localScale.y);
                transform.rotation = Quaternion.Euler(0, 0, realRotation);
                reflectidRotation = realRotation;
            }
            
            
            _gunArmAnimator.SetBool("OuterWallhang", false);
            _otherArmAnimator.SetBool("InnerWallhang", false);
            _playerAnimator.SetBool("NotFacingWall", false);
            _playerAnimator.SetBool("FacingWall", false);

            if (_PlayerScript.WallSliding)
            {
                var collisions = _PlayerScript.Controller2D.Collisions;
                if (collisions.Left == leftFacing)
                {
                    _gunArmAnimator.SetBool("OuterWallhang", true);
                    _playerAnimator.SetBool("FacingWall", true);
                    
                    
                }
                else
                {
                    _otherArmAnimator.SetBool("InnerWallhang", true);
                    _playerAnimator.SetBool("NotFacingWall", true);
                    
                    
                }
                
            }
            _gunArmAnimator.SetFloat("ArmPosition", reflectidRotation);
            _otherArmAnimator.SetFloat("ArmPosition", reflectidRotation);
        
        
        
        
        }
    }
}
