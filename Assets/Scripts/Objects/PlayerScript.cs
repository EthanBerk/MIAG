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
        
        public float accelerationTimeAirborne = .2f;
        public float accelerationTimeGrounded = .1f;
        
        private float _gravity;
        private float _jumpVelocity;
        
        private float _smoothDampVelocity;
        private void Update()
        {
            var input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
            if (_controller2D.Collisions.Above || _controller2D.Collisions.Below)
            {
                _velocity.y = 0;
            }
            
            if (Input.GetKeyDown (KeyCode.Space) && _controller2D.Collisions.Below) {
                _velocity.y = _jumpVelocity;
            }

            var targetVelocity = input.x * moveSpeed;
            _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocity, ref _smoothDampVelocity,
                (_controller2D.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            
            _velocity.y += _gravity * Time.deltaTime;
            _controller2D.Move(_velocity * Time.deltaTime);
        }
    }
}
