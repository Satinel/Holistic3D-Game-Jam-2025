using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] Rigidbody2D _rigidbody2D;

    void OnEnable()
    {
        Player.OnPlayerKilled += StopMoving;
        LevelManager.OnLevelFailed += StopMoving;
        LevelManager.OnLevelWon += StopMoving;
    }

    void OnDisable()
    {
        Player.OnPlayerKilled -= StopMoving;
        LevelManager.OnLevelFailed += StopMoving;
        LevelManager.OnLevelWon += StopMoving;
    }

    void Update()
    {
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

    void StopMoving()
    {
        _moveSpeed = 0;
    }
}
