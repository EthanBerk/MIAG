﻿using System;
using Controllers;
using UnityEngine;

namespace Objects
{
    [RequireComponent (typeof (Controller2D))]
    public class PlayerScript : MonoBehaviour
    {
        
        private Vector3 _velocity;
        private Controller2D _controller2D;
        // Start is called before the first frame update
        private void Start()
        {
            _gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            _jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
            _controller2D = gameObject.GetComponent<Controller2D>();
        }

        // Update is called once per frame

        public float moveSpeed;
        
        public float jumpHeight = 4;
        public float timeToJumpApex = .4f;

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
        
        
        
        private float _smoothDampVelocity;
        private void Update()
        {
            var input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
            var targetVelocity = input.x * moveSpeed;
            
            _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocity, ref _smoothDampVelocity,
                (_controller2D.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            
            bool wallSliding = false;
            int wallDirectionX = (_controller2D.Collisions.Left) ? -1 : 1;
            if ((_controller2D.Collisions.Left || _controller2D.Collisions.Right) && !_controller2D.Collisions.Below &&
                _velocity.y < 0)
            {
                wallSliding = true;
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
            
            if (_controller2D.Collisions.Above || _controller2D.Collisions.Below)
            {
                _velocity.y = 0;
            }
            
            if (Input.GetKeyDown (KeyCode.Space)) 
            {
                if (wallSliding)
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

                if (_controller2D.Collisions.Below)
                {
                    _velocity.y = _jumpVelocity;
                }
                
            }

           
            
            _velocity.y += _gravity * Time.deltaTime;
            _controller2D.Move(_velocity * Time.deltaTime);
        }
    }
}
