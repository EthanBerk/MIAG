using System;
using Controllers;
using interfaces;
using UnityEngine;

namespace Objects
{
    [RequireComponent (typeof (BoxCollisionController2D))]
    public class PlayerScript : MonoBehaviour, IDamageable
    {
        
        private Vector3 _velocity;
        public BoxCollisionController2D BoxCollisionController2D { get; private set; }
        private Animator _animator;
        // Start is called before the first frame update
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            _jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
            BoxCollisionController2D = gameObject.GetComponent<BoxCollisionController2D>();
        }

        // Update is called once per frame

        public float moveSpeed;
        
        public float jumpHeight = 4;
        public float timeToJumpApex = .4f;

        private float health;

        public Vector2 wallJumpAganst;
        public Vector2 wallJumpOff;
        public Vector2 wallJumpLeap;
        private float _wallSlideSpeedMax = 3;
        public float wallStickTime = .25f;
        private float _timeToWallUnstick;
        
        
        public float accelerationTimeAirborne = .2f;
        public float accelerationTimeGrounded = .1f;
        
        private float _gravity;
        private float _jumpVelocity;
        public bool WallSliding { get; private set; }
        
        
        
        private float _smoothDampVelocity;
        private void Update()
        {
            if (health < 0)
            {
                gameObject.SetActive(false);
            }
            var input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
            var targetVelocity = input.x * moveSpeed;
            WallSliding = false;
            
            _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocity, ref _smoothDampVelocity,
                (BoxCollisionController2D.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            
            
            int wallDirectionX = (BoxCollisionController2D.Collisions.Left) ? -1 : 1;
            if ((BoxCollisionController2D.Collisions.Left || BoxCollisionController2D.Collisions.Right) && !BoxCollisionController2D.Collisions.Below &&
                _velocity.y < 0)
            {
                WallSliding = true;
                if (_velocity.y < -_wallSlideSpeedMax)
                {
                    _velocity.y = -_wallSlideSpeedMax;
                }

                if (_timeToWallUnstick > 0)
                {
                    _smoothDampVelocity = 0;
                    _velocity.x = 0;
                    if (input.x != wallDirectionX && input.x != 0)
                    {
                        _timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        _timeToWallUnstick = wallStickTime;
                    }
                    
                }
                else
                {
                    _timeToWallUnstick = wallStickTime;
                }
                

            }
            
            if (BoxCollisionController2D.Collisions.Above || BoxCollisionController2D.Collisions.Below)
            {
                _velocity.y = 0;
            }
            
            if (Input.GetKeyDown (KeyCode.Space)) 
            {
                if (WallSliding)
                {
                    
                    if(input.x == 0)
                    {
                        _velocity.x = -wallDirectionX * wallJumpOff.x;
                        _velocity.y = wallJumpOff.y;
                        
                    }
                    else if(Math.Abs(input.x - (-wallDirectionX)) < 0.1)
                    {
                        _velocity.x = -wallDirectionX * wallJumpLeap.x;
                        _velocity.y = wallJumpLeap.y;
                    }
                    else if (Math.Abs(wallDirectionX - input.x) < 0.1)
                    {
                        _velocity.x = -wallDirectionX * wallJumpAganst.x;
                        _velocity.y = wallJumpAganst.y;
                    }
                    
                }

                if (BoxCollisionController2D.Collisions.Below)
                {
                    _velocity.y = _jumpVelocity;
                }
                
            }
            
            
            

           
            _animator.SetBool("Walking", BoxCollisionController2D.Collisions.Below && input.x != 0);
            _velocity.y += _gravity * Time.deltaTime;
            BoxCollisionController2D.Move(_velocity * Time.deltaTime);
        }

        public void hit(int damage)
        {
            health -= damage;
        }
    }
}
