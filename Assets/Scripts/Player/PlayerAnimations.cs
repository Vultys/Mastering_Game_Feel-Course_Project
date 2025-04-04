using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustVFX;

    [SerializeField] private ParticleSystem _jumpDustVFX;

    [SerializeField] private Transform _characterSpriteTransform;

    [SerializeField] private Transform _cowboyHatSpriteTransform;

    [SerializeField] private float _tiltAngle = 20f;

    [SerializeField] private float _tiltSpeed = 5f;

    [SerializeField] private float _cowboyHatTiltModifier = 2f;

    [SerializeField] private float _yLandVelocityCheck = -20f;

    private Vector2 _velocityBeforePhysicsUpdate;

    private Rigidbody2D _rigidbody;

    private CinemachineImpulseSource _impulseSource;

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable() 
    {
        PlayerController.OnJump += PlayJumpParticles;   
    }

    private void OnDisable() 
    {
        PlayerController.OnJump -= PlayJumpParticles;    
    }

    private void Update() 
    {
        DetectMoveDust();
        ApplyTilt();
    }

    private void FixedUpdate() 
    {
        _velocityBeforePhysicsUpdate = _rigidbody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(_velocityBeforePhysicsUpdate.y < _yLandVelocityCheck)
        {
            PlayJumpParticles();
            _impulseSource.GenerateImpulse();
        }    
    }

    private void DetectMoveDust()
    {
        if(PlayerController.Instance.CheckGrounded())
        {
            if(!_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Play();
            }
        }
        else
        {
            if(_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Stop();
            }
        } 
    }

    private void ApplyTilt()
    {
        float targetAngle;

        if(PlayerController.Instance.MoveInput.x < 0f)
        {
            targetAngle = _tiltAngle;
        }
        else if(PlayerController.Instance.MoveInput.x > 0f) 
        {
            targetAngle = -_tiltAngle;
        }
        else 
        {
            targetAngle = 0f;
        }

        Quaternion currentCharacterRotation = _characterSpriteTransform.rotation;
        Quaternion targetCharacterRotation = Quaternion.Euler(currentCharacterRotation.eulerAngles.x, currentCharacterRotation.eulerAngles.y, targetAngle);

        _characterSpriteTransform.rotation = Quaternion.Lerp(currentCharacterRotation, targetCharacterRotation, _tiltSpeed * Time.deltaTime);
        
        Quaternion currentHatRotation = _cowboyHatSpriteTransform.rotation;
        Quaternion targetHatRotation = Quaternion.Euler(currentHatRotation.eulerAngles.x, currentHatRotation.eulerAngles.y, -targetAngle / _cowboyHatTiltModifier);

        _cowboyHatSpriteTransform.rotation = Quaternion.Lerp(currentHatRotation, targetHatRotation, _cowboyHatTiltModifier * _tiltSpeed * Time.deltaTime);
    }

    private void PlayJumpParticles()
    {
        _jumpDustVFX.Play();
    }
}
