using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float _yForce = 10f;

    CircleCollider2D _circleCollider2D;
    Rigidbody2D _rigidbody2D;

    bool _hasLaunched = false;

    void Awake()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        Paddle.OnPaddleLaunch += Paddle_OnPaddleLaunch;
        Paddle.OnPaddleDestroyed += Paddle_OnPaddleDestroyed;
    }

    void OnDisable()
    {
        Paddle.OnPaddleLaunch -= Paddle_OnPaddleLaunch;
        Paddle.OnPaddleDestroyed -= Paddle_OnPaddleDestroyed;
    }

    void Paddle_OnPaddleLaunch(float xForce)
    {
        if(_hasLaunched) { return; }

        transform.SetParent(null, true);
        _hasLaunched = true;
        _circleCollider2D.enabled = true;
        _rigidbody2D.AddForceY(_yForce, ForceMode2D.Impulse);
        _rigidbody2D.AddForceX(xForce, ForceMode2D.Impulse);
    }

    void Paddle_OnPaddleDestroyed()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject, 0.05f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject, 0.05f);
        }
    }
}
