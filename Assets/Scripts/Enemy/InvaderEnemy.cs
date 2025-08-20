using UnityEngine;

public class InvaderEnemy : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1.5f;
    [SerializeField] float _speedIncrease = 0.25f, _maxSpeed = 10f;
    [SerializeField] float _rowDistance = 0.5f;
    [SerializeField] float _minX, _maxX, _minY, _maxY;
    [SerializeField] float _shotMin = 1.25f, _shotMax = 3.75f;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _bulletSpawnPoint;
    [SerializeField] Animator _animator;

    Rigidbody2D _rigidbody2D;
    int _verticalDirection = -1;
    bool _shouldMove;
    float _timer, _nextShotTime;

    static readonly int INVADER_TANTRUM_HASH = Animator.StringToHash("Invader Tantrum");

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _nextShotTime = Random.Range(_shotMin, _shotMax);

        _animator.Play(INVADER_TANTRUM_HASH, 0, Random.Range(0, 0.9f));
    }

    void OnEnable()
    {
        LevelManager.OnLevelStarted += LevelManager_OnLevelStarted;
        LevelManager.OnLevelFailed += StopMoving;
        LevelManager.OnLevelWon += StopMoving;
        Player.OnPlayerKilled += StopMoving;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= LevelManager_OnLevelStarted;
        LevelManager.OnLevelFailed -= StopMoving;
        LevelManager.OnLevelWon -= StopMoving;
        Player.OnPlayerKilled -= StopMoving;
    }

    void Update()
    {
        if(!_shouldMove) { return; }

        _rigidbody2D.linearVelocityX = _moveSpeed * transform.right.x;

        if(_rigidbody2D.position.x > _maxX)
        {
            _rigidbody2D.position = new(_maxX, _rigidbody2D.position.y);
            RowChange();
        }
        if(_rigidbody2D.position.x < _minX)
        {
            _rigidbody2D.position = new(_minX, _rigidbody2D.position.y);
            RowChange();
        }

        _timer += Time.deltaTime;
        if(_timer > _nextShotTime)
        {
            Shoot();
        }
    }

    void RowChange()
    {
        transform.right = -transform.right;
        _moveSpeed += _speedIncrease;
        if(_moveSpeed > _maxSpeed)
        {
            _moveSpeed = _maxSpeed;
        }

        _rigidbody2D.MovePosition(new(_rigidbody2D.position.x, _rigidbody2D.position.y + (_rowDistance * _verticalDirection)));
        
        if(_rigidbody2D.position.y > _maxY)
        {
            transform.position = new(_rigidbody2D.position.x, _maxY);
            _verticalDirection = -_verticalDirection;
        }
        if(_rigidbody2D.position.y < _minY)
        {
            transform.position = new(_rigidbody2D.position.x, _minY);
            _verticalDirection = -_verticalDirection;
        }
    }

    void Shoot()
    {
        _timer = 0;
        _nextShotTime = Random.Range(_shotMin, _shotMax);
        Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Dirt"))
        {
            collision.gameObject.SetActive(false);
        }
    }

    void LevelManager_OnLevelStarted()
    {
        _shouldMove = true;
    }

    void StopMoving()
    {
        _shouldMove = false;
        _rigidbody2D.linearVelocityX = 0;
    }
}
