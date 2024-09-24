using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _gunFireCoolDown = 0.5f;
    [SerializeField] private float _muzzleFlashTime = 0.05f;

    private Vector2 _mousePos;
    private float _lastFireTime = 0f;
    private int _startPoolSize = 20;
    private int _maxPoolSize = 40;

    private bool _isGunOnCooldown => Time.time < _lastFireTime;

    private Coroutine _muzzleFlashRoutine;
    private CinemachineImpulseSource _impulseSource;
    private Animator _animator;
    private ObjectPool<Bullet> _bulletPool;

    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake() 
    {
        _animator = GetComponent<Animator>(); 
        _impulseSource = GetComponent<CinemachineImpulseSource>();   
    }

    private void Start() 
    {
        CreateBulletPool();    
    }

    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void OnEnable() 
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
        OnShoot += MuzzleFlash;
    }

    private void OnDisable() 
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= GunScreenShake;
        OnShoot -= MuzzleFlash;
    }

    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && !_isGunOnCooldown) {
            OnShoot?.Invoke();
        }
    }

    private void ShootProjectile()
    {            
        ResetLastFireTime();
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCoolDown;
    }

    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }

    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(CreateNewBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, false, _startPoolSize, _maxPoolSize);
    }

    private Bullet CreateNewBullet() => Instantiate(_bulletPrefab);

    private void OnGetBullet(Bullet bullet) => bullet.gameObject.SetActive(true);
    
    private void OnReleaseBullet(Bullet bullet) => bullet.gameObject.SetActive(false);

    private void OnDestroyBullet(Bullet bullet) => Destroy(bullet);

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void MuzzleFlash()
    {
        if(_muzzleFlashRoutine != null)
        {
            StopCoroutine(_muzzleFlashRoutine);
        }

        _muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine()
    {        
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }
}