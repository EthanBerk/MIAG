using System;
using UnityEngine;

namespace Controllers
{
    [RequireComponent (typeof (BoxCollider2D))]
    public class Controller2D : MonoBehaviour
    {
        private BoxCollider2D _boxCollider2D;
        public CollisionInfo Collisions;

        // Start is called before the first frame update
        private void Start()
        {
            _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
            UpdateRaySpacing(); 
            UpdateRaycastOrigins();
        }

        // Update is called once per frame
        private void Update()
        {
            
        
        }

        public void Move(Vector3 velocity)
        {
            Collisions.Reset();
            UpdateRaycastOrigins();
            if (velocity.x != 0)
            {
                HorizontalCollisions(ref velocity);
                
            }if (velocity.y != 0)
            {
                VerticalCollisions(ref velocity);
            }
            transform.Translate(velocity);
        }

        public LayerMask collisionLayerMask;
        public float maxClimbAngle = 70;

        private void HorizontalCollisions(ref Vector3 velocity)
        {
            var directionX = Mathf.Sign(velocity.x);
            var rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (var i = 0; i < horizontalRayCount; i++)
            {
                var rayOrigin = (Math.Abs(directionX - (-1)) < 0.1) ? _raycastOrigins.BottomLeft : _raycastOrigins.BottomRight;
                rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayerMask);
                
                Debug.DrawRay(rayOrigin, Vector2.right * (directionX * rayLength),Color.red);
                

                if (!hit) continue;

                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                
                
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
                
                Collisions.Left = Math.Abs(directionX - (-1)) < 0.1;
                Collisions.Right = Math.Abs(directionX - 1) < 0.1;
            }
        }
        private void VerticalCollisions(ref Vector3 velocity)
        {
            var directionY = Mathf.Sign(velocity.y);
            var rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (var i = 0; i < horizontalRayCount; i++)
            {
                var rayOrigin = (Math.Abs(directionY - (-1)) < 0.1) ? _raycastOrigins.BottomLeft : _raycastOrigins.TopLeft;
                rayOrigin += Vector2.right * (_verticalRaySpacing * i);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayerMask);
                
                Debug.DrawRay(rayOrigin, Vector2.up * (directionY * rayLength),Color.red);
               

                if (!hit) continue;
               
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                Collisions.Below = Math.Abs(directionY - (-1)) < 0.1;
                Collisions.Above = Math.Abs(directionY - 1) < 0.1;
                
            }
        }
        
        [SerializeField]
        [Range( 0.001f, 0.3f )]
        private float skinWidth = 0.02f;
        
        private RaycastOrigins _raycastOrigins;

        private void UpdateRaycastOrigins()
        {
            var bounds = _boxCollider2D.bounds;
            bounds.Expand(skinWidth * -2);

            _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            _raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            _raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        private float _horizontalRaySpacing;
        private float _verticalRaySpacing;
        
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

       

        private void UpdateRaySpacing()
        {
            var bounds = _boxCollider2D.bounds;
            bounds.Expand(skinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
            
            _horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            _verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }
        
        private struct RaycastOrigins {
            public Vector2 TopLeft, TopRight;
            public Vector2 BottomLeft, BottomRight;
        }

        public struct CollisionInfo
        {
            public bool Above, Below;
            public bool Left, Right;

            public void Reset()
            {
                Above = Below = false;
                Right = Left = false;
            }
        }
    }
}
