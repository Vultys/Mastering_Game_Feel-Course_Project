 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject _splaterPrefab;

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
            SpawnDeathSplatterPrefab();
            Destroy(gameObject);
        }
    }

    private void SpawnDeathSplatterPrefab()
    {
        GameObject newSplatterPrefab = Instantiate(_splaterPrefab, transform.position, transform.rotation);
        ColorChanger splatterColorChanger = newSplatterPrefab.GetComponent<ColorChanger>();
        splatterColorChanger.SetColor(GetComponent<ColorChanger>().DefaultColor);
    }
}
