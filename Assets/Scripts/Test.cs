using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f, _rowDistance = 0.5f;
    [SerializeField] Ball _ballPrefab;
    [SerializeField] Transform _ballSpawnPoint;
    [SerializeField] float _ballForce;

    Vector2 _direction = Vector2.right;
    Rigidbody2D _rigidbody2D;
    Ball _activeBall;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _direction = Vector2.zero;

        if(Input.GetKey(KeyCode.W))
        {
            _direction = Vector2.up;
        }
        if(Input.GetKey(KeyCode.A))
        {
            _direction = Vector2.left;
        }
        if(Input.GetKey(KeyCode.S))
        {
            _direction = Vector2.down;
        }
        if(Input.GetKey(KeyCode.D))
        {
            _direction = Vector2.right;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!_activeBall)
            {
                _activeBall = Instantiate(_ballPrefab, _ballSpawnPoint.position, Quaternion.identity);
                _activeBall.Launch();
            }
        }
        
        _rigidbody2D.linearVelocity = _moveSpeed * _direction;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            DropDownOneRow();
        }
        if(collision.gameObject.CompareTag("Dirt"))
        {
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Ball"))
        {
            Destroy(collision.gameObject);
        }
    }

    void DropDownOneRow()
    {
        _rigidbody2D.MovePosition(new(transform.position.x, transform.position.y - _rowDistance));
        _direction = -_direction;
    }
}
