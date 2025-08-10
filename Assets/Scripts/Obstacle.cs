using UnityEngine;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int _hitPoints = 3;
    [SerializeField] List<Color> _colors = new();
    [SerializeField] SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer.color = _colors[_hitPoints - 1]; // At 0 the object is destroyed
    }

    public void TakeDamage(int amount)
    {
        _hitPoints -= amount;

        if(_hitPoints <= 0)
        {
            HandleDeath();
        }
        else
        {
            _spriteRenderer.color = _colors[_hitPoints - 1];
        }
    }

    void HandleDeath()
    {
        Destroy(gameObject);
    }
}
