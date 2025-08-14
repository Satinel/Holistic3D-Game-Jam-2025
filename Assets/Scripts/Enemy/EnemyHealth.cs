using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static event Action<EnemyHealth> OnAnyEnemyDestroyed;

    [SerializeField] int _maxHealth = 1;
    int _currentHealth;

    void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        if(_currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        OnAnyEnemyDestroyed?.Invoke(this);

        gameObject.SetActive(false); // TODO At least play a sound or something! Add to score and so on
    }
}
