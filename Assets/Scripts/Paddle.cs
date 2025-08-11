using UnityEngine;
using System;

public class Paddle : MonoBehaviour
{
    public static event Action OnPaddleDestroyed;

    [SerializeField] float _moveSpeed = 10f, _launchForce = 15f;
    [SerializeField] float _minX = -8.35f, _maxX = 8.35f;

    [SerializeField] Ball _ballPrefab;
    [SerializeField] Transform _ballSpawnPoint;
    [SerializeField] GameObject _fakeBall;

    float _xForce = 0;
    bool _canLaunch = true;
    Vector2 _direction = Vector2.zero;
    Rigidbody2D _rigidbody2D;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        Ball.OnBallDestroyed += Ball_OnBallDestroyed;
    }

    void OnDisable()
    {
        Ball.OnBallDestroyed -= Ball_OnBallDestroyed;
    }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        _xForce = 0;
        _direction = Vector2.zero;

        if(Input.GetKey(KeyCode.A))
        {
            _direction = Vector2.left;

            _xForce = -_launchForce;
        }

        if(Input.GetKey(KeyCode.D))
        {
            if(_direction == Vector2.zero)
            {
                _direction = Vector2.right;
                _xForce = _launchForce;
            }
            else
            {
                _direction = Vector2.zero;
                _xForce = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBall();
        }
    }
    
    void Move()
    {
        _rigidbody2D.linearVelocity = _moveSpeed * _direction;

        if(transform.position.x < _minX)
        {
            _rigidbody2D.MovePosition(new(_minX, transform.position.y));
        }
        
        if(transform.position.x > _maxX)
        {
            _rigidbody2D.MovePosition(new(_maxX, transform.position.y));
        }
    }

    void SpawnBall()
    {
        if(!_canLaunch) { return; }

        _canLaunch = false;
        _fakeBall.SetActive(false);
        Ball newBall = Instantiate(_ballPrefab, _ballSpawnPoint.position, Quaternion.identity);
        newBall.Launch(_xForce);
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Debug.Log("DEATH!");
            OnPaddleDestroyed?.Invoke();
        }
    }

    void Ball_OnBallDestroyed()
    {
        _fakeBall.SetActive(true);
        _canLaunch = true;
    }
}
