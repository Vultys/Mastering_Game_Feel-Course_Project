using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackThrust = 10f;

    [SerializeField] private GameObject _bulletVFX;

    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;

    private Gun _gun;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Instantiate(_bulletVFX, transform.position, Quaternion.identity);
        
        IHitable iHitable = other.GetComponent<IHitable>();
        iHitable?.TakeHit();

        IDamageable iDamageable = other.GetComponent<IDamageable>();
        iDamageable?.TakeDamage(_damageAmount, _knockbackThrust);

        _gun.ReleaseBulletFromPool(this);
    }

    public void Init(Gun gun, Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        _gun = gun;
        transform.position = bulletSpawnPos;
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }
}