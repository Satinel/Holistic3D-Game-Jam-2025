using UnityEngine;

public class Centipede : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f, _minDistance = 0.1f;

    int _waypointIndex = 0;
    Rigidbody2D _rigidbody2D;
    Transform _currentTarget;
    Player _player;
    WaypointManager _waypointManager;

    [SerializeField] Material _headMaterial, _bodyMaterial;
    [SerializeField] Obstacle _obstaclePrefab;
    [SerializeField] Centipede _centipedePrefab;
    [SerializeField] Transform _spawnPoint;
    EnemyHealth _leader;
    Centipede _follower;
    bool _isHead;
    public EnemyHealth EnemyHealth { get; private set; }

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        EnemyHealth = GetComponent<EnemyHealth>();
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


        transform.right = _currentTarget.position - transform.position;

        _rigidbody2D.linearVelocity = transform.right * _moveSpeed;

        if(Vector2.Distance(transform.position, _currentTarget.position) <= _minDistance)
        {
            _currentTarget = null;
            _waypointIndex++;
        }
    }

    void EnemyHealth_OnAnyEnemyDestroyed(EnemyHealth enemyHealth)
    {
        if(enemyHealth == EnemyHealth)
        {
            Instantiate(_obstaclePrefab, transform.position, Quaternion.identity);
            _follower = null;
            _leader = null;
        }
        else if(enemyHealth == _leader)
        {
            SetAsHead();
        }
        else if(_follower && enemyHealth == _follower.EnemyHealth)
        {
            _follower = null;
        }
    }

    void Player_OnWaypointSet(Vector2 _)
    {
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

    public void Setup(Player player, WaypointManager waypointManager, int waypointIndex)
    {
        _player = player;
        _waypointManager = waypointManager;
        _waypointIndex = waypointIndex;
    }

    public void SetAsHead()
    {
        _leader = null;
        _isHead = true;
        GetComponentInChildren<MeshRenderer>().material = _headMaterial;
    }

    public void SetFollower(Centipede follower)
    {
        _follower = follower;
        _follower.transform.position = _spawnPoint.position;
        _follower.GetComponentInChildren<MeshRenderer>().material = _bodyMaterial;
    }

    public void SetLeader(EnemyHealth leader)
    {
        _leader = leader;
    }

    public void Grow()
    {
        if(!_follower)
        {
            Centipede newSegment = Instantiate(_centipedePrefab, transform.parent);

            newSegment.SetLeader(GetComponent<EnemyHealth>());
            newSegment.Setup(_player, _waypointManager, _waypointIndex);

            SetFollower(newSegment);
        }
        else
        {
            _follower.Grow();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(_isHead && collision.gameObject.GetComponent<Lemming>())
        {
            Destroy(collision.gameObject);
            Grow();
        }
    }
}
