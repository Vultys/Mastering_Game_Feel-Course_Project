using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Range(0.1f, 1f)]
    [SerializeField] private float _parallaxOffset = 0.8f;

    private Camera _mainCamera;
    private Vector2 _startPosition;
    private Vector2 _travel => (Vector2) _mainCamera.transform.position - _startPosition;

    private void Awake() 
    {
        _mainCamera = Camera.main;    
    }

    private void Start() 
    {
        _startPosition = transform.position;
    }

    private void FixedUpdate() 
    {
        Vector2 newPosition = _startPosition + new Vector2(_travel.x * _parallaxOffset, 0f);
        transform.position = new Vector2(newPosition.x, transform.position.y);    
    }
}
