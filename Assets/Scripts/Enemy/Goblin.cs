using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] Rigidbody2D _rigidbody2D;

    void OnEnable()
    {
        Player.OnPlayerKilled += Player_OnPlayerKilled;
    }

    void OnDisable()
    {
        Player.OnPlayerKilled -= Player_OnPlayerKilled;
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

    void Player_OnPlayerKilled()
    {
        _moveSpeed = 0;
    }
}
