using UnityEngine;
using System;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnAnyEnemyDestroyed;

    [SerializeField] int _hitPoints = 1;
    [SerializeField] float _speedIncrease = 2.5f;
    [SerializeField] float _moveSpeed = 5f, _rowDistance = 0.5f;
    [SerializeField] float _xMax = 10f, _xMin = -10f;
    [SerializeField] bool _isHead;
    [SerializeField] Enemy _head;
    [SerializeField] List<Enemy> _followers;
    [SerializeField] Material _headMaterial;

    [SerializeField] List<Vector2> _wayPoints = new();

    bool _isActive = false;

    void OnEnable()
    {
        OnAnyEnemyDestroyed += Enemy_OnAnyEnemyDestroyed;
        Paddle.OnPaddleDestroyed += Paddle_OnPaddleDestroyed;
    }

    void OnDisable()
    {
        OnAnyEnemyDestroyed -= Enemy_OnAnyEnemyDestroyed;
        Paddle.OnPaddleDestroyed -= Paddle_OnPaddleDestroyed;
    }

    void Enemy_OnAnyEnemyDestroyed(Enemy destroyedEnemy)
    {
        _followers.Remove(destroyedEnemy);
    }

    void Paddle_OnPaddleDestroyed()
    {
        _moveSpeed = 0;
    }

    void Update()
    {
        if(!_isActive) { return; }

        transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
        if(_isHead)
        {
            HeadMovement();
        }
        else
        {
            BodyMovement();
        }
    }

    void HeadMovement()
    {
        if(transform.position.x >= _xMax && _moveSpeed > 0)
        {
            DropDownOneRow();
        }
        if(transform.position.x <= _xMin && _moveSpeed < 0)
        {
            DropDownOneRow();
        }
    }

    void BodyMovement()
    {
        if(_wayPoints.Count <= 0 || _wayPoints[0] == null) { return; }

        if(transform.position.x >= _wayPoints[0].x && _moveSpeed > 0)
        {
            DropDownOneRow();
            return;
        }
        if(transform.position.x <= _wayPoints[0].x && _moveSpeed < 0)
        {
            DropDownOneRow();
            return;
        }
    }

    void DropDownOneRow()
    {
        if(_isHead)
        {
            foreach(Enemy follower in _followers)
            {
                follower.AddToWaypoints(transform.position);
            }
        }
        else
        {
            _wayPoints.RemoveAt(0);
        }
        transform.position = new(transform.position.x, transform.position.y - _rowDistance, transform.position.z);
        _moveSpeed = -_moveSpeed;
    }

    public void AddToWaypoints(Vector2 newWaypoint)
    {
        _wayPoints.Add(newWaypoint);
    }

    public void ClearWaypoints()
    {
        _wayPoints = new();
    }

    public void MakeHead()
    {
        GetComponentInChildren<MeshRenderer>().material = _headMaterial;

        ClearWaypoints();
        IncreaseSpeed(_speedIncrease);

        foreach(Enemy follower in _followers)
        {
            follower.ClearWaypoints();
            follower.SetHead(this);
            follower.IncreaseSpeed(_speedIncrease);
        }

        _isHead = true;
        DropDownOneRow();
    }

    public void IncreaseSpeed(float amount)
    {
        if(_moveSpeed > 0)
        {
            _moveSpeed += amount;
        }
        else
        {
            _moveSpeed -= amount;
        }
    }

    public void SetHead(Enemy newHead)
    {
        _head = newHead;
    }

    public void SpawnAsHead()
    {
        _isHead = true;
        GetComponentInChildren<MeshRenderer>().material = _headMaterial;
    }

    public void AddFollower(Enemy enemy)
    {
        if(enemy == this) { return; }

        _followers.Add(enemy);
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Split(List<Enemy> segments)
    {
        List<Enemy> newSegments = segments;

        foreach(Enemy segment in segments)
        {
            _followers.Remove(segment);
        }

        foreach(Enemy follower in _followers)
        {
            follower.RemoveFollowers(newSegments);
        }
    }

    public void RemoveFollowers(List<Enemy> segments)
    {
        foreach(Enemy segment in segments)
        {
            _followers.Remove(segment);
        }
    }

    public void TakeDamage(int amount)
    {
        _hitPoints -= amount;

        if(_hitPoints <= 0)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        OnAnyEnemyDestroyed?.Invoke(this);

        if(_followers.Count <= 0 || _followers[0] == null)
        {
            Destroy(gameObject);
            return;
        }

        if(!_isHead && _head != null)
        {
            _head.Split(_followers);
        }

        _followers[0].MakeHead();

        Destroy(gameObject);
    }
}
