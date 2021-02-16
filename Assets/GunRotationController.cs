using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotationController : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera _mainCamera;
    void Start()
    {
        _mainCamera = Camera.main;
        
    }
    public float damping = 0.1f;
    public float testRotation;

    // Update is called once per frame
    void Update()
    {
        var worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var lookPos = worldPosition - transform.position;
        lookPos.Normalize();
        float Rotation = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        testRotation = Rotation;
        transform.rotation = Quaternion.Euler(0f,0f,Rotation - 90);
    }
}
