using UnityEngine;

public class ChaserEnemy : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f, _minDistance = 0.1f;

    int _waypointIndex = 0;
    Rigidbody2D _rigidbody2D;
    Transform _currentTarget;
    Player _player;
    WaypointManager _waypointManager;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        Player.OnWaypointSet += Player_OnWaypointSet;
    }

    void OnDisable()
    {
        Player.OnWaypointSet -= Player_OnWaypointSet;
    }

    void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _waypointManager = FindFirstObjectByType<WaypointManager>();
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

    void Player_OnWaypointSet(Vector2 _)
    {
        if(_currentTarget == _player.transform)
        {
            _waypointIndex = _waypointManager.Waypoints.Count - 1;
            _currentTarget = _waypointManager.Waypoints[_waypointIndex].transform;
        }
    }
}
