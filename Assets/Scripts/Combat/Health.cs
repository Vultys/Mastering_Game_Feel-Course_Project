using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject SplatterPrefab => _splaterPrefab;

    public GameObject DeathVFXPrefab => _deathVFXPrefab;

    public static Action<Health> OnDeath;

    [SerializeField] private GameObject _splaterPrefab;

    [SerializeField] private GameObject _deathVFXPrefab;

    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }
    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
