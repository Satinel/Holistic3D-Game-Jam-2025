using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action<Vector2> OnWaypointSet;

    [SerializeField] float _moveSpeed = 3f, _spawnRadius = 0.8f;
    [SerializeField] Ball _ballPrefab;
    [SerializeField] Transform _ballSpawnPoint;
    [SerializeField] float _minX, _maxX, _minY, _maxY;

    Vector2 _direction = Vector2.right;
    Rigidbody2D _rigidbody2D;
    Ball _activeBall;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        OnWaypointSet?.Invoke(transform.position);
    }

    void Update()
    {
        Vector2 previousDirection = _direction;

        _direction = Vector2.zero;

        if(Input.GetKey(KeyCode.W))
        {
            if(transform.position.y < _maxY)
            {
                _direction.y = 1;
            }
        }
        if(Input.GetKey(KeyCode.S))
        {
            if(_direction.y == 1)
            {
                _direction.y = 0;
            }
            else
            {
                if(transform.position.y > _minY)
                {
                    _direction.y = -1;
                }
            }
        }
        if(Input.GetKey(KeyCode.D))
        {
            if(transform.position.x < _maxX)
            {
                _direction.x = 1;
            }
        }
        if(Input.GetKey(KeyCode.A))
        {
            if(_direction.x == 1)
            {
                _direction.x = 0;
            }
            else
            {
                if(transform.position.x > _minX)
                {
                    _direction.x = -1;
                }
            }
        }

        if(_direction != previousDirection)
        {
            OnWaypointSet?.Invoke(transform.position);
        }

        _rigidbody2D.linearVelocity = _moveSpeed * _direction;

        SetBallSpawnPosition();

        if(Input.GetKeyDown(KeyCode.Space))
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
            gameObject.SetActive(false); // TODO Handle player death better and respawn the player and so on
        }
    }
}