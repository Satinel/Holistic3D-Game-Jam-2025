using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action<Vector2> OnWaypointSet;
    public static event Action OnPlayerKilled;

    [SerializeField] float _moveSpeed = 3f, _spawnRadius = 0.8f;
    [SerializeField] Ball _ballPrefab;
    [SerializeField] Transform _ballSpawnPoint;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] float _spriteFlashRate = 0.45f;
    float _flashTimer;

    Vector2 _direction = Vector2.right;
    Vector2 _spawnPosition = new();
    Rigidbody2D _rigidbody2D;
    Ball _activeBall;
    Animator _animator;
    bool _levelStarted, _levelOver;

    static readonly int WALK_HASH = Animator.StringToHash("Player Move");
    static readonly int IDLE_HASH = Animator.StringToHash("Player Idle");

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spawnPosition = transform.position;
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        OnWaypointSet?.Invoke(transform.position);
    }

    void OnEnable()
    {
        LevelManager.OnLevelStarted += LevelManager_OnLevelStarted;
        LevelManager.OnLevelFailed += LevelManager_OnLevelFailed;
        LevelManager.OnLevelWon += LevelManager_OnLevelWon;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= LevelManager_OnLevelStarted;
        LevelManager.OnLevelFailed -= LevelManager_OnLevelFailed;
        LevelManager.OnLevelWon -= LevelManager_OnLevelWon;
    }

    void Update()
    {
        if(_levelOver) { return; }

        if(!_levelStarted)
        {
            _flashTimer += Time.deltaTime;
            if(_flashTimer > _spriteFlashRate)
            {
                _spriteRenderer.enabled = !_spriteRenderer.enabled;
                _flashTimer = 0;
            }

            return;
        }

        Vector2 previousDirection = _direction;

        _direction = Vector2.zero;

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _direction.y = 1;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if(_direction.y == 1)
            {
                _direction.y = 0;
            }
            else
            {
                _direction.y = -1;
            }
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _direction.x = 1;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if(_direction.x == 1)
            {
                _direction.x = 0;
            }
            else
            {
                _direction.x = -1;
            }
        }

        if(_direction != previousDirection)
        {
            OnWaypointSet?.Invoke(transform.position);
            if(previousDirection == Vector2.zero)
            {
                _animator.Play(WALK_HASH);
            }
            else if(_direction == Vector2.zero)
            {
                _animator.Play(IDLE_HASH);
            }
        }

        _rigidbody2D.linearVelocity = _moveSpeed * _direction;

        SetBallSpawnPosition();
        _spriteRenderer.flipY = (_direction.x < 0) || (_direction.x == 0 && _spriteRenderer.flipY);

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if(!_activeBall)
            {
                _activeBall = Instantiate(_ballPrefab, _ballSpawnPoint.position, _ballSpawnPoint.rotation);
                _activeBall.Launch();
            }
        }
    }

    void SetBallSpawnPosition()
    {
        if(_direction == Vector2.zero) { return; }

        float posX = 0;
        float posY = 0;

        if(_direction.x < 0)
        {
            posX = -_spawnRadius;
        }
        if(_direction.x > 0)
        {
            posX = _spawnRadius;
        }
        if(_direction.y < 0)
        {
            posY = -_spawnRadius;
        }
        if(_direction.y > 0)
        {
            posY = _spawnRadius;
        }

        _ballSpawnPoint.localPosition = new(posX, posY);

        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _direction);
        _ballSpawnPoint.rotation = targetRotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(_levelOver) { return; }

        if(collision.gameObject.CompareTag("Dirt"))
        {
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Ball"))
        {
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            OnPlayerKilled?.Invoke();
            gameObject.SetActive(false); // TODO Handle player death better and respawn the player and so on
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Respawn"))
        {
            _rigidbody2D.position = _spawnPosition;
        }
    }

    void LevelManager_OnLevelStarted()
    {
        _levelStarted = true;
        _spriteRenderer.enabled = true;
    }

    void LevelManager_OnLevelFailed()
    {
        _rigidbody2D.linearVelocity = Vector2.zero;
        _levelOver = true;
        enabled = false;
    }

    void LevelManager_OnLevelWon()
    {
        _rigidbody2D.linearVelocity = Vector2.zero;
        _levelOver = true;
        enabled = false;
    }
}