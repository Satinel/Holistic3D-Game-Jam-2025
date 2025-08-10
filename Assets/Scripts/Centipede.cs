using UnityEngine;
using System.Collections.Generic;

public class Centipede : MonoBehaviour
{
    [SerializeField] int _totalSegments;
    [SerializeField] Vector2 _spawnPosition = new();
    [SerializeField] Enemy _enemyPrefab;

    Enemy _head;
    List<Enemy> _segments = new();

    void Start()
    {
        SpawnCentipede();
    }

    void OnEnable()
    {
        Enemy.OnAnyEnemyDestroyed += Enemy_OnAnyEnemyDestroyed;
    }

    void OnDisable()
    {
        Enemy.OnAnyEnemyDestroyed -= Enemy_OnAnyEnemyDestroyed;
    }

    void Enemy_OnAnyEnemyDestroyed(Enemy enemy)
    {
        _segments.Remove(enemy);
    }

    void SpawnCentipede()
    {
        for(int i = 0; i < _totalSegments; i++)
        {
            Enemy newEnemy = Instantiate(_enemyPrefab, new(_spawnPosition.x - i, _spawnPosition.y), Quaternion.identity, transform);
            newEnemy.name = $"Segment {i}";

            if(_segments.Count == 0)
            {
                newEnemy.SpawnAsHead();
                _head = newEnemy;
            }
            else
            {
                newEnemy.SetHead(_head);
            }
            
            foreach(Enemy segment in _segments)
            {
                segment.AddFollower(newEnemy);
            }

            _segments.Insert(i, newEnemy);
        }

        foreach(Enemy segment in _segments)
        {
            segment.Activate();
        }
    }


}
