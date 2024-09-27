using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Action OnExplode;
    public Action OnBeep;

    [SerializeField] private float _launchForce = 15f;
    [SerializeField] private int _damageAmount = 3;
    [SerializeField] private float _knockbackThrust = 10f;
    [SerializeField] private float _torqueAmount = 0.2f;
    [SerializeField] private float _timeBeforeDetonation = 3f;
    [SerializeField] private float _explosionRadius = 1.5f;
    [SerializeField] private float _lightBlinkTime = 0.15f;
    [SerializeField] private int _totalBlinks = 3;
    [SerializeField] private Grenade _prefab;
    [SerializeField] private GameObject _grenadeVFX;
    [SerializeField] private GameObject _grenadeLight;
    [SerializeField] private LayerMask _enemyLayerMask;

    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;

    private CinemachineImpulseSource _impulseSource;

    private int _currentBlinks;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable() 
    {
        OnExplode += Explosion;
        OnExplode += GrenadeScreenShake;
        OnExplode += DamageNearby;
        OnExplode += AudioManager.Instance.Grenade_OnExplode;
        OnBeep += BlinkLight;
        OnBeep += AudioManager.Instance.Grenade_OnBeep;
    }

    private void OnDisable() 
    {
        OnExplode -= Explosion;
        OnExplode -= GrenadeScreenShake;
        OnExplode -= DamageNearby;
        OnExplode -= AudioManager.Instance.Grenade_OnExplode;
        OnBeep -= BlinkLight;
        OnExplode -= AudioManager.Instance.Grenade_OnBeep;
    }
    
    private void Start() 
    {
        StartCoroutine(CountdownExplodeRoutine());    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>())
        {
            OnExplode?.Invoke();
        }
    }

    public void Init(Gun gun, Vector2 grenadeSpawnPos, Vector2 mousePos)
    {
        transform.position = grenadeSpawnPos;
        _fireDirection = (mousePos - grenadeSpawnPos).normalized;
        _rigidBody.AddForce(_fireDirection * _launchForce, ForceMode2D.Impulse);
        _rigidBody.AddTorque(_torqueAmount, ForceMode2D.Impulse);
    }

    private void Explosion()
    {        
        Instantiate(_grenadeVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void GrenadeScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void DamageNearby()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);
        foreach(var hit in hits)
        {
            hit.GetComponent<Health>().TakeDamage(_damageAmount);
        }
    }

    private IEnumerator CountdownExplodeRoutine()
    {
        while (_currentBlinks < _totalBlinks)
        {
            yield return new WaitForSeconds(_timeBeforeDetonation / _totalBlinks);
            OnBeep?.Invoke();
            yield return new WaitForSeconds(_lightBlinkTime);
            _grenadeLight.SetActive(false);
        }

        OnExplode?.Invoke();
    }

    private void BlinkLight()
    {
        _grenadeLight.SetActive(true);
        _currentBlinks++;        
    }
}
