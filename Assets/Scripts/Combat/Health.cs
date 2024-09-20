using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action OnDeath;

    [SerializeField] private GameObject _splaterPrefab;

    [SerializeField] private GameObject _deathVFXPrefab;

    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }

    private void OnEnable() 
    {
        OnDeath += SpawnDeathSplatterPrefab;
        OnDeath += SpawnDeathVFX;
    }

    private void OnDisable()
    {
        OnDeath -= SpawnDeathSplatterPrefab;
        OnDeath -= SpawnDeathVFX;
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    private void SpawnDeathSplatterPrefab()
    {
        GameObject newSplatterPrefab = Instantiate(_splaterPrefab, transform.position, transform.rotation);
        ColorChanger splatterColorChanger = newSplatterPrefab.GetComponent<ColorChanger>();
        splatterColorChanger.SetColor(GetComponent<ColorChanger>().DefaultColor);
    }

    private void SpawnDeathVFX()
    {
        GameObject deathVFX = Instantiate(_deathVFXPrefab, transform.position, transform.rotation);
        ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;
        ps.startColor = GetComponent<ColorChanger>().DefaultColor;
    }
}
