using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] Rigidbody2D _rigidbody2D;

    bool _shouldMove;

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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Lemming lemming))
        {
            lemming.Kill();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        transform.right = -transform.right;
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
