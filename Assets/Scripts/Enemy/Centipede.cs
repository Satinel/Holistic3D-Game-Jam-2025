using UnityEngine;
using System;
using System.Collections.Generic;

public class Centipede : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f, _minDistance = 0.1f, _followDistance = 1f;

    int _waypointIndex = 0;
    Rigidbody2D _rigidbody2D;
    Transform _currentTarget;
    Player _player;
    WaypointManager _waypointManager;

    [SerializeField] Material _headMaterial;
    [SerializeField] Obstacle _obstaclePrefab;
    bool _isHead;
    EnemyHealth _leader;
    Centipede _follower;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        EnemyHealth.OnAnyEnemyDestroyed += EnemyHealth_OnAnyEnemyDestroyed;
        Player.OnWaypointSet += Player_OnWaypointSet;
    }

    void OnDisable()
    {
        EnemyHealth.OnAnyEnemyDestroyed -= EnemyHealth_OnAnyEnemyDestroyed;
        Player.OnWaypointSet -= Player_OnWaypointSet;
    }

    
    void Update()
    {
        if(!_waypointManager || !_player) { return; }

        if(!_currentTarget)
        {
            if(_waypointManager.Waypoints.Count > _waypointIndex)
            {
                _currentTarget = _waypointManager.Waypoints[_waypointIndex].transform;
            }
            else
            {
                _currentTarget = _player.transform;
            }
        }

        if(_isHead)
        {
            transform.right = _currentTarget.position - transform.position;

            _rigidbody2D.linearVelocity = transform.right * _moveSpeed;

            if(Vector2.Distance(transform.position, _currentTarget.position) <= _minDistance)
            {
                _currentTarget = null;
                _waypointIndex++;
            }
        }
        else
        {
            transform.right = _leader.transform.position - transform.position;

            if(Vector2.Distance(transform.position, _leader.transform.position) >= _followDistance)
            {
                _rigidbody2D.linearVelocity = transform.right * _moveSpeed;
            }
            else
            {
                _rigidbody2D.linearVelocity = Vector2.zero;
            }
        }
    }

    void EnemyHealth_OnAnyEnemyDestroyed(EnemyHealth enemyHealth)
    {
        if(enemyHealth == GetComponent<EnemyHealth>())
        {
            _follower.SetWaypoints(_waypointIndex);
        }
        if(enemyHealth == _leader)
        {
            SetAsHead();
        }
    }

    void Player_OnWaypointSet(Vector2 _)
    {
        if(!_isHead) { return; }

        if(_currentTarget == _player.transform)
        {
            Invoke(nameof(UpdateWayPoints), 0.01f);
        }
    }

    void UpdateWayPoints()
    {
        _waypointIndex = _waypointManager.Waypoints.Count - 1;
        _currentTarget = _waypointManager.Waypoints[_waypointIndex].transform;
    }

    public void Setup(Player player, WaypointManager waypointManager)
    {
        _player = player;
        _waypointManager = waypointManager;
    }

    public void SetAsHead()
    {
        _isHead = true;
        GetComponentInChildren<MeshRenderer>().material = _headMaterial;
    }

    public void SetFollower(Centipede follower)
    {
        _follower = follower;
    }

    public void SetLeader(EnemyHealth leader)
    {
        _leader = leader;
    }

    public void SetWaypoints(int index)
    {
        _currentTarget = _waypointManager.Waypoints[index].transform;
    }
}
