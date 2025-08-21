using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5, _minDistance = 0.15f;
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] Vector3[] _wayPoints;
    [SerializeField] SpriteRenderer _spriteRenderer;

    [SerializeField] bool _shouldMove;
    int _currentWaypointIndex, _nextWaypointIndex;

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

        _rigidbody2D.linearVelocity = transform.right * _moveSpeed;


        if(Vector2.Distance(transform.position, _wayPoints[_currentWaypointIndex]) <= _minDistance)
        {
            _currentWaypointIndex = _nextWaypointIndex;
            _nextWaypointIndex = (_nextWaypointIndex + 1) % _wayPoints.Length;

            transform.right = _wayPoints[_currentWaypointIndex] - transform.position;

            _spriteRenderer.flipY = Vector3.Dot(transform.up, Vector3.up) < 0; // Vector3.Dot is reportedly faster than Vector3.Angle
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Lemming lemming))
        {
            lemming.Kill();
        }
    }

    void LevelManager_OnLevelStarted()
    {
        _shouldMove = true;
    }

    void StopMoving()
    {
        _shouldMove = false;
        _rigidbody2D.linearVelocity = Vector2.zero;
    }

    public void IncreaseMoveSpeed(float amount)
    {
        _moveSpeed += amount;
    }
}
