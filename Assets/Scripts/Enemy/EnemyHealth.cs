using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static event Action<EnemyHealth> OnAnyEnemyDestroyed;

    [field:SerializeField] public int ScoreValue { get; private set; } = 50;

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

        gameObject.SetActive(false);
    }

    public void ChangeScoreValue(int amount)
    {
        ScoreValue += amount;
    }
}
