using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraTracking : MonoBehaviour
{
    private MouseBox _mouseBox;
    private Bounds _playerBounds;
    private GameObject _player;
    public Camera mousePositionCamera;

    public Vector2 mouseBoxSize;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _mouseBox = new MouseBox(_playerBounds, mouseBoxSize);
    }

    private void Update()
    {
        _playerBounds = _player.GetComponent<BoxCollider2D>().bounds;
        _mouseBox.Update(_playerBounds, mousePositionCamera.ScreenToWorldPoint(Input.mousePosition), mouseBoxSize);
        Vector3 targetPosition = _mouseBox.veloctiy;
        targetPosition.z = transform.position.z;
        transform.position = targetPosition;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color (1, 0, 0, .5f);
        Gizmos.DrawCube (_playerBounds.center, mouseBoxSize);
        print(_playerBounds.center);
    }
    // Start is called before the first frame update
    struct MouseBox
    {
        public float top, bottom;
        public float left, right;
        public Vector2 center;
        public Vector2 veloctiy;

        public MouseBox(Bounds playerBounds, Vector2 size)
        {
            top = playerBounds.center.y + size.y / 2;
            bottom = playerBounds.center.y - size.y / 2;
            left = playerBounds.center.x - size.x / 2;
            right = playerBounds.center.x + size.x / 2;
            center = playerBounds.center;
            veloctiy = center;
        }

        public void Update(Bounds playerBounds, Vector2 mousePositon , Vector2 size)
        {
            top = playerBounds.center.y + size.y / 2;
            bottom = playerBounds.center.y - size.y / 2;
            left = playerBounds.center.x - size.x / 2;
            right = playerBounds.center.x + size.x / 2;
            center = playerBounds.center;
            veloctiy = center;
            
            if (mousePositon.x > right || mousePositon.x < left || mousePositon.y > top || mousePositon.y < bottom)
            {
                veloctiy = mousePositon - center;
            }
        }
        

    }
}
