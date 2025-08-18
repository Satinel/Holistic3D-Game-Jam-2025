using UnityEngine;
using System;

[RequireComponent(typeof(RainbowCycle))]
public class Ball : MonoBehaviour
{
    public static event Action OnBallDestroyed;

    [SerializeField] int _damage = 1;
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] bool _limitSpeed = true;
    [SerializeField] float _lifeTime = 10f;

    Rigidbody2D _rigidbody2D;
    RainbowCycle _rainbowCycle;
    Material _material;

    bool _hasLaunched = false;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rainbowCycle = GetComponent<RainbowCycle>();
        _material = GetComponent<MeshRenderer>().material;
    }

    void OnEnable()
    {
        Player.OnPlayerKilled += DestroyBall;
        LevelManager.OnLevelFailed += DestroyBall;
        LevelManager.OnLevelWon += DestroyBall;
    }

    void OnDisable()
    {
        Player.OnPlayerKilled -= DestroyBall;
        LevelManager.OnLevelFailed -= DestroyBall;
        LevelManager.OnLevelWon -= DestroyBall;
    }

    void Update()
    {
        _material.color = _rainbowCycle.LerpColor();
    }

    public void Launch()
    {
        if(_hasLaunched) { return; }

        _hasLaunched = true;

        float randomAngle = 0.25f;
        if(UnityEngine.Random.Range(1, 3) == 1)
        {
            randomAngle = -randomAngle;
        }

        _rigidbody2D.linearVelocity = (transform.up + (transform.right * randomAngle)).normalized * _moveSpeed;

        Destroy(gameObject, _lifeTime);
    }

    void DestroyBall()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(_limitSpeed)
        {
            // _rigidbody2D.linearVelocity = Vector2.Reflect(_rigidbody2D.linearVelocity, collision.contacts[0].normal).normalized * _moveSpeed; // https://www.youtube.com/watch?v=Vr-ojd4Y2a4
            _rigidbody2D.linearVelocity = _rigidbody2D.linearVelocity.normalized * _moveSpeed; // Above 'works' for kinematic rigidbody, this works for dynamic rigidbody
        }

        collision.gameObject.TryGetComponent(out EnemyHealth enemyHealth);
        if(enemyHealth)
        {
            enemyHealth.TakeDamage(_damage);
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
