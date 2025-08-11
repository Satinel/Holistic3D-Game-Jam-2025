using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    public static event Action OnBallDestroyed;

    [SerializeField] int _damage = 1;
    [SerializeField] float _moveSpeed = 10f;

    Rigidbody2D _rigidbody2D;

    bool _hasLaunched = false;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        Paddle.OnPaddleDestroyed += Paddle_OnPaddleDestroyed;
    }

    void OnDisable()
    {
        Paddle.OnPaddleDestroyed -= Paddle_OnPaddleDestroyed;
    }

    public void Launch(float xForce)
    {
        if(_hasLaunched) { return; }

        _hasLaunched = true;

        _rigidbody2D.linearVelocity = new Vector2(xForce, 1f) * _moveSpeed;
    }

    void Paddle_OnPaddleDestroyed()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // _rigidbody2D.linearVelocity = Vector2.Reflect(_rigidbody2D.linearVelocity, collision.contacts[0].normal).normalized * _moveSpeed; // https://www.youtube.com/watch?v=Vr-ojd4Y2a4
        _rigidbody2D.linearVelocity = _rigidbody2D.linearVelocity.normalized * _moveSpeed; // Above 'works' for kinematic rigidbody, this works for dynamic rigidbody

        collision.gameObject.TryGetComponent(out Enemy enemy);
        if(enemy)
        {
            enemy.TakeDamage(_damage);
        }

        collision.gameObject.TryGetComponent(out Obstacle obstacle);
        if(obstacle)
        {
            obstacle.TakeDamage(_damage);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Respawn"))
        {
            OnBallDestroyed?.Invoke();

            Destroy(gameObject);
        }
    }
}
