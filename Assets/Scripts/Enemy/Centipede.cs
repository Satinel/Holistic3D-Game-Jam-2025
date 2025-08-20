using UnityEngine;

public class Centipede : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f, _minDistance = 0.1f;

    int _waypointIndex = 0;
    Rigidbody2D _rigidbody2D;
    Transform _currentTarget;
    Player _player;
    WaypointManager _waypointManager;

    [SerializeField] Obstacle _obstaclePrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Color _followerColor = Color.yellow, _followerColor2 = Color.green, _headColor = Color.red;
    [SerializeField] SpriteRenderer _spriteRenderer;
    EnemyHealth _leader;
    Centipede _centipedePrefab;
    Centipede _follower;
    bool _isHead, _isEvenSegment, _levelOver;
    public EnemyHealth EnemyHealth { get; private set; }

    static readonly string LEMMING_KILLER_LAYER = "LemmingKiller";
    static readonly int SEGMENT_WALK = Animator.StringToHash("SegmentWalk");

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if(!_spriteRenderer)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        EnemyHealth = GetComponent<EnemyHealth>();
    }

    void OnEnable()
    {
        EnemyHealth.OnAnyEnemyDestroyed += EnemyHealth_OnAnyEnemyDestroyed;
        Player.OnWaypointSet += Player_OnWaypointSet;
        Player.OnPlayerKilled += Player_OnPlayerKilled;
        LevelManager.OnLevelFailed += LevelManager_OnLevelFailed;
        LevelManager.OnLevelWon += LevelManager_OnLevelWon;
    }

    void OnDisable()
    {
        EnemyHealth.OnAnyEnemyDestroyed -= EnemyHealth_OnAnyEnemyDestroyed;
        Player.OnWaypointSet -= Player_OnWaypointSet;
        Player.OnPlayerKilled -= Player_OnPlayerKilled;
        LevelManager.OnLevelFailed -= LevelManager_OnLevelFailed;
        LevelManager.OnLevelWon -= LevelManager_OnLevelWon;
    }

    void Update()
    {
        if(_levelOver || _player.enabled == false)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }

        if(!_waypointManager) { return; }

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

    void Player_OnPlayerKilled()
    {
        _levelOver = true;
    }

    void LevelManager_OnLevelFailed()
    {
        _levelOver = true;
    }

    void LevelManager_OnLevelWon()
    {
        _levelOver = true;
    }

    void UpdateWayPoints()
    {
        _waypointIndex = _waypointManager.Waypoints.Count - 1;
        _currentTarget = _waypointManager.Waypoints[_waypointIndex].transform;
    }

    public void Setup(Centipede prefab, Player player, WaypointManager waypointManager, int waypointIndex, bool isEven)
    {
        _centipedePrefab = prefab;
        _player = player;
        _waypointManager = waypointManager;
        _waypointIndex = waypointIndex;
        _isEvenSegment = isEven;
    }

    public void SetAsHead()
    {
        _leader = null;
        _isHead = true;
        _spriteRenderer.color = _headColor;
        _spriteRenderer.sortingOrder = 2;
        _spriteRenderer.enabled = true;
        GetComponentInChildren<Animator>().SetBool("IsHead", _isHead);
        int layerIdentifier = LayerMask.NameToLayer(LEMMING_KILLER_LAYER);
        gameObject.layer = layerIdentifier;
        gameObject.tag = "Enemy";
        if(EnemyHealth)
        {
            EnemyHealth.ChangeScoreValue(100);
        }
    }

    public void SetFollower(Centipede follower)
    {
        _follower = follower;
        _follower.transform.position = _spawnPoint.position;
        if(_isEvenSegment)
        {
            _follower.GetComponentInChildren<SpriteRenderer>().color = _followerColor2;
            _follower.GetComponentInChildren<Animator>().Play(SEGMENT_WALK, 0, 0.5f);
        }
        else
        {
            _follower.GetComponentInChildren<SpriteRenderer>().color = _followerColor;
            _follower.GetComponentInChildren<Animator>().Play(SEGMENT_WALK, 0, 0f);
        }
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
            newSegment.Setup(_centipedePrefab, _player, _waypointManager, _waypointIndex, !_isEvenSegment);

            SetFollower(newSegment);
            ShowFollower();
        }
        else
        {
            _follower.Grow();
        }
    }

    public void ShowFollower()
    {
        if(_follower)
        {
            _follower.GetComponentInChildren<SpriteRenderer>().enabled = _spriteRenderer.enabled;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(_isHead)
        {
            if(collision.gameObject.TryGetComponent(out Lemming lemming))
            {
                lemming.Kill();
                Grow();
            }
            // if(collision.gameObject.TryGetComponent(out Centipede component)) // This is fun (and works) but it's FAR too easy to abuse through looped waypoints
            // {
            //     if(component != _follower && !component.IsHead)
            //     {
            //         component.GetComponent<EnemyHealth>().TakeDamage(5);
            //     }
            // }
        }
    }
}
